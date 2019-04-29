#region Ctor
using SharpDX.XInput;
using System;
#endregion

namespace Spawn.InputOverlay.Input
{
    public class XboxOneInputEventArgs : EventArgs
    {
        #region Properties
        public Gamepad DeviceState { get; private set; }

        public double LeftStickX => Math.Round(DeviceState.LeftThumbX / 32767f, 3);

        public double LeftStickY => Math.Round(DeviceState.LeftThumbY / 32767f, 3);
        #endregion

        #region Ctor
        public XboxOneInputEventArgs(Gamepad deviceState) => DeviceState = deviceState;
        #endregion
    }
}