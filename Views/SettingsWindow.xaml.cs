using AutoClicker.Properties;
using AutoClicker.Utils;
using Serilog;
using System.Windows;
using System.Windows.Input;

namespace AutoClicker.Views
{
    public partial class SettingsWindow : Window
    {
        #region Dependency Properties

        public HotkeySettings HotkeySettings
        {
            get => (HotkeySettings)GetValue(HotkeySettingsProperty);
            set => SetValue(HotkeySettingsProperty, value);
        }

        public static readonly DependencyProperty HotkeySettingsProperty =
            DependencyProperty.Register(nameof(HotkeySettings), typeof(HotkeySettings), typeof(SettingsWindow));

        #endregion Dependency Properties

        #region Life Cycle

        public SettingsWindow()
        {

            InitializeComponent();
            DataContext = this;
            Title = Constants.SETTINGS_WINDOW_TITLE;
            HotkeySettings = Properties.HotkeySettings.Default;
        }

        #endregion Life Cycle

        #region Commands

        private void SaveCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            HotkeySettings.Save();
        }

        private void ResetCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            HotkeySettings.Reset();
        }

        #endregion Commands

        #region Helper Methods

        private void StartKeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeySettings.StartHotkey = GenericKeyDownHandler(e);
        }

        private void StopKeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeySettings.StopHotkey = GenericKeyDownHandler(e);
        }

        private void ToggleKeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeySettings.ToggleHotkey = GenericKeyDownHandler(e);
        }

        private int GenericKeyDownHandler(KeyEventArgs e)
        {
            e.Handled = true;
            return GetNewint(e.Key);
        }

        private int GetNewint(Key key)
        {
            int virtualKeyCode = KeyInterop.VirtualKeyFromKey(key);
            Log.Debug("GetNewint with virtualKeyCode {VirtualKeyCode}", virtualKeyCode);
            return virtualKeyCode;
        }

        #endregion Helper Methods

        private void Window_Closed(object sender, System.EventArgs e)
        {
            HotkeySettings.Reload();
        }
    }
}
