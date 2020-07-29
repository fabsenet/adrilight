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
    /// Interaction logic for LightingMode.xaml
    /// </summary>
    public partial class GeneralSetup : UserControl
    {
        public GeneralSetup()
        {
            InitializeComponent();
        }



        public class GeneralSetupSelectableViewPart : ISelectableViewPart
        {
            private readonly Lazy<GeneralSetup> lazyContent;

            public GeneralSetupSelectableViewPart(Lazy<GeneralSetup> lazyContent)
            {
                this.lazyContent = lazyContent ?? throw new ArgumentNullException(nameof(lazyContent));
            }

            public int Order => 900;

            public string ViewPartName => "General Setup";

            public object Content { get => lazyContent.Value; }
        }
    }
}
