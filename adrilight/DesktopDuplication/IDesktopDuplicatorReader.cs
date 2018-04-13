using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace adrilight
{
    public interface IDesktopDuplicatorReader
    {
        bool IsRunning { get; }

        void Run(CancellationToken token);
    }
}