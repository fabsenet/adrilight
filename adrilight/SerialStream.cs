

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;

namespace adrilight {

    class SerialStream : IDisposable {

       private readonly byte[] _messagePreamble = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 };
        private const int ColorsPerLed = 3;

        private BackgroundWorker _mBackgroundWorker = new BackgroundWorker();
        private Stopwatch _mStopwatch = new Stopwatch();

        private SerialPort _mSerialPort;

        public SerialStream() {
            _mBackgroundWorker.DoWork += mBackgroundWorker_DoWork;
            _mBackgroundWorker.WorkerSupportsCancellation = true;
        }

        public void Start() {
            if (!_mBackgroundWorker.IsBusy) {
                _mBackgroundWorker.RunWorkerAsync();
            }
        }

        public void Stop() {
            if (_mBackgroundWorker.IsBusy) {
                _mBackgroundWorker.CancelAsync();
            }
        }

        private byte[] GetOutputStream() {
            byte[] outputStream;

            int counter = _messagePreamble.Length;
            lock (SpotSet.Lock) {
                outputStream = new byte[_messagePreamble.Length + (Settings.LedsPerSpot * SpotSet.Spots.Length * ColorsPerLed)];
                Buffer.BlockCopy(_messagePreamble, 0, outputStream, 0, _messagePreamble.Length);

                foreach (Spot spot in SpotSet.Spots) {
                    
                    for (int i = 0; i < Settings.LedsPerSpot; i++) {
                        
                        outputStream[counter++] = spot.Blue; // blue
                        outputStream[counter++] = spot.Green; // green
                        outputStream[counter++] = spot.Red; // red
                    }
                }
            }

            return outputStream;
        }

        private void mBackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            try {
                _mSerialPort = new SerialPort(Settings.ComPort, 115200);
                _mSerialPort.Open();

                while (!_mBackgroundWorker.CancellationPending) {
                    _mStopwatch.Start();

                    byte[] outputStream = GetOutputStream();
                    _mSerialPort.Write(outputStream, 0, outputStream.Length);

                    int timespan = Settings.MinimumRefreshRateMs - (int)_mStopwatch.ElapsedMilliseconds;
                    if (timespan > 0) {
                        Thread.Sleep(timespan);
                    }

                    _mStopwatch.Stop();
                    _mStopwatch.Reset();
                }
            } catch (Exception ex) {
                Console.Write(ex);
            } finally {
                if (null != _mSerialPort && _mSerialPort.IsOpen) {
                    _mSerialPort.Close();
                    _mSerialPort.Dispose();
                }
            }

            e.Cancel = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                Stop();
            }
        }
    }
}