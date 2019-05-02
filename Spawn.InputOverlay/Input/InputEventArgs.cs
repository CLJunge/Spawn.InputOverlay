#region Ctor
using SharpDX.XInput;
using System;
#endregion

namespace Spawn.InputOverlay.Input
{
    public class InputEventArgs : EventArgs
    {
        #region Properties
        public double LeftStickX { get; }

        public bool IsLeftTriggerPressed { get; }
        public bool IsRightTriggerPressed { get; }

        public GamepadButtonFlags PressedButtons { get; }
        #endregion

        #region Ctor
        public InputEventArgs(double leftStickX,
            bool isLeftTriggerPressed, bool isRightTriggerPressed,
            GamepadButtonFlags pressedButtons)
        {
            LeftStickX = leftStickX;
            IsLeftTriggerPressed = isLeftTriggerPressed;
            IsRightTriggerPressed = isRightTriggerPressed;
            PressedButtons = pressedButtons;
        }
        #endregion
    }
}