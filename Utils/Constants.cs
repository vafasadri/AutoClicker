﻿namespace AutoClicker.Utils
{
    public static class Constants
    {
        public const string MAIN_WINDOW_TITLE_DEFAULT = "AutoClicker";
        public const string MAIN_WINDOW_TITLE_RUNNING = " - Running...";
        public const string MAIN_WINDOW_START_BUTTON_CONTENT = "Start";
        public const string MAIN_WINDOW_TOGGLE_BUTTON_CONTENT = "Toggle";
        public const string MAIN_WINDOW_STOP_BUTTON_CONTENT = "Stop";

        public const string ABOUT_WINDOW_TITLE = "About";
        public const string SETTINGS_WINDOW_TITLE = "Hotkey Settings";
        public const string CAPTURE_MOUSE_COORDINATES_WINDOW_TITLE = "Capture Mouse Coordinates";

        public const string SYSTEM_TRAY_MENU_MINIMIZE = "Minimize to tray";
        public const string SYSTEM_TRAY_MENU_SHOW = "Show AutoClicker";
        public const string SYSTEM_TRAY_MENU_EXIT = "Exit";

        public const string RESOURCES_DIRECTORY = "Resources";
        public const string RUNNING_ICON_RESOURCE_PATH = RESOURCES_DIRECTORY + "\\Icons\\icon_running.ico";
        //public const string KEY_MAPPINGS_RESOURCE_PATH = "keyMappings.json";
       // public const string SETTINGS_FILE_PATH = "AutoClicker_Settings.json";
        public const string LOG_FILE_PATH = "AutoClicker_Logs.txt";

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;

        public const int MOD_NONE = 0x0;
        public const int START_HOTKEY_ID = 9000;
        public const int STOP_HOTKEY_ID = 9001;
        public const int TOGGLE_HOTKEY_ID = 9002;
        public const int WM_HOTKEY = 0x0312;      
    }
}
