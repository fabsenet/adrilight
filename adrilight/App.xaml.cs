using adrilight.Fakes;
using adrilight.ui;
using adrilight.View;
using adrilight.ViewModel;
using GalaSoft.MvvmLight;
using Microsoft.Win32;
using Ninject;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Ninject.Extensions.Conventions;
using adrilight.Resources;
using adrilight.Util;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights;

namespace adrilight
{   
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App : Application
    {
        private static Mutex _adrilightMutex = new Mutex(true, "adrilight2");

        private static readonly ILogger _log = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs startupEvent)
        {
            if(!_adrilightMutex.WaitOne(TimeSpan.Zero, true))
            {
                //another instance is already running!
                MessageBox.Show("There is already an instance of adrilight running. Please start only a single instance at any given time."
                    , "Adrilight is already running!");
                App.Current.Shutdown();
                return;
            }
            SetupDebugLogging();
            SetupLoggingForProcessWideEvents();

            base.OnStartup(startupEvent);

            _log.Debug($"adrilight {VersionNumber}: Main() started.");
            kernel = SetupDependencyInjection(false);

            this.Resources["Locator"] = new ViewModelLocator(kernel);


            UserSettings = kernel.Get<IUserSettings>();
            _telemetryClient = kernel.Get<TelemetryClient>();

            SetupNotifyIcon();

            if (!UserSettings.StartMinimized)
            {
                OpenSettingsWindow();
            }


            kernel.Get<AdrilightUpdater>().StartThread();

            SetupTrackingForProcessWideEvents(_telemetryClient);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _adrilightMutex.Dispose();
        }

        private TelemetryClient _telemetryClient;

        private static TelemetryClient SetupApplicationInsights(IUserSettings settings)
        {
            const string ik = "65086b50-8c52-4b13-9b05-92fbe69c7a52";
            TelemetryConfiguration.Active.InstrumentationKey = ik;
            var tc = new TelemetryClient
            {
                InstrumentationKey = ik
            };

            tc.Context.User.Id = settings.InstallationId.ToString();
            tc.Context.Session.Id = Guid.NewGuid().ToString();
            tc.Context.Device.OperatingSystem = Environment.OSVersion.ToString();

            GlobalDiagnosticsContext.Set("user_id", tc.Context.User.Id);
            GlobalDiagnosticsContext.Set("session_id", tc.Context.Session.Id);
            return tc;
        }

        internal static IKernel SetupDependencyInjection(bool isInDesignMode)
        {
            var kernel = new StandardKernel();
            if(isInDesignMode)
            {
                //setup fakes
                kernel.Bind<IUserSettings>().To<UserSettingsFake>().InSingletonScope();
                kernel.Bind<IContext>().To<ContextFake>().InSingletonScope();
                kernel.Bind<ISpotSet>().To<SpotSetFake>().InSingletonScope();
                kernel.Bind<ISerialStream>().To<SerialStreamFake>().InSingletonScope();
                kernel.Bind<IDesktopDuplicatorReader>().To<DesktopDuplicatorReaderFake>().InSingletonScope();
            }
            else
            {
                //setup real implementations
                var settingsManager = new UserSettingsManager();
                var settings = settingsManager.LoadIfExists() ?? settingsManager.MigrateOrDefault();
                kernel.Bind<IUserSettings>().ToConstant(settings);

                kernel.Bind<IContext>().To<WpfContext>().InSingletonScope();
                kernel.Bind<ISpotSet>().To<SpotSet>().InSingletonScope();
                kernel.Bind<ISerialStream>().To<SerialStream>().InSingletonScope();
                kernel.Bind<IDesktopDuplicatorReader>().To<DesktopDuplicatorReader>().InSingletonScope();
            }
            kernel.Bind<SettingsViewModel>().ToSelf().InSingletonScope();
            kernel.Bind<TelemetryClient>().ToConstant(SetupApplicationInsights(kernel.Get<IUserSettings>()));
            kernel.Bind(x => x.FromThisAssembly()
            .SelectAllClasses()
            .InheritedFrom<ISelectableViewPart>()
            .BindAllInterfaces());

            //eagerly create required singletons [could be replaced with actual pipeline]
            var desktopDuplicationReader = kernel.Get<IDesktopDuplicatorReader>();
            var serialStream = kernel.Get<ISerialStream>();

            return kernel;
        }

        private void SetupLoggingForProcessWideEvents()
        {
            AppDomain.CurrentDomain.UnhandledException +=
    (sender, args) => ApplicationWideException(sender, args.ExceptionObject as Exception, "CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (sender, args) => ApplicationWideException(sender, args.Exception, "DispatcherUnhandledException");

            Exit += (s, e) => _log.Debug("Application exit!");

            SystemEvents.PowerModeChanged += (s, e) => _log.Debug("Changing Powermode to {0}", e.Mode);
        }

        private void SetupTrackingForProcessWideEvents(TelemetryClient tc)
        {
            if (tc == null)
            {
                throw new ArgumentNullException(nameof(tc));
            }

            AppDomain.CurrentDomain.UnhandledException += (sender, args) => tc.TrackException(args.ExceptionObject as Exception);

            DispatcherUnhandledException += (sender, args) => tc.TrackException(args.Exception);

            Exit += (s, e) => { tc.TrackEvent("AppExit"); tc.Flush(); };

            SystemEvents.PowerModeChanged += (s, e) => tc.TrackEvent("PowerModeChanged", new Dictionary<string, string> { { "Mode", e.Mode.ToString() } });
            tc.TrackEvent("AppStart");
        }

        [System.Diagnostics.Conditional("DEBUG")]
        private void SetupDebugLogging()
        {
            var config = new LoggingConfiguration();
            var debuggerTarget = new DebuggerTarget() { Layout = "${processtime} ${message:exceptionSeparator=\n\t:withException=true}" };
            config.AddTarget("debugger", debuggerTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, debuggerTarget));

            LogManager.Configuration = config;

            _log.Info($"DEBUG logging set up!");
        }

        SettingsWindow _mainForm;
        private IKernel kernel;

        private void OpenSettingsWindow()
        {
            if (_mainForm == null)
            {
                _mainForm = new SettingsWindow();
                _mainForm.Closed += MainForm_FormClosed;
                _mainForm.Show();
                _telemetryClient.TrackEvent("SettingsWindow opened");
            }
            else
            {
                //bring to front?
                _mainForm.Focus();
            }
        }

        private void MainForm_FormClosed(object sender, EventArgs e)
        {
            if (_mainForm == null) return;

            //deregister to avoid memory leak
            _mainForm.Closed -= MainForm_FormClosed;
            _mainForm = null;
            _telemetryClient.TrackEvent("SettingsWindow closed");
        }

        private void SetupNotifyIcon()
        {
            var icon = new System.Drawing.Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("adrilight.adrilight_icon.ico"));
            var contextMenu = new System.Windows.Forms.ContextMenu();
            contextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Settings...", (s, e) => OpenSettingsWindow()));
            contextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Exit", (s, e) => Shutdown(0)));

            var notifyIcon = new System.Windows.Forms.NotifyIcon()
            {
                Text = $"adrilight {VersionNumber}",
                Icon = icon,
                Visible = true,
                ContextMenu = contextMenu
            };
            notifyIcon.DoubleClick += (s, e) => { OpenSettingsWindow(); };
            
            Exit += (s, e) => notifyIcon.Dispose();
        }


        public static string VersionNumber { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        private IUserSettings UserSettings { get; set; }

        private void ApplicationWideException(object sender, Exception ex, string eventSource)
        {
            _log.Fatal(ex, $"ApplicationWideException from sender={sender}, adrilight version={VersionNumber}, eventSource={eventSource}");

            var sb = new StringBuilder();
            sb.AppendLine($"Sender: {sender}");
            sb.AppendLine($"Source: {eventSource}");
            if (sender != null)
            {
                sb.AppendLine($"Sender Type: {sender.GetType().FullName}");
            }
            sb.AppendLine("-------");
            do
            {
                sb.AppendLine($"exception type: {ex.GetType().FullName}");
                sb.AppendLine($"exception message: {ex.Message}");
                sb.AppendLine($"exception stacktrace: {ex.StackTrace}");
                sb.AppendLine("-------");
                ex = ex.InnerException;
            } while (ex != null);

            MessageBox.Show(sb.ToString(), "unhandled exception :-(");
            try
            {
                Shutdown(-1);
            }
            catch
            {
                Environment.Exit(-1);
            }
        }
    }
}
