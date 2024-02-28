using AutoClicker.Enums;
using AutoClicker.Models;
using AutoClicker.Properties;
using AutoClicker.Utils;
using Serilog;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CheckBox = System.Windows.Controls.CheckBox;
using MouseAction = AutoClicker.Enums.MouseAction;
using MouseButton = AutoClicker.Enums.MouseButton;
using MouseCursor = System.Windows.Forms.Cursor;
using NotifyIcon = System.Windows.Forms.NotifyIcon;
using Point = System.Drawing.Point;
using Timer = System.Windows.Threading.DispatcherTimer;
using MouseTrack = System.Collections.Generic.List<(System.Drawing.Point Position, double Timestamp)>;
namespace AutoClicker.Views
{
    public partial class MainWindow : Window
    {
        public AutoClickerSettings AutoClickerSettings => AutoClickerSettings.Default;
        public KeyDisplayConverter DisplayConverter => (KeyDisplayConverter)Resources["DisplayConverter"];
        public DragDropState DragDropState = new DragDropState();

        public bool IsIdle
        {
            get { return (bool)GetValue(IsIdleProperty); }
            set { SetValue(IsIdleProperty, value); }
        }
        public static DependencyProperty IsIdleProperty = DependencyProperty.Register(nameof(IsIdle), typeof(bool), typeof(MainWindow),
               new UIPropertyMetadata(true));
        private int timesRepeated = 0;
        private readonly Timer clickTimer;
        private readonly Uri runningIconUri =
            new Uri(Constants.RUNNING_ICON_RESOURCE_PATH, UriKind.Relative);

        private NotifyIcon systemTrayIcon;
        private SystemTrayMenu systemTrayMenu;
        private AboutWindow aboutWindow = null;
        private SettingsWindow settingsWindow = null;
        private CaptureMouseScreenCoordinatesWindow captureMouseCoordinatesWindow;
        private CaptureDragDropWindow captureDragDropWindow;
        private ImageSource _defaultIcon;
        private IntPtr _mainWindowHandle;
        private HwndSource _source;
        #region Life Cycle

        public MainWindow()
        {
            clickTimer = new Timer();
            clickTimer.Tick += OnClickTimerElapsed;

            DataContext = this;
            ResetTitle();
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _mainWindowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_mainWindowHandle);
            _source.AddHook(StartStopHooks);
            HotkeySettings hotkeySettings = HotkeySettings.Default;
            hotkeySettings.SettingChanging += SettingsUtils_HotKeyChangedEvent;
            hotkeySettings.StartHotkey = hotkeySettings.StartHotkey;
            hotkeySettings.StopHotkey = hotkeySettings.StopHotkey;
            hotkeySettings.ToggleHotkey = hotkeySettings.ToggleHotkey;
            _defaultIcon = Icon;
            DragDropState.SteadyInterval.Interval = MotionEngine.TimerInterval;
            DragDropState.SteadyInterval.Tick += DragNDrop_Tick;
            InitializeSystemTrayMenu();
        }

        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(StartStopHooks);

            HotkeySettings.Default.SettingChanging -= SettingsUtils_HotKeyChangedEvent;
            UnregisterHotkey(Constants.START_HOTKEY_ID);
            UnregisterHotkey(Constants.STOP_HOTKEY_ID);
            UnregisterHotkey(Constants.TOGGLE_HOTKEY_ID);

            systemTrayIcon.Click -= SystemTrayIcon_Click;
            systemTrayIcon.Dispose();

            systemTrayMenu.SystemTrayMenuActionEvent -= SystemTrayMenu_SystemTrayMenuActionEvent;
            systemTrayMenu.Dispose();

            Log.Information("Application closing");
            Log.Debug("==================================================");               
            base.OnClosed(e);
        }

        #endregion Life Cycle

        #region Commands

        private async void StartCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (!IsIdle) return;
            int interval = CalculateInterval();
            Log.Information("Starting operation, interval={Interval}ms", interval);

            timesRepeated = 0;
            DragDropState.Enabled = AutoClickerSettings.ActionMode == ActionMode.DragDrop;
            Icon = new BitmapImage(runningIconUri);
            Title += Constants.MAIN_WINDOW_TITLE_RUNNING;
            systemTrayIcon.Text += Constants.MAIN_WINDOW_TITLE_RUNNING;
            IsIdle = false;
            if (DragDropState.Enabled)
            {
                await DragNDrop_Start();
            }
            else
            {
                var set = AutoClickerSettings;
                clickTimer.Interval = new TimeSpan(0, set.Hours, set.Minutes, set.Seconds, set.Milliseconds);
                clickTimer.Start();
            }
        }

        private void StartCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanStartOperation();
        }

        private void StopCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            Log.Information("Stopping operation");
            clickTimer.Stop();
            DragDropState.SteadyInterval.Cancel();
            ResetTitle();
            Icon = _defaultIcon;
            IsIdle = true;
        }

        private void StopCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsIdle;
        }

        private void ToggleCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsIdle)
                StartCommand_Execute(sender, e);
            else
                StopCommand_Execute(sender, e);
        }

        private void ToggleCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsIdle || CanStartOperation();
        }

        private void SaveSettingsCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            Log.Information("Saving Settings");
            AutoClickerSettings.Save();
            //SettingsUtils.SetApplicationSettings(AutoClickerSettings);
        }

        private void HotkeySettingsCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (settingsWindow == null)
            {
                settingsWindow = new SettingsWindow();
                settingsWindow.Closed += (o, args) => settingsWindow = null;
            }

            settingsWindow.Show();
        }

        private void ExitCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            Exit();
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private void AboutCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (aboutWindow == null)
            {
                aboutWindow = new AboutWindow();
                aboutWindow.Closed += (o, args) => aboutWindow = null;
            }

            aboutWindow.Show();
        }

        private void CaptureMouseScreenCoordinatesCommand_Execute(
            object sender,
            EventArgs e
        )
        {
            if (captureMouseCoordinatesWindow == null)
            {
                captureMouseCoordinatesWindow = new CaptureMouseScreenCoordinatesWindow();
                captureMouseCoordinatesWindow.Closed += (o, args) => captureMouseCoordinatesWindow = null;
                captureMouseCoordinatesWindow.OnCoordinatesCaptured += (_, point) =>
                {
                    switch ((sender as Button).Tag as string)
                    {
                        case "Click":
                            AutoClickerSettings.PickedClickPosition = point;
                            AutoClickerSettings.ClickLocationMode = LocationMode.PickedLocation;
                            break;
                        case "Drag":
                            AutoClickerSettings.PickedDragPosition = point;
                            AutoClickerSettings.DragLocationMode = LocationMode.PickedLocation;
                            break;
                        case "Drop":
                            AutoClickerSettings.PickedDropPosition = point;
                            AutoClickerSettings.DropLocationMode = LocationMode.PickedLocation;
                            break;
                    }
                };
            }

            captureMouseCoordinatesWindow.Show();
        }
        #endregion Commands

        #region Helper Methods

        private int CalculateInterval()
        {
            return AutoClickerSettings.Milliseconds
                + (AutoClickerSettings.Seconds * 1000)
                + (AutoClickerSettings.Minutes * 60 * 1000)
                + (AutoClickerSettings.Hours * 60 * 60 * 1000);
        }

        private bool IsIntervalValid()
        {
            return CalculateInterval() > 0;
        }

        private bool CanStartOperation()
        {
            if (!IsIdle) return false;
            if (AutoClickerSettings.ActionMode == ActionMode.Click)
            {
                return !clickTimer.IsEnabled && IsRepeatModeValid() && IsIntervalValid();
            }
            else
            {
                return MotionEngine.CanCreate(
                GetSelectedDragPosition(),
                GetSelectedDropPosition(),
                AutoClickerSettings.DragDropMotionMode,
                AutoClickerSettings.DragDropTime,
                AutoClickerSettings.DragDropAcceleration,
                AutoClickerSettings.DragDropSpeed,
                DragDropState.Record);
            }
        }

        private int GetTimesToRepeat()
        {
            return AutoClickerSettings.SelectedRepeatMode == RepeatMode.Count ? AutoClickerSettings.SelectedTimesToRepeat : -1;
        }

        private Point GetSelectedClickPosition()
        {
            return AutoClickerSettings.ClickLocationMode == LocationMode.CurrentLocation ?
                MouseCursor.Position : AutoClickerSettings.PickedClickPosition;
        }
        private Point GetSelectedDragPosition()
        {

            return AutoClickerSettings.DragDropMotionMode == MotionMode.Custom ? DragDropState.Record?.FirstOrDefault().Position ?? Point.Empty : AutoClickerSettings.DragLocationMode == LocationMode.CurrentLocation ?
               MouseCursor.Position : AutoClickerSettings.PickedDragPosition;
        }
        private Point GetSelectedDropPosition()
        {
            return AutoClickerSettings.DragDropMotionMode == MotionMode.Custom ? DragDropState.Record?.LastOrDefault().Position ?? Point.Empty : AutoClickerSettings.DropLocationMode == LocationMode.CurrentLocation ?
                   MouseCursor.Position : AutoClickerSettings.PickedDropPosition;
        }

        private int GetNumberOfMouseActions()
        {
            return AutoClickerSettings.SelectedMouseAction == MouseAction.Single ? 1 : 2;
        }

        private bool IsRepeatModeValid()
        {
            return AutoClickerSettings.SelectedRepeatMode == RepeatMode.Infinite
                || (AutoClickerSettings.SelectedRepeatMode == RepeatMode.Count && AutoClickerSettings.SelectedTimesToRepeat > 0);
        }

        private void ResetTitle()
        {
            Title = Constants.MAIN_WINDOW_TITLE_DEFAULT;
            if (systemTrayIcon != null)
            {
                systemTrayIcon.Text = Constants.MAIN_WINDOW_TITLE_DEFAULT;
            }
        }

        private void InitializeSystemTrayMenu()
        {
            systemTrayIcon = new NotifyIcon
            {
                Visible = true,
                Icon = AssemblyUtils.GetApplicationIcon()
            };

            systemTrayIcon.Click += SystemTrayIcon_Click;
            systemTrayIcon.Text = Constants.MAIN_WINDOW_TITLE_DEFAULT;
            systemTrayMenu = new SystemTrayMenu();
            systemTrayMenu.SystemTrayMenuActionEvent += SystemTrayMenu_SystemTrayMenuActionEvent;
        }

        private void ReRegisterHotkey(int hotkeyId, int virtualKey)
        {
            UnregisterHotkey(hotkeyId);
            RegisterHotkey(hotkeyId, virtualKey);
        }

        private void RegisterHotkey(int hotkeyId, int virtualKey)
        {
            //Log.Information("RegisterHotkey with hotkeyId {HotkeyId} and hotkey {Hotkey}", hotkeyId, hotkey.DisplayName);
            User32ApiUtils.RegisterHotKey(_mainWindowHandle, hotkeyId, Constants.MOD_NONE, virtualKey);
        }

        private void UnregisterHotkey(int hotkeyId)
        {
            Log.Information("UnregisterHotkey with hotkeyId {HotkeyId}", hotkeyId);
            if (User32ApiUtils.UnregisterHotKey(_mainWindowHandle, hotkeyId))
                return;
            Log.Warning("No hotkey registered on {HotkeyId}", hotkeyId);
        }

        #endregion Helper Methods

        #region Event Handlers

        private void OnClickTimerElapsed(object sender, EventArgs e)
        {
            var btn = AutoClickerSettings.SelectedMouseButton;
            var pos = GetSelectedClickPosition();
            PerformMouseClick(GetMouseDownCode(btn), GetMouseUpCode(btn), pos.X, pos.Y);
            timesRepeated++;
            if (timesRepeated == GetTimesToRepeat())
            {
                clickTimer.Stop();
                ResetTitle();
            }
        }
        int GetMouseDownCode(MouseButton button)
        {
            switch (button)
            {
                case
                MouseButton.Left:
                    return Constants.MOUSEEVENTF_LEFTDOWN;
                case MouseButton.Right:
                    return Constants.MOUSEEVENTF_RIGHTDOWN;
                case MouseButton.Middle:
                    return Constants.MOUSEEVENTF_MIDDLEDOWN;
                default:
                    throw new InvalidOperationException();
            };
        }
        int GetMouseUpCode(MouseButton button)
        {
            switch (button)
            {
                case
                MouseButton.Left:
                    return Constants.MOUSEEVENTF_LEFTUP;
                case MouseButton.Right:
                    return Constants.MOUSEEVENTF_RIGHTUP;
                case MouseButton.Middle:
                    return Constants.MOUSEEVENTF_MIDDLEUP;
                default:
                    throw new InvalidOperationException();
            };
        }
        private async Task DragNDrop_Start()
        {
            var drag = GetSelectedDragPosition();
            var drop = DragDropState.DropPosition = GetSelectedDropPosition();
            var btn = AutoClickerSettings.SelectedMouseButton; //GetMouseButtonCode();
            PerformMouseClick(GetMouseDownCode(btn), 0, drag.X, drag.Y);
            DragDropState.MotionEngine = MotionEngine.Create(drag,
                drop,
                AutoClickerSettings.DragDropMotionMode,
                AutoClickerSettings.DragDropTime,
                AutoClickerSettings.DragDropAcceleration,
                AutoClickerSettings.DragDropSpeed,
                DragDropState.Record);

            await DragDropState.SteadyInterval.Run();
            PerformMouseClick(0, GetMouseUpCode(btn), DragDropState.DropPosition.X, DragDropState.DropPosition.Y);
            StopCommand_Execute(this, null);           
        }
        private void DragNDrop_Tick(object sender, SteadyIntervalTickEventArgs e)
        {
            var p = DragDropState.MotionEngine.GetNext(e.TimeStamp);

            if (p == null)
            {
                e.Continue = false;
                return;
            }
            var point = p.Value;
            PerformMouseClick(GetMouseDownCode(AutoClickerSettings.SelectedMouseButton), 0, point.X, point.Y);
            User32ApiUtils.SetCursorPosition(point.X, point.Y);
            // if we have passed the target point           
            e.Continue = true;
        }

        private void PerformMouseClick(int mouseDownAction, int mouseUpAction, int xPos, int yPos)
        {
            for (int i = 0; i < GetNumberOfMouseActions(); ++i)
            {
                var setCursorPos = User32ApiUtils.SetCursorPosition(xPos, yPos);
                if (!setCursorPos)
                {
                    Log.Error($"Could not set the mouse cursor.");
                }

                User32ApiUtils.ExecuteMouseEvent(mouseDownAction | mouseUpAction, xPos, yPos, 0, 0);
            }
        }

        private IntPtr StartStopHooks(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            int hotkeyId = wParam.ToInt32();
            if (msg == Constants.WM_HOTKEY && hotkeyId == Constants.START_HOTKEY_ID || hotkeyId == Constants.STOP_HOTKEY_ID || hotkeyId == Constants.TOGGLE_HOTKEY_ID)
            {
                int virtualKey = ((int)lParam >> 16) & 0xFFFF;
                HotkeySettings hotKeys = HotkeySettings.Default;
                if (virtualKey == hotKeys.StartHotkey && CanStartOperation())
                {
                    StartCommand_Execute(null, null);
                }
                if (virtualKey == hotKeys.StopHotkey && clickTimer.IsEnabled)
                {
                    StopCommand_Execute(null, null);
                }
                if (virtualKey == hotKeys.ToggleHotkey && CanStartOperation() | clickTimer.IsEnabled)
                {
                    ToggleCommand_Execute(null, null);
                }
                handled = true;
            }
            return IntPtr.Zero;
        }
        private void SettingsUtils_HotKeyChangedEvent(object sender, SettingChangingEventArgs e)
        {

            var keyDisplay = (string)DisplayConverter.Convert(e.NewValue, null, null, null);

            Log.Information("HotKeyChangedEvent with operation {Operation} and hotkey {Hotkey}", e.SettingName, keyDisplay);

            switch (e.SettingName)
            {
                case nameof(HotkeySettings.StartHotkey):
                    ReRegisterHotkey(Constants.START_HOTKEY_ID, (int)e.NewValue);
                    startButton.Content = $"{Constants.MAIN_WINDOW_START_BUTTON_CONTENT} ({keyDisplay})";
                    break;
                case nameof(HotkeySettings.StopHotkey):
                    ReRegisterHotkey(Constants.STOP_HOTKEY_ID, (int)e.NewValue);
                    stopButton.Content = $"{Constants.MAIN_WINDOW_STOP_BUTTON_CONTENT} ({keyDisplay})";
                    break;
                case nameof(HotkeySettings.ToggleHotkey):
                    ReRegisterHotkey(Constants.TOGGLE_HOTKEY_ID, (int)e.NewValue);
                    toggleButton.Content = $"{Constants.MAIN_WINDOW_TOGGLE_BUTTON_CONTENT} ({keyDisplay})";
                    break;
                default:
                    Log.Warning("Operation {Operation} not supported!", e.SettingName);
                    throw new NotSupportedException($"Operation {e.SettingName} not supported!");

            }
        }

        private void SystemTrayIcon_Click(object sender, EventArgs e)
        {
            systemTrayMenu.IsOpen = true;
            systemTrayMenu.Focus();
        }

        private void SystemTrayMenu_SystemTrayMenuActionEvent(object sender, SystemTrayMenuActionEventArgs e)
        {
            switch (e.Action)
            {
                case SystemTrayMenuAction.Show:
                    Show();
                    break;
                case SystemTrayMenuAction.Hide:
                    Hide();
                    break;
                case SystemTrayMenuAction.Exit:
                    Exit();
                    break;
                default:
                    Log.Warning("Action {Action} not supported!", e.Action);
                    throw new NotSupportedException($"Action {e.Action} not supported!");
            }
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (aboutWindow == null)
            {
                aboutWindow = new AboutWindow();
                aboutWindow.Closed += (o, args) => aboutWindow = null;
            }

            aboutWindow.Show();
        }

        private void MinimizeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            systemTrayMenu.ToggleMenuItemsVisibility(true);
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Exit();
        }

        #endregion Event Handlers

        private void TopMostCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            Topmost = checkbox.IsChecked.Value;
        }

        private void CaptureDragDropCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = captureDragDropWindow == null;
        }

        private void CaptureDragDropCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (captureDragDropWindow != null) return;
            captureDragDropWindow = new CaptureDragDropWindow();
            captureDragDropWindow.Closed += (o, args) => captureDragDropWindow = null;
            captureDragDropWindow.DragDropCaptured += CaptureDragDropWindow_DragDropCaptured;
            captureDragDropWindow.Show();
        }

        private void CaptureDragDropWindow_DragDropCaptured(object sender, MouseTrack e)
        {
            DragDropState.Record = e;
            AutoClickerSettings.DragDropMotionMode = MotionMode.Custom;
        }
    }
}