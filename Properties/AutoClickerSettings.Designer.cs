﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoClicker.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.7.0.0")]
    public sealed partial class AutoClickerSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static AutoClickerSettings defaultInstance = ((AutoClickerSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new AutoClickerSettings())));
        
        public static AutoClickerSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Click")]
        public global::AutoClicker.Enums.ActionMode ActionMode {
            get {
                return ((global::AutoClicker.Enums.ActionMode)(this["ActionMode"]));
            }
            set {
                this["ActionMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CurrentLocation")]
        public global::AutoClicker.Enums.LocationMode ClickLocationMode {
            get {
                return ((global::AutoClicker.Enums.LocationMode)(this["ClickLocationMode"]));
            }
            set {
                this["ClickLocationMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CurrentLocation")]
        public global::AutoClicker.Enums.LocationMode DragLocationMode {
            get {
                return ((global::AutoClicker.Enums.LocationMode)(this["DragLocationMode"]));
            }
            set {
                this["DragLocationMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CurrentLocation")]
        public global::AutoClicker.Enums.LocationMode DropLocationMode {
            get {
                return ((global::AutoClicker.Enums.LocationMode)(this["DropLocationMode"]));
            }
            set {
                this["DropLocationMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point PickedClickPosition {
            get {
                return ((global::System.Drawing.Point)(this["PickedClickPosition"]));
            }
            set {
                this["PickedClickPosition"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point PickedDragPosition {
            get {
                return ((global::System.Drawing.Point)(this["PickedDragPosition"]));
            }
            set {
                this["PickedDragPosition"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point PickedDropPosition {
            get {
                return ((global::System.Drawing.Point)(this["PickedDropPosition"]));
            }
            set {
                this["PickedDropPosition"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int Hours {
            get {
                return ((int)(this["Hours"]));
            }
            set {
                this["Hours"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int Minutes {
            get {
                return ((int)(this["Minutes"]));
            }
            set {
                this["Minutes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int Seconds {
            get {
                return ((int)(this["Seconds"]));
            }
            set {
                this["Seconds"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int Milliseconds {
            get {
                return ((int)(this["Milliseconds"]));
            }
            set {
                this["Milliseconds"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Left")]
        public global::AutoClicker.Enums.MouseButton SelectedMouseButton {
            get {
                return ((global::AutoClicker.Enums.MouseButton)(this["SelectedMouseButton"]));
            }
            set {
                this["SelectedMouseButton"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Single")]
        public global::AutoClicker.Enums.MouseAction SelectedMouseAction {
            get {
                return ((global::AutoClicker.Enums.MouseAction)(this["SelectedMouseAction"]));
            }
            set {
                this["SelectedMouseAction"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Infinite")]
        public global::AutoClicker.Enums.RepeatMode SelectedRepeatMode {
            get {
                return ((global::AutoClicker.Enums.RepeatMode)(this["SelectedRepeatMode"]));
            }
            set {
                this["SelectedRepeatMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int SelectedTimesToRepeat {
            get {
                return ((int)(this["SelectedTimesToRepeat"]));
            }
            set {
                this["SelectedTimesToRepeat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double DragDropSpeed {
            get {
                return ((double)(this["DragDropSpeed"]));
            }
            set {
                this["DragDropSpeed"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ConstantTime")]
        public global::AutoClicker.Enums.MotionMode DragDropMotionMode {
            get {
                return ((global::AutoClicker.Enums.MotionMode)(this["DragDropMotionMode"]));
            }
            set {
                this["DragDropMotionMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double DragDropTime {
            get {
                return ((double)(this["DragDropTime"]));
            }
            set {
                this["DragDropTime"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double DragDropAcceleration {
            get {
                return ((double)(this["DragDropAcceleration"]));
            }
            set {
                this["DragDropAcceleration"] = value;
            }
        }
    }
}
