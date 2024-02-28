using AutoClicker.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Drawing.Point;
using WPFCursor = System.Windows.Forms.Cursor;
using MouseTrack = System.Collections.Generic.List<(System.Drawing.Point Position, double Timestamp)>;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Drawing;

namespace AutoClicker.Views
{
    public partial class CaptureDragDropWindow : Window
    {
        readonly MouseTrack keyPoints = new MouseTrack();
        readonly Stopwatch stopwatch = new Stopwatch();
        bool IsDragging = false;       
        public event EventHandler<MouseTrack> DragDropCaptured;
        #region Life Cycle

        public CaptureDragDropWindow()
        {
            DataContext = this;
            InitializeComponent();

            Log.Information("Opening window to capture drag and drop pattern.");

            Title = Constants.CAPTURE_MOUSE_COORDINATES_WINDOW_TITLE;
            Width = 0;
            Height = 0;
            Top = 0;
            Left = 0;
            WindowStyle = WindowStyle.None;
            WindowStartupLocation = WindowStartupLocation.Manual;
            ResizeMode = ResizeMode.NoResize;
            //Topmost = true;

            var screens = Screen.AllScreens;
            Log.Debug($"Total screens detected: {screens.Length}");

            // Need to do some special screen dimension calculation here to accomodate multiple monitors.
            // This works with horizontal, vertical and a combination of horizontal & vertical.
            // (e.g. 3 monitors total, 2 are side by side horizontally and the 3rd
            // is above/below the others) and vise versa.

            foreach (Screen screen in screens)
            {
                Log.Information(screen.ToString());

                // Find the lowest X & Y screen values, it's possible for screens to have negative
                // values depending on how the multi monitor setup is configured
                if (screen.Bounds.X < Left)
                {
                    Left = screen.Bounds.X;
                }

                if (screen.Bounds.Y < Top)
                {
                    Top = screen.Bounds.Y;
                }

                Width += screen.Bounds.Width;
                Height += screen.Bounds.Height;
            }

            Log.Information($"Set window size. Width: {Width}, Height: {Height}");
            Log.Information($"Set window position. Left: {Left}, Top: {Top}");
            Log.Information("Opened window to capture mouse coordinates.");
        }

        #endregion

        #region Event Handlers
        private double GetTimeStamp()
        {
            double tick = stopwatch.ElapsedTicks;
            tick /= Stopwatch.Frequency;
            tick *= 1000;
            return tick;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point position = WPFCursor.Position;
            
            if (IsDragging) keyPoints.Add((position, GetTimeStamp()));
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            IsDragging = true;
            Point position = WPFCursor.Position;
            keyPoints.Add((position, GetTimeStamp()));
            
            DragDropCaptured?.Invoke(this, keyPoints);                     
            Log.Information($"Captured drop position: {position.X}, {position.Y}");
            Close();            
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            IsDragging = true;

            Point position = WPFCursor.Position;
            keyPoints.Add((position, 0));
            stopwatch.Start();
            Log.Information($"Captured drag start position: {position.X}, {position.Y}");            
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            Log.Information($"Rendered window size: Width: {RenderSize.Width}, Height: {RenderSize.Height}");
            Log.Information($"Rendered window position: Left:{Left}, Height: {Height}");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Log.Information("Closing window to capture mouse coordinates.");
        }

        #endregion
    }
}
