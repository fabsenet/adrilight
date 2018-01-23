using adrilight.Resources;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace adrilight.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        private static ILogger _log = LogManager.GetCurrentClassLogger();

        private const string ProjectPage = "https://github.com/fabsenet/adrilight";
        private const string IssuesPage = "https://github.com/fabsenet/adrilight/issues";
        private const string LatestReleasePage = "https://github.com/fabsenet/adrilight/releases/latest";

        public SettingsViewModel(IUserSettings userSettings, IList<ISelectableViewPart> selectableViewParts, ISpotSet spotSet, IContext context)
        {
            if (selectableViewParts == null)
            {
                throw new ArgumentNullException(nameof(selectableViewParts));
            }

            this.Settings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));
            this.spotSet = spotSet ?? throw new ArgumentNullException(nameof(spotSet));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            SelectableViewParts = selectableViewParts.OrderBy(p => p.Order)
                .ToList();
            SelectedViewPart = SelectableViewParts.First();

            PossibleLedCountsVertical = Enumerable.Range(10, 190).ToList();
            PossibleLedCountsHorizontal = Enumerable.Range(10, 290).ToList();

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

                    case nameof(Settings.LedsPerSpot):
                        RaisePropertyChanged(() => LedCount);
                        RaisePropertyChanged(() => OffsetLedMaximum);
                        break;

                    case nameof(Settings.UseLinearLighting):
                        RaisePropertyChanged(() => UseNonLinearLighting);
                        break;

                    case nameof(Settings.OffsetLed):
                        RaisePropertyChanged(() => OffsetLedMaximum);
                        break;
                }
            };
        }

        public string Title { get; } = $"adrilight {App.VersionNumber}";
        public int LedCount => spotSet.CountLeds(Settings.SpotsX, Settings.SpotsY) * Settings.LedsPerSpot;

        public bool UseNonLinearLighting
        {
            get => !Settings.UseLinearLighting;
            set => Settings.UseLinearLighting = !value;
        }
        public IUserSettings Settings { get; }
        public IContext Context { get; }
        public IList<String> AvailableComPorts { get; } = SerialPort.GetPortNames();

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

        [DllImport("gdi32.dll")] public static extern bool DeleteObject(IntPtr hObject);
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

        public int SpotsYMaximum
        {
            get
            {
                return _spotsYMaximum = Math.Max(Settings.SpotsY, _spotsYMaximum);
            }
        }

        public int OffsetLedMaximum => Math.Max(Settings.OffsetLed, LedCount);

        public int ScreenWidth => (int)System.Windows.SystemParameters.PrimaryScreenWidth;
        public int ScreenHeight => (int)System.Windows.SystemParameters.PrimaryScreenHeight;

        private const int CanvasPadding = 300;

        public int CanvasWidth => ScreenWidth + 2 * CanvasPadding;
        public int CanvasHeight => ScreenHeight + 2 * CanvasPadding;

        public List<Spot> PreviewSpots
        {
            get
            {
                var list = new List<Spot>();
                list.Add(new Spot(CanvasPadding + 0, CanvasPadding + 0, 200, 200));
                list.Add(new Spot(CanvasPadding + 500, CanvasPadding + 100, 200, 200));

                list[0].SetColor(0, 155, 255);
                list[1].SetColor(255, 155, 255);

                return list;
            }
        }

    }
}
