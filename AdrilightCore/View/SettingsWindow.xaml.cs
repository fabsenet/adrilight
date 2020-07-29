using adrilight.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace adrilight.ui
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();


            var settingsViewModel = DataContext as SettingsViewModel;
            if (settingsViewModel != null)
            {
                settingsViewModel.IsSettingsWindowOpen = true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var settingsViewModel = DataContext as SettingsViewModel;
            if(settingsViewModel != null)
            {
                settingsViewModel.IsSettingsWindowOpen = false;
            }
        }
    }
}
