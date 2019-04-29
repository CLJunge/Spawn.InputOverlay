using System;

namespace Spawn.InputOverlay.Input
{
    public interface IInputHandler : IDisposable
    {
        event EventHandler DeviceConnected;
        event EventHandler DeviceDisconnected;
        event EventHandler<InputUpdatedEventArgs> InputUpdated;

        bool IsDeviceConnected { get; }

        void RestartTimer();
    }
}