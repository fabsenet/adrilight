using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace adrilight {

    static class Program {

        private static readonly ILogger _log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            var config = new LoggingConfiguration();
            var debuggerTarget = new DebuggerTarget();
            config.AddTarget("debugger", debuggerTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, debuggerTarget));

            LogManager.Configuration = config;
#endif

            _log.Debug($"adrilight {VersionNumber}: Main() started.");

            Settings.Load();

            AppDomain.CurrentDomain.UnhandledException += 
                (sender, args) => ApplicationOnThreadException(sender, args.ExceptionObject as Exception);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += (sender, args) => ApplicationOnThreadException(sender, args.Exception);

            Application.Run(new MainForm());
        }

        private static void ApplicationOnThreadException(object sender, Exception ex)
        {
            _log.Fatal(ex, $"ApplicationOnThreadException from sender={sender}, adrilight version={VersionNumber}");

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
            Application.Exit();
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
    }
}
