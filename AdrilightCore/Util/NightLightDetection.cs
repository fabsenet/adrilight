using adrilight.ViewModel;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace adrilight.Util
{
    class NightLightDetection
    {
        private readonly ILogger _log = LogManager.GetCurrentClassLogger();

        private readonly SettingsViewModel _settingsViewModel;

        public NightLightDetection(SettingsViewModel settingsViewModel)
        {
            _settingsViewModel = settingsViewModel ?? throw new ArgumentNullException(nameof(settingsViewModel));

            _settingsViewModel.Settings.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(_settingsViewModel.Settings.AlternateWhiteBalanceMode))
                {
                    ActOnce();
                }
            };
            ActOnce();
        }

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Task _actingTask;

        public void Start()
        {
            _actingTask = Task.Run(ActInLoop);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();

            if (_actingTask == null || _actingTask.IsCompleted) return;

            _actingTask.Wait();
        }

        private async Task ActInLoop()
        {
            try
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2), _cancellationTokenSource.Token);
                    ActOnce();
                }
            }
            catch (OperationCanceledException)
            {
                //expected!
                return;
            }
        }

        private readonly object _actOnceLock = new object();
        private void ActOnce()
        {
            lock (_actOnceLock)
            {
                switch (_settingsViewModel.Settings.AlternateWhiteBalanceMode)
                {
                    case Settings.AlternateWhiteBalanceModeEnum.On:
                        _settingsViewModel.IsInNightLightMode = true;
                        break;
                    case Settings.AlternateWhiteBalanceModeEnum.Off:
                        _settingsViewModel.IsInNightLightMode = false;
                        break;

                    case Settings.AlternateWhiteBalanceModeEnum.Auto:
                        try
                        {
                            _settingsViewModel.IsInNightLightMode = IsWindowsInNightLightMode();
                        }
                        catch (Exception ex)
                        {
                            _log.Fatal(ex, $"Failed to guess the night light mode with an exception, switching from auto to off.");
                            _settingsViewModel.Settings.AlternateWhiteBalanceMode = Settings.AlternateWhiteBalanceModeEnum.Off;
                        }
                        break;
                }
            }
        }


        private RegistryKey _stateKeyWin10 = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount\$$windows.data.bluelightreduction.bluelightreductionstate\Current", false);
        private RegistryKey _stateKeyInsider = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.bluelightreductionstate\windows.data.bluelightreduction.bluelightreductionstate", false);

        private byte[] _lastData;
        private bool _lastResult;
        Microsoft.ML.MLContext _context;
        Microsoft.ML.Data.TransformerChain<Microsoft.ML.ITransformer> _model;
        private PredictionEngine<NightLightDataRow, NightLightState> _predictor;

        private bool IsWindowsInNightLightMode()
        {
            try
            {
                var data = (byte[])(_stateKeyInsider ?? _stateKeyWin10).GetValue("Data");

                if(data == null)
                {
                    const string msg = "could not read or find any data for the night light mode in the registry.";
                    _log.Error(msg);
                    throw new Exception(msg);
                }

                if(_lastData != null && data.SequenceEqual(_lastData))
                {
                    return _lastResult;
                }
                if (_log.IsDebugEnabled) _log.Debug("read regval: " + data.Select(b => b.ToString()).Aggregate((s1, s2) => s1 + "," + s2));

                var stateString = data.Select(b => b.ToString()).Aggregate((s1, s2) => s1 + "," + s2);

                if (_predictor == null)
                {
                    _context = new Microsoft.ML.MLContext();
                    using (var zipStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("adrilight.Resources.NightLightDetectionModel.zip"))
                    {
                        _model = _context.Model.Load(zipStream, out var inputSchema) as Microsoft.ML.Data.TransformerChain<Microsoft.ML.ITransformer>;
                        _predictor = _context.Model.CreatePredictionEngine<NightLightDataRow, NightLightState>(_model);
                    }
                }

                var predicted = _predictor.Predict(new NightLightDataRow(data, true));
                var isUnclearResult = predicted.Probability <= 0.9f && predicted.Probability >= 0.1f;
                if (isUnclearResult)
                {
                    _log.Warn($"predicted: {predicted.PredictedLabel}, {predicted.Probability:0.00000}");
                }
                else
                {
                    _log.Debug($"predicted: {predicted.PredictedLabel}, {predicted.Probability:0.00000}");
                }

                _lastData = data;
                _lastResult = predicted.PredictedLabel;

                return predicted.PredictedLabel;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "IsWindowsInNightLightMode() failed");
                throw;
            }
        }
    }


    class NightLightDataRow
    {
        public bool IsActive { get; }

        //  [Microsoft.ML.Data.VectorType(43)]
        public byte[] Data { get; }

        [Microsoft.ML.Data.VectorType(35)]
        public float[] Data2 {
            get;
        }

        public NightLightDataRow()
        {

        }

        public NightLightDataRow(byte[] data, bool isActive)
        {
            Data = data;
            Data2 = data.Concat(Enumerable.Repeat((byte)0, Math.Max(0, 35 - data.Length))).Select(d => (float)d).Take(35).ToArray();
            IsActive = isActive;
        }
    }

    class NightLightState
    {
        public float Probability { get; set; }
        public bool PredictedLabel { get; set; }

    }
}