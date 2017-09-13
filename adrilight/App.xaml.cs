using adrilight.ui;
using Microsoft.Win32;
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

using AdriSettings = adrilight.Properties.Settings;

namespace adrilight
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILogger _log = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs startupEvent)
        {
            base.OnStartup(startupEvent);

#if DEBUG
            var config = new LoggingConfiguration();
            var debuggerTarget = new DebuggerTarget() { Layout = "${processtime} ${message:exceptionSeparator=\n\t:withException=true}" };
            config.AddTarget("debugger", debuggerTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, debuggerTarget));

            LogManager.Configuration = config;
#endif
            _log.Debug($"adrilight {VersionNumber}: Main() started.");

            AppDomain.CurrentDomain.UnhandledException +=
    (sender, args) => ApplicationOnThreadException(sender, args.ExceptionObject as Exception);

            Settings.Load();

            DispatcherUnhandledException += (sender, args) => ApplicationOnThreadException(sender, args.Exception);

            Exit += (s, e) => _log.Debug("Application exit!");
            SystemEvents.PowerModeChanged += (s, e) => _log.Debug("Changing Powermode to {0}", e.Mode);

            SetupNotifyIcon();

            //subscribe for changes in the settings
            AdriSettings.Default.PropertyChanged += (s, e) => SpotSet.Refresh();
            //exeucte once to setup the leds
            SpotSet.Refresh();

            //subscribe for changes in the settings
            AdriSettings.Default.PropertyChanged += (s, e) => RefreshCapturingState();
            //exeucte once to start the capturing initially
            RefreshCapturingState();


            //subscribe for changes in the settings
            AdriSettings.Default.PropertyChanged += (s, e) => RefreshTransferState();
            //exeucte once to start the serial stream initially
            RefreshTransferState();


            if (!Settings.StartMinimized)
            {
                OpenSettingsWindow();
            }
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

        private static DesktopDuplicatorReader _desktopDuplicatorReader;
        private static CancellationTokenSource _cancellationTokenSource;

        private static void RefreshCapturingState()
        {

            var isRunning = _cancellationTokenSource != null && _desktopDuplicatorReader != null && _desktopDuplicatorReader.IsRunning;
            var shouldBeRunning = Settings.TransferActive || Settings.OverlayActive;

            if (isRunning && !shouldBeRunning)
            {
                //stop it!
                _log.Debug("stopping the capturing");
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
                _desktopDuplicatorReader = null;
            }
            else if (!isRunning && shouldBeRunning)
            {
                //start it
                _log.Debug("starting the capturing");
                _cancellationTokenSource = new CancellationTokenSource();
                _desktopDuplicatorReader = new DesktopDuplicatorReader();
                var thread = new Thread(() => _desktopDuplicatorReader.Run(_cancellationTokenSource.Token))
                {
                    IsBackground = true,
                    Priority = ThreadPriority.BelowNormal,
                    Name = "DesktopDuplicatorReader"
                };
                thread.Start();
            }
        }

        private static SerialStream _mSerialStream;


        private static void RefreshTransferState()
        {
            if (null == _mSerialStream)
            {
                _mSerialStream = new SerialStream();
            }

            if (Settings.TransferActive && !_mSerialStream.IsRunning)
            {
                //start it
                _log.Debug("starting the serial stream");
                _mSerialStream.Start();
            }
            else if (!Settings.TransferActive && _mSerialStream.IsRunning)
            {
                //stop it
                _log.Debug("stopping the serial stream");
                _mSerialStream.Stop();
            }
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


        private void ApplicationOnThreadException(object sender, Exception ex)
        {
            _log.Fatal(ex, $"DispatcherUnhandledException from sender={sender}, adrilight version={VersionNumber}");

            var sb = new StringBuilder();
            sb.AppendLine($"Sender: {sender}");
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
