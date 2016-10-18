/* See the file "LICENSE" for the full license governing this code. */

using System.Text;

namespace Bambilight {

    class Utils {

        public static string ConvertByteArrayToString(byte[] ba) {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }
    }
}
