using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adrilight.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(IUserSettings userSettings)
        {
            this.Settings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));
        }

        public string Title { get; } = $"adrilight {App.VersionNumber}";

        public IUserSettings Settings { get; }
    }
}
