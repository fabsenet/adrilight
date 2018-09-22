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
    /// Interaction logic for WhatsNew.xaml
    /// </summary>
    public partial class WhatsNew : UserControl
    {
        public WhatsNew()
        {
            InitializeComponent();
        }



        public class WhatsNewSetupSelectableViewPart : ISelectableViewPart
        {
            private readonly Lazy<WhatsNew> lazyContent;

            public WhatsNewSetupSelectableViewPart(Lazy<WhatsNew> lazyContent)
            {
                this.lazyContent = lazyContent ?? throw new ArgumentNullException(nameof(lazyContent));
            }

            public int Order => 50;

            public string ViewPartName => "Whats New?";

            public object Content => lazyContent.Value;
        }
    }
}
