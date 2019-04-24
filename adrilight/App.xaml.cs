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

namespace adrilight
{   
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App : Application
    {
        private static Mutex _adrilightMutex;

        private static readonly ILogger _log = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs startupEvent)
        {
            ReadVersionDetails();

            if (!ViewModelBase.IsInDesignModeStatic)
            {
                _adrilightMutex = new Mutex(true, "adrilight2");
                if (!_adrilightMutex.WaitOne(TimeSpan.Zero, true))
                {
                    //another instance is already running!
                    MessageBox.Show("There is already an instance of adrilight running. Please start only a single instance at any given time."
                        , "Adrilight is already running!");
                    Shutdown();
                    return;
                }
            }
            SetupDebugLogging();
            SetupLoggingForProcessWideEvents();

            base.OnStartup(startupEvent);

            _log.Debug($"adrilight {VersionNumber}: Main() started.");
            kernel = SetupDependencyInjection(false);

            this.Resources["Locator"] = new ViewModelLocator(kernel);


            UserSettings = kernel.Get<IUserSettings>();

            var isNewVersion = VersionNumber != UserSettings.AdrilightVersion;
            if (!IsPrivateBuild && isNewVersion)
            {
                //place for upgrades of settings between versions
                UserSettings.AdrilightVersion = VersionNumber;
                if(UserSettings.ConfigFileVersion == 1)
                {
                    UserSettings.ConfigFileVersion = 2;

                    //convert from weird legacy led matrix width and height to simple strip length!
                    UserSettings.SpotsX = Math.Max(1, UserSettings.SpotsX);
                    UserSettings.SpotsY = Math.Max(1, UserSettings.SpotsY-2);
                }
            }

            SetupNotifyIcon();

            if (!UserSettings.StartMinimized || isNewVersion)
            {
                OpenSettingsWindow();
            }

            _nightLightDetection = kernel.Get<NightLightDetection>();
            _nightLightDetection.Start();

            kernel.Get<AdrilightUpdater>().StartThread();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _nightLightDetection.Stop();

            base.OnExit(e);
            _adrilightMutex?.Dispose();
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
            kernel.Bind(x => x.FromThisAssembly()
            .SelectAllClasses()
            .InheritedFrom<ISelectableViewPart>()
            .BindAllInterfaces());
            kernel.Bind<NightLightDetection>().ToSelf().InSingletonScope();

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
        private NightLightDetection _nightLightDetection;

        private void OpenSettingsWindow()
        {
            if (_mainForm == null)
            {
                _mainForm = new SettingsWindow();
                _mainForm.Closed += MainForm_FormClosed;
                _mainForm.Show();
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
        }

        private void SetupNotifyIcon()
        {
            var icon = new System.Drawing.Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("adrilight.adrilight_icon.ico"));
            var contextMenu = new System.Windows.Forms.ContextMenu();
            contextMenu.MenuItems.Add(CreateSendingMenuItem());
            
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

        private System.Windows.Forms.MenuItem CreateSendingMenuItem()
        {
            var menuItem = new System.Windows.Forms.MenuItem();
            menuItem.Click += (_, __) => UserSettings.TransferActive = !UserSettings.TransferActive;

            void UpdateMenuItem()
            {
                menuItem.Text = UserSettings.TransferActive ? "Sending Active" : "Sending Disabled";
                menuItem.Checked = UserSettings.TransferActive;
            }

            //initial update
            UpdateMenuItem();

            //update on changed setting
            UserSettings.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(UserSettings.TransferActive)) { UpdateMenuItem(); } };

            return menuItem;
        }

        public static bool IsPrivateBuild { get; private set; }
        public static string VersionNumber { get; private set; }

        private static void ReadVersionDetails()
        {
            if (VersionNumber == null)
            {
                VersionNumber = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
                IsPrivateBuild = VersionNumber == "0.0.0";
                if (IsPrivateBuild)
                {
#if DEBUG
                    VersionNumber = "*private debug build*";
#else
                    VersionNumber = "*private release build*";
#endif
                }
            }
        }

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
