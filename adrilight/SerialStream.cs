/* See the file "LICENSE" for the full license governing this code. */

using SlimDX;
using SlimDX.Direct3D9;
using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

namespace Bambilight {

    class SerialStream : IDisposable {

       private readonly byte[] MESSAGE_PREAMBLE = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 };
        private const int COLORS_PER_LED = 3;

        private BackgroundWorker mBackgroundWorker = new BackgroundWorker();
        private Stopwatch mStopwatch = new Stopwatch();

        private SerialPort mSerialPort;

        public SerialStream() {
            mBackgroundWorker.DoWork += mBackgroundWorker_DoWork;
            mBackgroundWorker.WorkerSupportsCancellation = true;
        }

        public void Start() {
            if (!mBackgroundWorker.IsBusy) {
                mBackgroundWorker.RunWorkerAsync();
            }
        }

        public void Stop() {
            if (mBackgroundWorker.IsBusy) {
                mBackgroundWorker.CancelAsync();
            }
        }

        private byte[] GetOutputStream() {
            byte[] outputStream;

            int counter = MESSAGE_PREAMBLE.Length;
            lock (SpotSet.Lock) {
                outputStream = new byte[MESSAGE_PREAMBLE.Length + (Settings.LedsPerSpot * SpotSet.Spots.Length * COLORS_PER_LED)];
                Buffer.BlockCopy(MESSAGE_PREAMBLE, 0, outputStream, 0, MESSAGE_PREAMBLE.Length);

                foreach (Spot spot in SpotSet.Spots) {
                    
                    for (int i = 0; i < Settings.LedsPerSpot; i++) {
                        
                        outputStream[counter++] = spot.Brush.Color.B; // blue
                        outputStream[counter++] = spot.Brush.Color.G; // green
                        outputStream[counter++] = spot.Brush.Color.R; // red
                    }
                }
            }

            return outputStream;
        }

        private void mBackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            try {
                mSerialPort = new SerialPort(Settings.ComPort, 115200);
                mSerialPort.Open();

                while (!mBackgroundWorker.CancellationPending) {
                    mStopwatch.Start();

                    byte[] outputStream = GetOutputStream();
                    mSerialPort.Write(outputStream, 0, outputStream.Length);

                    int timespan = Settings.MinimumRefreshRateMs - (int)mStopwatch.ElapsedMilliseconds;
                    if (timespan > 0) {
                        Thread.Sleep(timespan);
                    }

                    mStopwatch.Stop();
                    mStopwatch.Reset();
                }
            } catch (Exception ex) {
                Console.Write(ex);
            } finally {
                if (null != mSerialPort && mSerialPort.IsOpen) {
                    mSerialPort.Close();
                    mSerialPort.Dispose();
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