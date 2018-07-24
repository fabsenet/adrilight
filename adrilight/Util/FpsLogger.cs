using MoreLinq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace adrilight.Util
{
    sealed class FpsLogger : IDisposable
    {
        private ILogger _log = LogManager.GetCurrentClassLogger();

        private readonly string _name;
        private readonly Subject<Unit> _frames = new Subject<Unit>();

#if DEBUG
        //infinite timespan sequence: forever 1s
        private readonly IEnumerable<TimeSpan> _logTimes = MoreEnumerable.Generate(TimeSpan.FromSeconds(1), _ => TimeSpan.FromSeconds(1));
#else
        //infinite timespan sequence: 10x 1s, then forever 5min
        private readonly IEnumerable<TimeSpan> _logTimes =
            Enumerable.Repeat(TimeSpan.FromSeconds(1), 10)
            .Concat(MoreEnumerable.Generate(TimeSpan.FromMinutes(5), _ => TimeSpan.FromMinutes(5)));
#endif

        private readonly IEnumerator<TimeSpan> _enumerator;
        private readonly IDisposable _loggingSubscription;
        private readonly IDisposable _valueUpdatingSubscription;


        public FpsLogger(string name)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _enumerator = _logTimes.GetEnumerator();

            var loggingTrigger = Observable.Generate(1, _ => true, i => i + 1, i => i, _ =>
            {
                _enumerator.MoveNext();
                return _enumerator.Current;
            });

            var fpsObserverable = _frames
                .Buffer(TimeSpan.FromSeconds(1))
                .Select(nums => nums.Count);

            _valueUpdatingSubscription = fpsObserverable.Subscribe(f => Fps = f);

            _loggingSubscription = loggingTrigger
                .WithLatestFrom(fpsObserverable, (_, fps) => fps)
                .Subscribe(WriteFpsLog);
        }

        public int Fps { get; private set; }

        private void WriteFpsLog(int fps)
        {
            _log.Debug($"there were {fps} frames for {_name} in the last second.");
        }

        public void TrackSingleFrame() => _frames.OnNext(Unit.Default);

        public void Dispose()
        {
            _valueUpdatingSubscription?.Dispose();
            _loggingSubscription?.Dispose();
            _frames?.Dispose();
            _enumerator?.Dispose();
        }
    }
}
