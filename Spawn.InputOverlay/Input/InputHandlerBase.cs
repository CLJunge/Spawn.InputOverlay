#region Using
using NLog;
using Spawn.InputOverlay.Properties;
using System;
using System.Windows.Threading;
#endregion

namespace Spawn.InputOverlay.Input
{
    public abstract class InputHandlerBase : IInputHandler
    {
        #region Logger
        protected static readonly Logger s_logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Member Variables
        protected readonly DispatcherTimer m_connectionTimer;
        protected readonly DispatcherTimer m_inputTimer;
        #endregion

        #region Properties
        public abstract bool IsDeviceConnected { get; }
        #endregion

        #region EventHandlers
        public event EventHandler DeviceConnected;
        public event EventHandler DeviceDisconnected;
        public event EventHandler<InputEventArgs> InputUpdated;

        protected void RaiseDeviceConnectedEvent(object sender, EventArgs e) => DeviceConnected?.Invoke(sender, e);
        protected void RaiseDeviceDisconnectedEvent(object sender, EventArgs e) => DeviceDisconnected?.Invoke(sender, e);
        protected void RaiseInputUpdatedEvent(object sender, InputEventArgs e) => InputUpdated?.Invoke(sender, e);
        #endregion

        #region Ctor
        public InputHandlerBase()
        {
            m_connectionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(250),
            };

            m_inputTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(Settings.Default.RefreshRate),
            };
        }
        #endregion

        #region StartConnectionTimer
        public void StartConnectionTimer() => m_connectionTimer.Start();
        #endregion

        #region RestartInputTime
        public void RestartInputTimer()
        {
            m_inputTimer.Stop();
            m_inputTimer.Interval = TimeSpan.FromMilliseconds(Settings.Default.RefreshRate);
            m_inputTimer.Start();

            s_logger.Trace("Timer restarted");
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            s_logger.Trace("Disposing...");

            m_connectionTimer.Stop();
            m_inputTimer.Stop();
        }
        #endregion
    }
}