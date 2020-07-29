using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adrilight.Fakes
{
    class SmarthomeFake : ISmarthome
    {
        public Task DoWorkAsync() => Task.CompletedTask;
    }
}
