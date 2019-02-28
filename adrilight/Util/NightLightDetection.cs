using adrilight.ViewModel;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                    await Task.Delay(TimeSpan.FromSeconds(10), _cancellationTokenSource.Token);
                    ActOnce();
                }
            }
            catch (TaskCanceledException)
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
                        _settingsViewModel.IsInNightLightMode = IsWindowsInNightLightMode();
                        break;
                }
            }
        }

        private Dictionary<byte[], bool> _knownBlueLightStates = new Dictionary<byte[], bool>()
        {
            //off, timer active
            { new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 203, 243, 164, 226, 5, 42, 43, 14, 16, 67, 66, 1, 0, 198, 20, 171, 186, 244, 147, 163, 243, 172, 234, 1, 0, 0, 0, 0 }
            , false },
            //off, timer active
            {new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 248, 253, 187, 226, 5, 42, 43, 14, 19, 67, 66, 1, 0, 208, 10, 2, 198, 20, 177, 184, 144, 204, 170, 225, 173, 234, 1, 0, 0, 0, 0 }, false },
            //off, timer active
            {new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 182, 206, 188, 226, 5, 42, 43, 14, 19, 67, 66, 1, 0, 208, 10, 2, 198, 20, 222, 220, 163, 176, 170, 228, 173, 234, 1, 0, 0, 0, 0 }, false }

            //forced on
            { new byte[]{ 67,66,1,0,10,2,1,0,42,6,189,193,167,226,5,42,43,14,21,67,66,1,0,16,0,208,10,2,198,20,181,206,156,165,219,255,172, 234,1,0,0,0,0 }
            , true },
            //forced on
            { new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 156, 205, 188, 226, 5, 42, 43, 14, 21, 67, 66, 1, 0, 16, 0, 208, 10, 2, 198, 20, 255, 140, 207, 208, 164, 228, 173, 234, 1, 0, 0, 0, 0 }
            , true },       };

        private readonly HashSet<string> alreadyReportedStates = new HashSet<string>();
        private bool IsWindowsInNightLightMode()
        {
            try
            {
                var regkey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.bluelightreductionstate\windows.data.bluelightreduction.bluelightreductionstate");
                var regval = (byte[])regkey.GetValue("Data");
                if(_log.IsDebugEnabled) _log.Debug("read regval: "+regval.Select(b => b.ToString()).Aggregate((s1,s2) => s1+","+s2));

                foreach(var kvp in _knownBlueLightStates)
                {
                    if (kvp.Key.SequenceEqual(regval))
                    {
                        return kvp.Value;
                    }
                }

                var stateString = regval.Select(b => b.ToString()).Aggregate((s1, s2) => s1 + "," + s2);

                //this state is unknown
                if (alreadyReportedStates.Add(stateString))
                {
                    _log.Warn($"Cannot handle bluelight state: {stateString}");
                    //ask user??
                }
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "IsWindowsInNightLightMode() failed");

                //this feature should never break adrilight, so the default is just false!
                return false;
            }
        }
    }
}