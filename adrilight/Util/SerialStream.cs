using adrilight.Settings;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using System.Buffers;
using System.Windows.Media;
using adrilight.Util;
using System.Linq;
using Newtonsoft.Json;

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
            RefreshTransferState();
            _log.Info($"SerialStream created.");

            if (!IsValid())
            {
                UserSettings.TransferActive = false;
                UserSettings.ComPort = null;
            }
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

        public bool IsValid() => SerialPort.GetPortNames().Contains(UserSettings.ComPort) || UserSettings.ComPort == "Fake Port";

        private void RefreshTransferState()
        {
            if (UserSettings.TransferActive && !IsRunning)
            {
                if (IsValid())
                {
                    //start it
                    _log.Debug("starting the serial stream");
                    Start();
                }
                else
                {
                    UserSettings.TransferActive = false;
                    UserSettings.ComPort = null;
                }
            }
            else if (!UserSettings.TransferActive && IsRunning)
            {
                //stop it
                _log.Debug("stopping the serial stream");
                Stop();
            }
        }

        private readonly byte[] _messagePreamble = {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09};
        private readonly byte[] _messagePostamble = {85, 204, 165};


        private Thread _workerThread;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private int frameCounter;
        private int blackFrameCounter;

        public void Start()
        {
            _log.Debug("Start called.");
            if (_workerThread != null) return;

            _cancellationTokenSource = new CancellationTokenSource();
            _workerThread = new Thread(DoWork) {
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

        private (byte[] Buffer, int OutputLength) GetOutputStream()
        {
            byte[] outputStream;

            int counter = _messagePreamble.Length;
            lock (SpotSet.Lock)
            {
                const int colorsPerLed = 3;
                int bufferLength = _messagePreamble.Length
                                   + (SpotSet.Spots.Length * colorsPerLed)
                                   + _messagePostamble.Length;


                outputStream = ArrayPool<byte>.Shared.Rent(bufferLength);

                Buffer.BlockCopy(_messagePreamble, 0, outputStream, 0, _messagePreamble.Length);
                Buffer.BlockCopy(_messagePostamble, 0, outputStream, bufferLength - _messagePostamble.Length,
                    _messagePostamble.Length);

                var allBlack = true;
                foreach (Spot spot in SpotSet.Spots)
                {
                    if (UserSettings.ColorMode.Equals(ColorModeEnum.Static))
                    {
                        outputStream[counter++] = UserSettings.StaticColorModeBlue; // blue
                        outputStream[counter++] = UserSettings.StaticColorModeGreen; // green
                        outputStream[counter++] = UserSettings.StaticColorModeRed; // red

                        allBlack = allBlack && UserSettings.StaticColorModeRed == 0 &&
                                   UserSettings.StaticColorModeGreen == 0 && UserSettings.StaticColorModeBlue == 0;
                    }
                    else if (!UserSettings.SendRandomColors)
                    {
                        outputStream[counter++] = spot.Blue; // blue
                        outputStream[counter++] = spot.Green; // green
                        outputStream[counter++] = spot.Red; // red
                    }
                    else
                    {
                        allBlack = false;
                        var n = frameCounter % 360;
                        var c = ColorUtil.FromAhsb(255, n, 1, 0.5f);
                        outputStream[counter++] = c.B; // blue
                        outputStream[counter++] = c.G; // green
                        outputStream[counter++] = c.R; // red
                    }
                }

                if (allBlack)
                {
                    blackFrameCounter++;
                }

                return (outputStream, bufferLength);
            }
        }

        private void DoWork(object tokenObject)
        {
            var cancellationToken = (CancellationToken) tokenObject;
            ISerialPortWrapper serialPort = null;

            if (String.IsNullOrEmpty(UserSettings.ComPort))
            {
                _log.Warn("Cannot start the serial sending because the comport is not selected.");
                return;
            }

            frameCounter = 0;
            blackFrameCounter = 0;

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

                            serialPort = UserSettings.ComPort != "Fake Port"
                                ? (ISerialPortWrapper) new WrappedSerialPort(new SerialPort(UserSettings.ComPort,
                                    baudRate))
                                : new FakeSerialPort();

                            try
                            {
                                serialPort.Open();
                            }
                            catch
                            {
                                // useless UnauthorizedAccessException 
                            }

                            if (!serialPort.IsOpen)
                            {
                                serialPort = null;

                                //allow the system some time to recover
                                Thread.Sleep(500);
                                continue;
                            }

                            openedComPort = UserSettings.ComPort;
                        }

                        //send frame data
                        var (outputBuffer, streamLength) = GetOutputStream();
                        serialPort.Write(outputBuffer, 0, streamLength);

                        if (++frameCounter == 1024 && blackFrameCounter > 1000)
                        {
                            //there is maybe something wrong here because most frames where black. report it once per run only
                            var settingsJson = JsonConvert.SerializeObject(UserSettings, Formatting.None);
                            _log.Info(
                                $"Sent {frameCounter} frames already. {blackFrameCounter} were completely black. Settings= {settingsJson}");
                        }

                        ArrayPool<byte>.Shared.Return(outputBuffer);

                        //ws2812b LEDs need 30 µs = 0.030 ms for each led to set its color so there is a lower minimum to the allowed refresh rate
                        //receiving over serial takes it time as well and the arduino does both tasks in sequence
                        //+1 ms extra safe zone
                        var fastLedTime = (streamLength - _messagePreamble.Length - _messagePostamble.Length) / 3.0 *
                                          0.030d;
                        var serialTransferTime = streamLength * 10.0 * 1000.0 / baudRate;
                        var minTimespan = (int) (fastLedTime + serialTransferTime) + 1;

                        Thread.Sleep(minTimespan);
                    }
                }
                catch (OperationCanceledException)
                {
                    _log.Debug("OperationCanceledException catched. returning.");

                    return;
                }
                catch (Exception ex)
                {
                    if (ex.GetType() != typeof(AccessViolationException) &&
                        ex.GetType() != typeof(UnauthorizedAccessException))
                    {
                        _log.Debug(ex, "Exception catched.");
                    }

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

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
            }
        }
    }
}