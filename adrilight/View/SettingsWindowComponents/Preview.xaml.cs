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
    public partial class Preview : UserControl
    {
        public Preview()
        {
            InitializeComponent();
        }



        public class PreviewSelectableViewPart : ISelectableViewPart
        {
            private readonly Lazy<Preview> lazyContent;

            public PreviewSelectableViewPart(Lazy<Preview> lazyContent)
            {
                this.lazyContent = lazyContent ?? throw new ArgumentNullException(nameof(lazyContent));
            }

            public int Order => 700;

            public string ViewPartName => "Preview Results";

            public object Content { get => lazyContent.Value; }
        }
    }
}
