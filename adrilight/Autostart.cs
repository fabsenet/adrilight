

using System.Windows.Forms;
using Microsoft.Win32;

namespace adrilight {

    static class Autostart {

        private static readonly RegistryKey _autostartKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public static void Add() {
            if (!IsStartupItem()) {
                _autostartKey.SetValue("adrilight", Application.ExecutablePath.ToString());
            }
        }

        public static void Remove() {
            if (IsStartupItem()) {
                _autostartKey.DeleteValue("adrilight", false);
            }
        }

        private static bool IsStartupItem() {
            if (null != _autostartKey.GetValue("adrilight")) {
                return true;
            }
            return false;
        }
    }
}
