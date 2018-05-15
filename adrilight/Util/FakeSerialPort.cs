using NLog;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace adrilight.Util
{
    class FakeSerialPort : ISerialPortWrapper
    {
        private ILogger _log = LogManager.GetCurrentClassLogger();

        public FakeSerialPort() => _log.Warn("FakeSerialPort created!");

        public bool IsOpen { get; private set; }

        public void Open() => IsOpen = true;
        public void Close() => IsOpen = false;

        public void Dispose() {}


        public void Write(byte[] outputBuffer, int v, int streamLength)
        {
            _log.Warn($"Faking writing of {outputBuffer.Length} bytes to the serial port");
            Thread.Sleep(1);
        }
    }
}
