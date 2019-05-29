using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace adrilight
{
    public interface IDesktopDuplicatorReader
    {
        RunStateEnum RunState { get; }

        void Run(CancellationToken token);
    }
    public enum RunStateEnum
    {
        Stopped,
        Running,
        Stopping
    };
}