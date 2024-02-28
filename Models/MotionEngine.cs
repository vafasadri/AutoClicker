using AutoClicker.Enums;
using AutoClicker.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using MouseTrack = System.Collections.Generic.List<(System.Drawing.Point Position, double Timestamp)>;
namespace AutoClicker
{
    public abstract class MotionEngine
    {
        public const int TimerInterval = 5;
        public abstract int SafetyTimeLock { get; }
        int times = 0;
        protected abstract Point GetNextInternal(long timestamp);
        public Point? GetNext(long timestamp)
        {
            if (times > SafetyTimeLock) return null;
            times++;
            return GetNextInternal(timestamp);
        }
        private static Vector2 Vectorize(Point pathStart, Point pathEnd, double abs)
        {
            double dx = pathEnd.X - pathStart.X;
            double dy = pathEnd.Y - pathStart.Y;
            double d = Math.Sqrt(dx * dx + dy * dy);
            double sin = dy / d;
            double cos = dx / d;
            double x = abs * cos;
            double y = abs * sin;
            return new Vector2((float)x, (float)y);
        }
        public static MotionEngine Create(Point start, Point end, MotionMode motionMode, double time, double accel, double speed, MouseTrack points)
        {
            switch (motionMode)
            {
                case MotionMode.ConstantTime:
                    return new SpeedEngine(start, end, Vectorize(start, end, start.Distance(end) / time * TimerInterval));
                case MotionMode.ConstantSpeed:
                    return new SpeedEngine(start, end, Vectorize(start, end, speed * TimerInterval));
                case MotionMode.Acceleration:
                    return new AccelerationEngine(start, end, Vectorize(start, end, accel * TimerInterval * TimerInterval));
                case MotionMode.Custom:
                    return new CustomEngine(points);
                default:
                    throw new Exception();
            }
        }
        public static bool CanCreate(Point start, Point end, MotionMode motionMode, double time, double accel, double speed, MouseTrack records)
        {
            if (motionMode != MotionMode.Custom && start == end) return false;
            switch (motionMode)
            {
                case MotionMode.ConstantTime:
                    return time > 0 && time != double.PositiveInfinity;

                case MotionMode.ConstantSpeed:
                    return speed > 0 && speed != double.PositiveInfinity;

                case MotionMode.Acceleration:
                    return accel > 0 && accel != double.PositiveInfinity;

                case MotionMode.Custom:
                    return records != null && records[0].Position == start && records[records.Count - 1].Position == end && records[records.Count - 1].Timestamp > TimerInterval;

                default:
                    throw new NotSupportedException();
            }
        }
    }
    class SpeedEngine : MotionEngine
    {
        protected Vector2 Speed;
        protected Vector2 Position;

        public override int SafetyTimeLock { get; }

        protected override Point GetNextInternal(long timestamp)
        {
            Position.X += Speed.X;
            Position.Y += Speed.Y;
            return new Point((int)Position.X, (int)Position.Y);
        }
        public SpeedEngine(Point position, Point end, Vector2 speed)
        {
            this.Position = new Vector2(position.X, position.Y);
            this.Speed = speed;
            this.SafetyTimeLock = (int)Math.Ceiling(position.Distance(end) / speed.Length());
        }
    }
    class AccelerationEngine : SpeedEngine
    {
        protected Vector2 Acceleration;
        public override int SafetyTimeLock { get; }
        protected override Point GetNextInternal(long timestamp)
        {
            base.Speed.X += Acceleration.X;
            base.Speed.Y += Acceleration.Y;
            return base.GetNextInternal(timestamp);
        }
        public AccelerationEngine(Point position, Point end, Vector2 acceleration) : base(position, end, Vector2.Zero)
        {
            this.Acceleration = acceleration;
            //SafetyTimeLock = (int)Math.Ceiling( Math.Sqrt(2 * position.Distance(end) / acceleration.Length()));
            var d = position.Distance(end);
            double j = 0;
            double a = acceleration.Length();
            int i = 0;
            for (; j < d; j += a * (i++)) ;
            SafetyTimeLock = i - 1;
        }
    }
    class CustomEngine : MotionEngine
    {
        readonly MouseTrack points;
        int lastIndex = 0;
        public CustomEngine(MouseTrack points)
        {
            this.points = points;
            this.SafetyTimeLock = (int)(points[points.Count - 1].Timestamp / TimerInterval);
        }

        public override int SafetyTimeLock { get; }
        protected override Point GetNextInternal(long timestamp)
        {
            int i = lastIndex;
            for (; i < points.Count && points[i].Timestamp <= timestamp; i++) ;
            i--;
            lastIndex = i;
            return points[i].Position;
        }
    }
}
