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
    /// Interaction logic for LedSetup.xaml
    /// </summary>
    public partial class LedSetup : UserControl
    {
        public LedSetup()
        {
            InitializeComponent();
        }

        public class LedSetupSelectableViewPart : ISelectableViewPart
        {
            private readonly Lazy<LedSetup> lazyContent;

            public LedSetupSelectableViewPart(Lazy<LedSetup> lazyContent)
            {
                this.lazyContent = lazyContent ?? throw new ArgumentNullException(nameof(lazyContent));
            }
            public int Order => 10;

            public string ViewPartName => "Physical LED Setup";

            public object Content { get => lazyContent.Value; }
        }
    }
}
