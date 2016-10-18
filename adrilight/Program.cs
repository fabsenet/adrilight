/* See the file "LICENSE" for the full license governing this code. */

using System;
using System.Windows.Forms;

namespace Bambilight {

    static class Program {

        public static int ScreenWidth { 
            get {
                return Screen.PrimaryScreen.Bounds.Width;
            } 
        }

        public static int ScreenHeight { 
            get {
                return Screen.PrimaryScreen.Bounds.Height;
            } 
        }

        public static int ScreenPixel { 
            get {
                return ScreenWidth * ScreenHeight;
            } 
        }

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main() {
            Settings.Refresh();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
