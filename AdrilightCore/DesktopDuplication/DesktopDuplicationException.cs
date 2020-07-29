using System;

namespace adrilight.DesktopDuplication
{
    [Serializable]
    public class DesktopDuplicationException : Exception
    {
        public DesktopDuplicationException(string message)
            : base(message) { }
        public DesktopDuplicationException(string message, Exception innerException)
                    : base(message, innerException) { }


    }
}
