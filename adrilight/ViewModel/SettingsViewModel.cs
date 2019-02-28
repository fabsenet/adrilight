using adrilight.DesktopDuplication;
using adrilight.Resources;
using adrilight.Settings;
using adrilight.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace adrilight.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private static ILogger _log = LogManager.GetCurrentClassLogger();

        private const string ProjectPage = "https://github.com/fabsenet/adrilight";
        private const string IssuesPage = "https://github.com/fabsenet/adrilight/issues";
        private const string LatestReleasePage = "https://github.com/fabsenet/adrilight/releases/latest";

        public SettingsViewModel(IUserSettings userSettings, IList<ISelectableViewPart> selectableViewParts
            , ISpotSet spotSet, IContext context, ISerialStream serialStream)
        {
            if (selectableViewParts == null)
            {
                throw new ArgumentNullException(nameof(selectableViewParts));
            }

            this.Settings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));
            this.spotSet = spotSet ?? throw new ArgumentNullException(nameof(spotSet));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            this.serialStream = serialStream ?? throw new ArgumentNullException(nameof(serialStream));
            SelectableViewParts = selectableViewParts.OrderBy(p => p.Order)
                .ToList();
#if DEBUG
            SelectedViewPart = SelectableViewParts.Last();
#else
            SelectedViewPart = SelectableViewParts.First();
#endif

            PossibleLedCountsVertical = Enumerable.Range(10, 190).ToList();
            PossibleLedCountsHorizontal = Enumerable.Range(10, 290).ToList();

            PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(SelectedViewPart):
                        var name = SelectedViewPart?.ViewPartName ?? "nothing";
                        break;
                }
            };

            Settings.PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(Settings.SpotsX):
                        RaisePropertyChanged(() => SpotsXMaximum);
                        RaisePropertyChanged(() => LedCount);
                        RaisePropertyChanged(() => OffsetLedMaximum);
                        break;

                    case nameof(Settings.SpotsY):
                        RaisePropertyChanged(() => SpotsYMaximum);
                        RaisePropertyChanged(() => LedCount);
                        RaisePropertyChanged(() => OffsetLedMaximum);
                        break;

                    case nameof(Settings.UseLinearLighting):
                        RaisePropertyChanged(() => UseNonLinearLighting);
                        break;

                    case nameof(Settings.OffsetLed):
                        RaisePropertyChanged(() => OffsetLedMaximum);
                        break;

                    case nameof(Settings.Autostart):
                        if (Settings.Autostart)
                        {
                            StartUpManager.AddApplicationToCurrentUserStartup();
                        }
                        else
                        {
                            StartUpManager.RemoveApplicationFromCurrentUserStartup();
                        }
                        break;

                    case nameof(Settings.ComPort):
                        RaisePropertyChanged(() => TransferCanBeStarted);
                        RaisePropertyChanged(() => TransferCanNotBeStarted);
                        break;
                }
            };
        }

        public string Title { get; } = $"adrilight {App.VersionNumber}";
        public int LedCount => SpotSet.CountLeds(Settings.SpotsX, Settings.SpotsY);

        public bool TransferCanBeStarted => serialStream.IsValid();
        public bool TransferCanNotBeStarted => !TransferCanBeStarted;

        public bool UseNonLinearLighting
        {
            get => !Settings.UseLinearLighting;
            set => Settings.UseLinearLighting = !value;
        }
        public IUserSettings Settings { get; }
        public IContext Context { get; }
        public IList<String> AvailableComPorts { get; } = SerialPort.GetPortNames().Concat(new[] { "Fake Port" }).ToList();

        public IList<ISelectableViewPart> SelectableViewParts { get; }

        public IList<int> PossibleLedCountsHorizontal { get; }
        public IList<int> PossibleLedCountsVertical { get; }

        public ISelectableViewPart _selectedViewPart;
        public ISelectableViewPart SelectedViewPart
        {
            get => _selectedViewPart;
            set
            {
                Set(ref _selectedViewPart, value);
                _log.Info($"SelectedViewPart is now {_selectedViewPart?.ViewPartName}");

                IsPreviewTabOpen = _selectedViewPart is View.SettingsWindowComponents.Preview.PreviewSelectableViewPart;
            }
        }

        private bool _isPreviewTabOpen;
        public bool IsPreviewTabOpen
        {
            get => _isPreviewTabOpen;
            private set
            {
                Set(ref _isPreviewTabOpen, value);
                _log.Info($"IsPreviewTabOpen is now {_isPreviewTabOpen}");
            }
        }

        private bool _isSettingsWindowOpen;
        public bool IsSettingsWindowOpen
        {
            get => _isSettingsWindowOpen;
            set
            {
                Set(ref _isSettingsWindowOpen, value);
                _log.Info($"IsSettingsWindowOpen is now {_isSettingsWindowOpen}");
            }
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public void SetPreviewImage(Bitmap image)
        {
            Context.Invoke(() =>
            {
                if (PreviewImageSource == null)
                {
                    //first run creates writableimage
                    var imagePtr = image.GetHbitmap();
                    try
                    {
                        var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(imagePtr, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                        PreviewImageSource = new WriteableBitmap(bitmapSource);
                    }
                    finally
                    {
                        var i = DeleteObject(imagePtr);
                    }
                }
                else
                {
                    //next runs reuse the writable image
                    Rectangle colorBitmapRectangle = new Rectangle(0, 0, image.Width, image.Height);
                    Int32Rect colorBitmapInt32Rect = new Int32Rect(0, 0, PreviewImageSource.PixelWidth, PreviewImageSource.PixelHeight);

                    BitmapData data = image.LockBits(colorBitmapRectangle, ImageLockMode.WriteOnly, image.PixelFormat);

                    PreviewImageSource.WritePixels(colorBitmapInt32Rect, data.Scan0, data.Width * data.Height * 4, data.Stride);

                    image.UnlockBits(data);
                }
            });
        }
        public WriteableBitmap _previewImageSource;
        public WriteableBitmap PreviewImageSource
        {
            get => _previewImageSource;
            set
            {
                _log.Info("PreviewImageSource created.");
                Set(ref _previewImageSource, value);

                RaisePropertyChanged(() => ScreenWidth);
                RaisePropertyChanged(() => ScreenHeight);
                RaisePropertyChanged(() => CanvasWidth);
                RaisePropertyChanged(() => CanvasHeight);
            }
        }


        public ICommand OpenUrlProjectPageCommand { get; } = new RelayCommand(() => OpenUrl(ProjectPage));
        public ICommand OpenUrlIssuesPageCommand { get; } = new RelayCommand(() => OpenUrl(IssuesPage));
        public ICommand OpenUrlLatestReleaseCommand { get; } = new RelayCommand(() => OpenUrl(LatestReleasePage));
        private static void OpenUrl(string url) => Process.Start(url);

        public ICommand ExitAdrilight { get; } = new RelayCommand(() => App.Current.Shutdown(0));

        private int _spotsXMaximum = 300;
        public int SpotsXMaximum
        {
            get
            {
                return _spotsXMaximum = Math.Max(Settings.SpotsX, _spotsXMaximum);
            }
        }

        private int _spotsYMaximum = 300;
        private readonly ISpotSet spotSet;
        private readonly ISerialStream serialStream;

        public int SpotsYMaximum
        {
            get
            {
                return _spotsYMaximum = Math.Max(Settings.SpotsY, _spotsYMaximum);
            }
        }

        public int OffsetLedMaximum => Math.Max(Settings.OffsetLed, LedCount);

        public int ScreenWidth => (PreviewImageSource?.PixelWidth ?? 1000);
        public int ScreenHeight => (PreviewImageSource?.PixelHeight ?? 1000);

        public int CanvasPadding => 300 / DesktopDuplicator.ScalingFactor;

        public int CanvasWidth => ScreenWidth + 2 * CanvasPadding;
        public int CanvasHeight => ScreenHeight + 2 * CanvasPadding;


        public ISpot[] _previewSpots;
        public ISpot[] PreviewSpots
        {
            get => _previewSpots;
            set {
                _previewSpots = value;
                RaisePropertyChanged();
            }
        }

        public Uri WhatsNewUrl {
            get
            {
                if (App.IsPrivateBuild)
                {
                    return new Uri($"https://fabse.net/adrilight/privateBuild/{Thread.CurrentThread.CurrentUICulture.Name}");
                }
                else
                {
                    return new Uri($"https://fabse.net/adrilight/{App.VersionNumber}/{Thread.CurrentThread.CurrentUICulture.Name}");
                }
            }
        }

        public bool _isInNightLightMode = false;
        public bool IsInNightLightMode {
            get => _isInNightLightMode;
            set
            {
                Set(ref _isInNightLightMode, value);
                RaisePropertyChanged(nameof(IsInDaylightLightMode));
            }
        }

        public bool IsInDaylightLightMode { get { return !_isInNightLightMode; } }

        public IDictionary<AlternateWhiteBalanceModeEnum, string> AlternateWhiteBalanceModes { get; } =
            new SortedDictionary<AlternateWhiteBalanceModeEnum, string>() {
                {AlternateWhiteBalanceModeEnum.On, "Forced On" },
                {AlternateWhiteBalanceModeEnum.Auto, "Auto detect" },
                {AlternateWhiteBalanceModeEnum.Off, "Forced Off" },
            };
    }
}
