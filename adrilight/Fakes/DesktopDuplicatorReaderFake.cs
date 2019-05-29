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
        public RunStateEnum RunState { get; set; } = RunStateEnum.Stopped;

        public void Run(CancellationToken token)
        {
            Task.Factory.StartNew(() =>
            {
                RunState = RunStateEnum.Running;
                try
                {
                    token.WaitHandle.WaitOne();
                }
                finally
                {
                    RunState = RunStateEnum.Stopped;
                }
            });
        }
    }
}
