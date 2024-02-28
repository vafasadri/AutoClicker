using System.Collections.Generic;
using System.Drawing;

namespace AutoClicker.Models
{
    public class DragDropState
    {
        public bool Enabled { get; set; }

        public Point DropPosition { get; set; }

        public SteadyInterval SteadyInterval { get; } = new SteadyInterval();

        public MotionEngine MotionEngine { get; set; }

        public List<(Point Position, double Timestamp)> Record { get; set; }
    }
}
