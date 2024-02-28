using AutoClicker.Enums;
using System;

namespace AutoClicker.Models
{
    public class SystemTrayMenuActionEventArgs : EventArgs
    {
        public SystemTrayMenuAction Action { get; set; }
    }
}
