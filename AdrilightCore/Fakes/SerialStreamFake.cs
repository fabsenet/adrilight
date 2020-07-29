using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adrilight.Fakes
{
    class SerialStreamFake : ISerialStream
    {
        public bool IsRunning { get; set; } = false;

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public bool IsValid() => true;

    }
}
