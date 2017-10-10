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

namespace adrilight.View
{
    /// <summary>
    /// Interaction logic for LightingMode.xaml
    /// </summary>
    public partial class LightingModeSetup : UserControl
    {
        public LightingModeSetup()
        {
            InitializeComponent();
        }



        public class LightingModeSetupSelectableViewPart : ISelectableViewPart
        {
            private readonly Lazy<LightingModeSetup> lazyContent;

            public LightingModeSetupSelectableViewPart(Lazy<LightingModeSetup> lazyContent)
            {
                this.lazyContent = lazyContent ?? throw new ArgumentNullException(nameof(lazyContent));
            }

            public int Order => 60;

            public string ViewPartName => "Lighting Mode Selection";

            public object Content { get => lazyContent.Value; }
        }
    }
}
