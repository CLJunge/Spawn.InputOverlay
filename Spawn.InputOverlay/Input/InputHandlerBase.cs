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
        protected virtual Logger Log => s_logger;
        private static readonly Logger s_logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Member Variables
        private readonly InputType m_inputType;

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
        public InputHandlerBase(InputType inputType)
        {
            m_inputType = inputType;

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
        public void StartConnectionTimer()
        {
            m_connectionTimer.Start();

            Log.Info("Waiting for device... ({0})", m_inputType);
        }
        #endregion

        #region StopConnectionTimer
        public void StopConnectionTimer()
        {
            m_connectionTimer.Stop();

            Log.Trace("Stopped connection timer ({0})", m_inputType);
        }
        #endregion

        #region RestartInputTimer
        public void RestartInputTimer()
        {
            m_inputTimer.Stop();
            m_inputTimer.Interval = TimeSpan.FromMilliseconds(Settings.Default.RefreshRate);
            m_inputTimer.Start();

            Log.Trace("Timer restarted ({0})", m_inputType);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Log.Trace("Disposing... ({0})", m_inputType);

            m_connectionTimer.Stop();
            m_inputTimer.Stop();

            Log.Debug("Stopped timers ({0})", m_inputType);
        }
        #endregion
    }
}