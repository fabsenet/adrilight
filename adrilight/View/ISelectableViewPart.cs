using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adrilight.View
{
    interface ISelectableViewPart
    {
        int Order { get; }
        string ViewPartName { get; }
        object Content { get; }
    }
}
