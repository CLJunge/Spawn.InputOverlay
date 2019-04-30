#region Using
using NLog;
using SharpDX.XInput;
using Spawn.InputOverlay.Properties;
using System;
using System.Windows.Threading;
#endregion

namespace Spawn.InputOverlay.Input
{
    public class XInputHandler : IInputHandler
    {
        #region EventHandler
        public event EventHandler DeviceConnected;
        public event EventHandler DeviceDisconnected;
        public event EventHandler<InputUpdatedEventArgs> InputUpdated;
        #endregion

        #region Logger
        private static readonly Logger s_logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Member Variables
        private readonly Controller m_controller;
        private readonly DispatcherTimer m_timer;

        private bool m_blnPrevIsDeviceConntectedValue;
        private bool m_blnFiredInitialEvent;
        #endregion

        #region Properties
        public bool IsDeviceConnected => (m_controller?.IsConnected ?? false);
        #endregion

        #region Ctor
        public XInputHandler(UserIndex userIndex = UserIndex.One)
        {
            m_controller = new Controller(userIndex);

            m_timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(Settings.Default.RefreshRate),
            };
            m_timer.Tick += OnTimerTick;
            m_timer.Start();

            m_blnPrevIsDeviceConntectedValue = IsDeviceConnected;
        }
        #endregion

        #region OnTimerTick
        private void OnTimerTick(object sender, EventArgs e)
        {
            s_logger.Debug("Tick");

            CheckConnection();

            CheckInput();
        }
        #endregion

        #region CheckConnection
        private void CheckConnection()
        {
            s_logger.Debug("Checking device connection...");

            if (!m_blnFiredInitialEvent)
            {
                if (IsDeviceConnected)
                    DeviceConnected?.Invoke(this, EventArgs.Empty);

                m_blnFiredInitialEvent = true;
            }

            if (IsDeviceConnected != m_blnPrevIsDeviceConntectedValue)
            {
                if (IsDeviceConnected)
                    DeviceConnected?.Invoke(this, EventArgs.Empty);
                else
                    DeviceDisconnected?.Invoke(this, EventArgs.Empty);
            }

            m_blnPrevIsDeviceConntectedValue = IsDeviceConnected;
        }
        #endregion

        #region CheckInput
        private void CheckInput()
        {
            if (IsDeviceConnected && m_controller.GetState(out State state))
                InputUpdated?.Invoke(this, new InputUpdatedEventArgs(state.Gamepad));
        }
        #endregion

        #region RestartTime
        public void RestartTimer()
        {
            m_timer.Stop();
            m_timer.Interval = TimeSpan.FromMilliseconds(Settings.Default.RefreshRate);
            m_timer.Start();

            s_logger.Debug("Timer restarted");
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            s_logger.Debug("Disposing...");

            m_timer?.Stop();
        }
        #endregion
    }
}