using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace adrilight.Fakes
{
    class DesktopDuplicatorReaderFake : IDesktopDuplicatorReader
    {
        public bool IsRunning { get; set; }= false;

        public void Run(CancellationToken token)
        {
            Task.Factory.StartNew(() =>
            {
                IsRunning = true;
                try
                {
                    token.WaitHandle.WaitOne();
                }
                finally
                {
                    IsRunning = false;
                }
            });
        }
    }
}
