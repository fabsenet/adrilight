using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adrilight.Util
{
    interface ISerialPortWrapper : IDisposable
    {
        bool IsOpen { get; }

        void Close();
        void Open();
        void Write(byte[] outputBuffer, int v, int streamLength);
    }
}
