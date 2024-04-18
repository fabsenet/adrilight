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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace adrilight.View.SettingsWindowComponents
{
    /// <summary>
    /// Interaction logic for ColorModeSetup.xaml
    /// </summary>
    public partial class ColorModeSetup : UserControl
    {
        public ColorModeSetup()
        {
            InitializeComponent();
        }



        public class ColorModeSelectableViewPart : ISelectableViewPart
        {
            private readonly Lazy<ColorModeSetup> lazyContent;

            public ColorModeSelectableViewPart(Lazy<ColorModeSetup> lazyContent)
            {
                this.lazyContent = lazyContent ?? throw new ArgumentNullException(nameof(lazyContent));
            }

            public int Order => 800;

            public string ViewPartName => "Color Mode";

            public object Content { get => lazyContent.Value; }
        }
    }
}
