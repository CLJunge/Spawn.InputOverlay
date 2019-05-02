#region Using
using NLog;
using System;
#endregion

namespace Spawn.InputOverlay.Input
{
    public class DeviceManager : IDisposable
    {
        #region Logger
        private static readonly Logger s_logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Singleton
        private static DeviceManager s_instance;

        public static DeviceManager Instance => s_instance ?? (s_instance = new DeviceManager());
        #endregion

        #region EventHandlers
        public event EventHandler DeviceConnected;
        public event EventHandler DeviceDisconnected;

        protected void RaiseDeviceConnectedEvent(object sender, EventArgs e) => DeviceConnected?.Invoke(sender, e);
        protected void RaiseDeviceDisconnectedEvent(object sender, EventArgs e) => DeviceDisconnected?.Invoke(sender, e);
        #endregion

        #region Member Variables
        private readonly IInputHandler m_directInputHandler;
        private readonly IInputHandler m_xInputHandler;
        private InputType m_inputType;
        #endregion

        #region Ctor
        private DeviceManager()
        {
            m_directInputHandler = new DirectInputHandler();
            m_directInputHandler.DeviceConnected += OnDeviceConnected;
            m_directInputHandler.DeviceDisconnected += OnDeviceDisconnected;

            m_xInputHandler = new XInputHandler();
            m_xInputHandler.DeviceConnected += OnDeviceConnected;
            m_xInputHandler.DeviceDisconnected += OnDeviceDisconnected;

            m_inputType = InputType.None;
        }
        #endregion

        #region OnDeviceConnected
        private void OnDeviceConnected(object sender, EventArgs e)
        {
            m_directInputHandler.StopConnectionTimer();

            if (sender.Equals(m_directInputHandler))
            {
                m_inputType = InputType.DirectInput;

                m_xInputHandler.StopConnectionTimer();
            }
            else if (sender.Equals(m_xInputHandler))
            {
                m_inputType = InputType.XInput;
            }

            RaiseDeviceConnectedEvent(sender, e);
        }
        #endregion

        #region OnDeviceDisconnected
        private void OnDeviceDisconnected(object sender, EventArgs e)
        {
            RaiseDeviceDisconnectedEvent(sender, e);

            m_directInputHandler.StartConnectionTimer();
            m_xInputHandler.StartConnectionTimer();
            m_inputType = InputType.None;
        }
        #endregion

        #region StartConnectionTimers
        public void StartConnectionTimers()
        {
            m_directInputHandler.StartConnectionTimer();
            m_xInputHandler.StartConnectionTimer();
        }
        #endregion

        #region RestartInputTimer
        public void RestartInputTimer()
        {
            switch (m_inputType)
            {
                case InputType.DirectInput:
                    m_directInputHandler.RestartInputTimer();
                    break;

                case InputType.XInput:
                    m_xInputHandler.RestartInputTimer();
                    break;
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            m_directInputHandler.Dispose();
            m_xInputHandler.Dispose();
        }
        #endregion
    }
}