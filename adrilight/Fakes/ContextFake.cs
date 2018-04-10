using adrilight.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adrilight.Fakes
{
    class ContextFake : IContext
    {
        public bool IsSynchronized => true;

        public void BeginInvoke(Action action)
        {
        }

        public void Invoke(Action action)
        {
        }
    }
}
