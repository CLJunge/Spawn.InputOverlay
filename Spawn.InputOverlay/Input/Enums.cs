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
        DualShock4,
        ThirdParty
    }

    [Flags]
    public enum GamepadButtons
    {
        None = 0,
        LeftShoulder = 256,
        RightShoulder = 512,
        A = 4096,
        B = 8192,
        X = 16384,
        Y = 32768,
        LeftTrigger = 65536,
        RightTrigger = 131072
    }
}