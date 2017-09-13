using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace adrilight
{
    internal sealed class SerialStream : IDisposable, ISerialStream
    {
        private ILogger _log = LogManager.GetCurrentClassLogger();

        public SerialStream(IUserSettings userSettings, ISpotSet spotSet)
        {
            UserSettings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));
            SpotSet = spotSet ?? throw new ArgumentNullException(nameof(spotSet));

            UserSettings.PropertyChanged += UserSettings_PropertyChanged;
            _log.Info($"SerialStream created.");
        }

        private void UserSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(UserSettings.TransferActive):
                    RefreshTransferState();
                    break;
            }
        }

        private void RefreshTransferState()
        {
            if (UserSettings.TransferActive && !IsRunning)
            {
                //start it
                _log.Debug("starting the serial stream");
                Start();
            }
            else if (!UserSettings.TransferActive && IsRunning)
            {
                //stop it
                _log.Debug("stopping the serial stream");
                Stop();
            }
        }

        private readonly byte[] _messagePreamble = {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09};


        private Thread _workerThread;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly Stopwatch _stopwatch = new Stopwatch();


        public void Start()
        {
            _log.Debug("Start called.");
            if (_workerThread != null) return;

            _cancellationTokenSource = new CancellationTokenSource();
            _workerThread = new Thread(DoWork)
            {
                Name = "Serial sending",
                IsBackground = true
            };
            _workerThread.Start(_cancellationTokenSource.Token);
        }

        public void Stop()
        {
            _log.Debug("Stop called.");
            if (_workerThread == null) return;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
            _workerThread?.Join();
            _workerThread = null;
        }

        public bool IsRunning => _workerThread != null && _workerThread.IsAlive;

        private IUserSettings UserSettings { get; }
        private ISpotSet SpotSet { get; }

        private byte[] GetOutputStream()
        {
            byte[] outputStream;

            int counter = _messagePreamble.Length;
            lock (SpotSet.Lock)
            {
                const int colorsPerLed = 3;
                outputStream = new byte[_messagePreamble.Length + (UserSettings.LedsPerSpot*SpotSet.Spots.Length*colorsPerLed)];
                Buffer.BlockCopy(_messagePreamble, 0, outputStream, 0, _messagePreamble.Length);

                foreach (Spot spot in SpotSet.Spots)
                {
                    for (int i = 0; i < UserSettings.LedsPerSpot; i++)
                    {
                        outputStream[counter++] = spot.Blue; // blue
                        outputStream[counter++] = spot.Green; // green
                        outputStream[counter++] = spot.Red; // red
                    }
                }
            }

            return outputStream;
        }

        private void DoWork(object tokenObject)
        {
            var cancellationToken = (CancellationToken) tokenObject;
            SerialPort serialPort = null;

            if (String.IsNullOrEmpty(UserSettings.ComPort))
            {
                return;
            }

            //retry after exceptions...
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    const int baudRate = 1000000;
                    string openedComPort = null;

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        //open or change the serial port
                        if (openedComPort != UserSettings.ComPort)
                        {
                            serialPort?.Close();

                            serialPort = new SerialPort(UserSettings.ComPort, baudRate);
                            serialPort.Open();
                            openedComPort = UserSettings.ComPort;
                        }

                        //send frame data
                        byte[] outputStream = GetOutputStream();
                        serialPort.Write(outputStream, 0, outputStream.Length);

                        //ws2812b LEDs need 30 µs = 0.030 ms for each led to set its color so there is a lower minimum to the allowed refresh rate
                        //receiving over serial takes it time as well and the arduino does both tasks in sequence
                        //+1 ms extra safe zone
                        var fastLedTime = (outputStream.Length - _messagePreamble.Length)/3.0*0.030d;
                        var serialTransferTime = outputStream.Length*10.0*1000.0/baudRate;
                        var minTimespan = (int) (fastLedTime + serialTransferTime) + 1;

                        Task.Delay(minTimespan, cancellationToken).Wait(cancellationToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    _log.Debug("OperationCanceledException catched. returning.");

                    return;
                }
                catch (Exception ex)
                {
                    _log.Debug(ex, "Exception catched.");
                    //to be safe, we reset the serial port
                    if (serialPort != null && serialPort.IsOpen)
                    {
                        serialPort.Close();
                    }
                    serialPort?.Dispose();
                    serialPort = null;

                    //allow the system some time to recover
                    Thread.Sleep(500);
                }
                finally
                {
                    if (serialPort != null && serialPort.IsOpen)
                    {
                        serialPort.Close();
                        serialPort.Dispose();
                    }
                }
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
            }
        }
    }
}