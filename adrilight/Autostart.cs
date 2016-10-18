/* See the file "LICENSE" for the full license governing this code. */

using Microsoft.Win32;
using System.Windows.Forms;

namespace Bambilight {

    static class Autostart {

        private static RegistryKey AutostartKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public static void Add() {
            if (!IsStartupItem()) {
                AutostartKey.SetValue("Bambilight", Application.ExecutablePath.ToString());
            }
        }

        public static void Remove() {
            if (IsStartupItem()) {
                AutostartKey.DeleteValue("Bambilight", false);
            }
        }

        private static bool IsStartupItem() {
            if (null != AutostartKey.GetValue("Bambilight")) {
                return true;
            }
            return false;
        }
    }
}
