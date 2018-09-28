using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adrilight.ViewModel
{
    public interface ISelectableViewPart
    {
        int Order { get; }
        string ViewPartName { get; }
        object Content { get; }
    }
}
