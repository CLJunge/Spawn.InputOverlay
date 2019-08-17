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

        public byte LeftTrigger { get; }

        public byte RightTrigger { get; }

        public GamepadButtonFlags PressedButtons { get; }
        #endregion

        #region Ctor
        public InputEventArgs(double leftStickX,
            byte leftTrigger, byte rightTrigger,
            GamepadButtonFlags pressedButtons)
        {
            LeftStickX = leftStickX;
            LeftTrigger = leftTrigger;
            RightTrigger = rightTrigger;
            PressedButtons = pressedButtons;
        }
        #endregion
    }
}