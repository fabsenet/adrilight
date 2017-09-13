using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adrilight.ui
{
    class SettingsViewModel : MvxViewModel
    {
        public SettingsViewModel(IUserSettings userSettings)
        {
            this.Settings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));
        }

        public IUserSettings Settings { get; }
    }
}
