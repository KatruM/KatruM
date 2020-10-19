using System;
using System.Threading;
using Xamarin.Forms;

namespace BDI3Mobile.Helpers
{
    public class TimerStopWatch
    {
        private readonly TimeSpan _timeSpan;
        private readonly Action _callback;

        private CancellationTokenSource _cancellationTokenSource;

        public TimerStopWatch(TimeSpan timeSpan, Action callback)
        {
            _timeSpan = timeSpan;
            _callback = callback;
            _cancellationTokenSource = new CancellationTokenSource();
        }
        public void Start()
        {
            CancellationTokenSource cts = _cancellationTokenSource;
            Device.StartTimer(_timeSpan, () =>
            {
                if (cts.IsCancellationRequested)
                {
                    return false;
                }
                _callback.Invoke();
                //true to continuous, false to single use
                return true; 
            });
        }

        public void Stop()
        {
            Interlocked.Exchange(ref _cancellationTokenSource, new CancellationTokenSource()).Cancel();
        }
    }
}
