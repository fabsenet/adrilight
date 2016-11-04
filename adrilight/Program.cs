

using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace adrilight {

    static class Program {

        public static int ScreenWidth => Screen.PrimaryScreen.Bounds.Width;

        public static int ScreenHeight => Screen.PrimaryScreen.Bounds.Height;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main() {
            Settings.Refresh();

            AppDomain.CurrentDomain.UnhandledException += (sender, args) => ApplicationOnThreadException(sender, args.ExceptionObject as Exception);
                Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += (sender,args) => ApplicationOnThreadException(sender, args.Exception);
            Application.Run(new MainForm());
        }

        private static void ApplicationOnThreadException(object sender, Exception ex)
        {
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
    }
}
