

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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += ApplicationOnThreadException;
            Application.Run(new MainForm());
        }

        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs args)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Sender: {sender}");
            if (sender != null)
            {
                sb.AppendLine($"Sender Type: {sender.GetType().FullName}");
            }
            sb.AppendLine("-------");
            var ex = args.Exception;
            do
            {
                sb.AppendLine($"exception type: {args.Exception.GetType().FullName}");
                sb.AppendLine($"exception message: {args.Exception.Message}");
                sb.AppendLine($"exception stacktrace: {args.Exception.StackTrace}");
                sb.AppendLine("-------");
                ex = ex.InnerException;
            } while (ex != null);

            MessageBox.Show(sb.ToString(), "unhandled exception :-(");
            Application.Exit();
        }
    }
}
