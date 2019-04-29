#region Ctor
using SharpDX.XInput;
using System;
#endregion

namespace Spawn.InputOverlay.Input
{
    public class InputUpdatedEventArgs : EventArgs
    {
        #region Properties
        public Gamepad DeviceState { get; private set; }

        public double LeftStickX => Math.Round(DeviceState.LeftThumbX / 32767f, 3);
        #endregion

        #region Ctor
        public InputUpdatedEventArgs(Gamepad deviceState) => DeviceState = deviceState;
        #endregion
    }
}