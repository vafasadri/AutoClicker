﻿namespace AutoClicker.Enums
{
    public enum MouseButton
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    public enum MouseAction
    {
        Single = 0,
        Double = 1
    }

    public enum RepeatMode
    {
        Infinite = 0,
        Count = 1
    }

    public enum LocationMode
    {
        CurrentLocation = 0,
        PickedLocation = 1
    }

    public enum Operation
    {
        Start = 0,
        Stop = 1,
        Toggle = 2
    }

    public enum SystemTrayMenuAction
    {
        Show = 0,
        Hide = 1,
        Exit = 2
    }
    public enum ActionMode
    {
        Click = 0,
        DragDrop = 1
    }
    public enum MotionMode
    {
        ConstantTime = 0,
        ConstantSpeed = 1,
        Acceleration = 2,
        Custom = 3
    }
}
