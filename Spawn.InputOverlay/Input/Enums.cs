using System;

namespace Spawn.InputOverlay.Input
{
    public enum InputType
    {
        None,
        DirectInput,
        XInput
    }

    public enum DirectInputDeviceType
    {
        None,
        DualShock4
    }

    [Flags]
    public enum Buttons
    {
        None,
    }
}