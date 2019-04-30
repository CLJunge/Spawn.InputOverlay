#region Ctor
using System;
#endregion

namespace Spawn.InputOverlay.Input
{
    public class InputEventArgs : EventArgs
    {
        #region Properties
        public double LeftStickX { get; }

        public bool IsDPadLeftPressed { get; }

        public bool IsDPadRightPressed { get; }

        public bool IsLeftTriggerPressed { get; }

        public bool IsRightTriggerPressed { get; }

        public bool IsAccelerateButtonPressed { get; }

        public bool IsBrakeButtonPressed { get; }
        #endregion

        #region Ctor
        public InputEventArgs(double leftStickX,
            bool isDPadLeftPressed, bool isDPadRightPressed,
            bool isLeftTriggerPressed, bool isRightTriggerPressed,
            bool isAccelerateButtonPressed, bool isBrakeButtonPressed)
        {
            LeftStickX = leftStickX;
            IsDPadLeftPressed = isDPadLeftPressed;
            IsDPadRightPressed = isDPadRightPressed;
            IsLeftTriggerPressed = isLeftTriggerPressed;
            IsRightTriggerPressed = isRightTriggerPressed;
            IsAccelerateButtonPressed = isAccelerateButtonPressed;
            IsBrakeButtonPressed = isBrakeButtonPressed;
        }
        #endregion
    }
}