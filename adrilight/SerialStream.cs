

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

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

        private void mBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (String.IsNullOrEmpty(Settings.ComPort)) return;

            try
            {
                const int baudRate = 1000000; // 115200;
                _mSerialPort = new SerialPort(Settings.ComPort, baudRate);
                _mSerialPort.Open();

                while (!_mBackgroundWorker.CancellationPending) {
                    _mStopwatch.Start();

                    byte[] outputStream = GetOutputStream();
                    _mSerialPort.Write(outputStream, 0, outputStream.Length);

                    //ws2812b LEDs need 30 µs = 0.030 ms for each led to set its color so there is a lower minimum to the allowed refresh rate
                    //receiving over serial takes it time as well and the arduino does both tasks in sequence
                    //+1 ms extra safe zone
                    var fastLedTime = (outputStream.Length - _messagePreamble.Length)/3.0*0.030d;
                    var serialTransferTime = outputStream.Length*10.0*1000.0/baudRate;
                    var minTimespan = (int)(fastLedTime + serialTransferTime)+1;

                    int timespan = Math.Max(minTimespan, Settings.MinimumRefreshRateMs - (int)_mStopwatch.ElapsedMilliseconds);
                    if (timespan > 0) {
                        Thread.Sleep(timespan);
                    }

                    _mStopwatch.Stop();
                    _mStopwatch.Reset();
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "error in serial send routine");
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