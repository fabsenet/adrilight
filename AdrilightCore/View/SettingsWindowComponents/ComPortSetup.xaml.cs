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
    /// Interaction logic for ComPortSetup.xaml
    /// </summary>
    public partial class ComPortSetup : UserControl
    {
        public ComPortSetup()
        {
            InitializeComponent();
        }



        public class ComPortSetupSelectableViewPart : ISelectableViewPart
        {
            private readonly Lazy<ComPortSetup> lazyContent;

            public ComPortSetupSelectableViewPart(Lazy<ComPortSetup> lazyContent)
            {
                this.lazyContent = lazyContent ?? throw new ArgumentNullException(nameof(lazyContent));
            }

            public int Order => 100;

            public string ViewPartName => "Serial Communication Setup";

            public object Content { get => lazyContent.Value; }
        }
    }
}
