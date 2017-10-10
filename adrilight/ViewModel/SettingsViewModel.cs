using adrilight.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace adrilight.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        private static ILogger _log = LogManager.GetCurrentClassLogger();

        private const string ProjectPage = "https://github.com/fabsenet/adrilight";
        private const string IssuesPage = "https://github.com/fabsenet/adrilight/issues";
        private const string LatestReleasePage = "https://github.com/fabsenet/adrilight/releases/latest";

        public SettingsViewModel(IUserSettings userSettings, IList<ISelectableViewPart> selectableViewParts)
        {
            if (selectableViewParts == null)
            {
                throw new ArgumentNullException(nameof(selectableViewParts));
            }

            this.Settings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));
            SelectableViewParts = selectableViewParts.OrderBy(p => p.Order)
                .ToList();
            SelectedViewPart = SelectableViewParts.First();

            PossibleLedCountsVertical = Enumerable.Range(10, 190).ToList();
            PossibleLedCountsHorizontal = Enumerable.Range(10, 290).ToList();

            Settings.PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(Settings.SpotsX): RaisePropertyChanged(() => SpotsXMaximum);
                        break;
                }
            };
        }

        public string Title { get; } = $"adrilight {App.VersionNumber}";

        private bool _isLeftMenuOpen;
        public bool IsLeftMenuOpen
        {
            get => _isLeftMenuOpen;
            set => Set(ref _isLeftMenuOpen, value);
        }

        public IUserSettings Settings { get; }
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
                IsLeftMenuOpen = false;
            }
        }

        public ICommand OpenUrlProjectPageCommand { get; } = new RelayCommand(() => OpenUrl(ProjectPage));
        public ICommand OpenUrlIssuesPageCommand { get; } = new RelayCommand(() => OpenUrl(IssuesPage));
        public ICommand OpenUrlLatestReleaseCommand { get; } = new RelayCommand(() => OpenUrl(LatestReleasePage));
        private static void OpenUrl(string url) => Process.Start(url);

        public ICommand ExitAdrilight { get; } = new RelayCommand(() => _log.Warn("exiting not implemented"), () => false);

        private int _spotsXMaximum = 300;
        public int SpotsXMaximum
        {
            get
            {
                return _spotsXMaximum = Math.Max(Settings.SpotsX, _spotsXMaximum);
            }
        }

        private int _spotsYMaximum = 300;
        public int SpotsYMaximum
        {
            get
            {
                return _spotsYMaximum = Math.Max(Settings.SpotsY, _spotsYMaximum);
            }
        }
    }
}
