using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoClicker
{
    public class SteadyIntervalTickEventArgs
    {
        public bool Continue { get; set; }
        public long TimeStamp { get; set; }
    }
    public class SteadyInterval
    {

        private readonly Stopwatch stopwatch = new Stopwatch();
        private long _interval;
        public event EventHandler<SteadyIntervalTickEventArgs> Tick;

        public long Interval
        {
            get => _interval;
            set
            {
                if (!IsIdle) throw new Exception();
                _interval = value;
            }
        }

        bool canceled;
        public bool IsIdle { get; private set; } = true;
        void RunInternal()
        {

            if (!IsIdle) throw new Exception();
            IsIdle = false;
            canceled = false;
            stopwatch.Restart();
            long ticks = 0;
            SteadyIntervalTickEventArgs e = new SteadyIntervalTickEventArgs();

            while (true)
            {
                long newTicks = stopwatch.ElapsedMilliseconds / Interval;
                long lag = newTicks - ticks;
                for (long i = 0; i < lag; i++)
                {
                    if (canceled) goto e;
                    e.Continue = true;
                    e.TimeStamp = stopwatch.ElapsedMilliseconds;
                    Tick(this, e);
                    if (e.Continue == false) goto e;
                }
                ticks = newTicks;
            }
        e:
            IsIdle = true;
        }
        public void Cancel()
        {
            canceled = true;
        }
        public Task Run()
        {
            return Task.Run(RunInternal);
        }
    }
}
