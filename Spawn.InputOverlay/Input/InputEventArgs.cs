#region Ctor
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

        public bool IsAccelerateButtonPressed { get; }

        public bool IsBrakeButtonPressed { get; }
        #endregion

        #region Ctor
        public InputEventArgs(double leftStickX,
            bool isLeftTriggerPressed, bool isRightTriggerPressed,
            bool isAccelerateButtonPressed, bool isBrakeButtonPressed)
        {
            LeftStickX = leftStickX;
            IsLeftTriggerPressed = isLeftTriggerPressed;
            IsRightTriggerPressed = isRightTriggerPressed;
            IsAccelerateButtonPressed = isAccelerateButtonPressed;
            IsBrakeButtonPressed = isBrakeButtonPressed;
        }
        #endregion
    }
}