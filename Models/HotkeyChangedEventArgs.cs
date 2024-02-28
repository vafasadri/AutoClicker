using AutoClicker.Enums;
using System;

namespace AutoClicker.Models
{
    public class HotkeyChangedEventArgs : EventArgs
    {
        public int Hotkey { get; set; }
        public Operation Operation { get; set; }
    }
}
