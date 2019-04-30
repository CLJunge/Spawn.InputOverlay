#region Using
using System;
#endregion

namespace Spawn.InputOverlay.Input
{
    public interface IInputHandler : IDisposable
    {
        #region EventHandlers
        event EventHandler DeviceConnected;
        event EventHandler DeviceDisconnected;
        event EventHandler<InputEventArgs> InputUpdated;
        #endregion

        #region Properties
        bool IsDeviceConnected { get; }
        #endregion

        #region Methods
        void StartConnectionTimer();
        void RestartInputTimer();
        #endregion
    }
}