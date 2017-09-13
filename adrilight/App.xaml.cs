using adrilight.ui;
using Microsoft.Win32;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Wpf.Platform;
using MvvmCross.Wpf.Views.Presenters;
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
using AdriSettings = adrilight.Properties.Settings;

namespace adrilight
{


    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App : Application
    {
        private static readonly ILogger _log = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs startupEvent)
        {
            SetupDebugLogging();
            SetupLoggingForProcessWideEvents();

            base.OnStartup(startupEvent);

            _log.Debug($"adrilight {VersionNumber}: Main() started.");
            SetupDependencyInjection();





            UserSettings = Mvx.Resolve<IUserSettings>();
                        
            SetupNotifyIcon();

            if (!UserSettings.StartMinimized)
            {
                OpenSettingsWindow();
            }
        }

        private void SetupDependencyInjection()
        {
            Mvx.LazyConstructAndRegisterSingleton<IUserSettings, UserSettings>();
            Mvx.LazyConstructAndRegisterSingleton<ISpotSet, SpotSet>();

            Mvx.ConstructAndRegisterSingleton<ISerialStream, SerialStream>();
            Mvx.ConstructAndRegisterSingleton<IDesktopDuplicatorReader, DesktopDuplicatorReader>();
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
            contextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Settings...", (s, e) => OpenSettingsWindow()));
            contextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Exit", (s, e) => Shutdown(0)));

            var notifyIcon = new System.Windows.Forms.NotifyIcon()
            {
                Text = $"adrilight {GetVersionNumber()}",
                Icon = icon,
                Visible = true,
                ContextMenu = contextMenu
            };
            notifyIcon.DoubleClick += (s, e) => { OpenSettingsWindow(); };
            
            Exit += (s, e) => notifyIcon.Dispose();
        }


        public static string VersionNumber { get; } = GetVersionNumber();
        private IUserSettings UserSettings { get; set; }

        private static string GetVersionNumber()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string currentVersion;
            using (var versionStream = assembly.GetManifestResourceStream("adrilight.version.txt"))
            {
                currentVersion = new StreamReader(versionStream).ReadToEnd().Trim();
            }
            return currentVersion;
        }


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
            Shutdown(-1);
        }
    }
}
