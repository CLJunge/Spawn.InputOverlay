#region Using
using SharpDX.XInput;
using System;
using System.Windows.Threading;
#endregion

namespace Spawn.InputOverlay.Input
{
    public class XboxInputHandler : IInputHandler
    {
        #region EventHandler
        public event EventHandler DeviceConnected;
        public event EventHandler DeviceDisconnected;
        public event EventHandler<XboxOneInputEventArgs> InputUpdated;
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
        public XboxInputHandler(UserIndex userIndex = UserIndex.One)
        {
            m_controller = new Controller(userIndex);
            m_timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(5),
            };

            m_timer.Tick += OnTimerTick;
            m_timer.Start();

            m_blnPrevIsDeviceConntectedValue = IsDeviceConnected;
        }
        #endregion

        #region OnTimerTick
        private void OnTimerTick(object sender, EventArgs e)
        {
            //Debug.WriteLine("Tick");

            CheckConnection();

            CheckInput();
        }
        #endregion

        #region CheckConnection
        private void CheckConnection()
        {
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
                InputUpdated?.Invoke(this, new XboxOneInputEventArgs(state.Gamepad));
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            m_timer?.Stop();
        }
        #endregion
    }
}