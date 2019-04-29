#region Using
using SharpDX.DirectInput;
using Spawn.InputOverlay.Properties;
using System;
using System.Windows.Threading;
#endregion

namespace Spawn.InputOverlay.Input
{
    public class DirectInputHandler : IInputHandler
    {
        #region EventHandler
        public event EventHandler DeviceConnected;
        public event EventHandler DeviceDisconnected;
        public event EventHandler<InputUpdatedEventArgs> InputUpdated;
        #endregion

        #region Member Variables
        private readonly Joystick m_controller;
        private readonly DispatcherTimer m_timer;

        private bool m_blnPrevIsDeviceConntectedValue;
        private bool m_blnFiredInitialEvent;
        #endregion

        #region Properties
        public bool IsDeviceConnected => m_controller != null;
        #endregion

        #region Ctor
        public DirectInputHandler()
        {
            m_controller = FindController();

            m_timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(Settings.Default.RefreshRate),
            };
            m_timer.Tick += OnTimerTick;
            m_timer.Start();

            m_blnPrevIsDeviceConntectedValue = IsDeviceConnected;
        }
        #endregion

        #region FindController
        private Joystick FindController()
        {
            Joystick retVal = null;

            DirectInput directInput = new DirectInput();
            Guid gamepadId = Guid.Empty;

            foreach (DeviceInstance deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly))
            {
                gamepadId = deviceInstance.InstanceGuid;

                if (gamepadId != Guid.Empty)
                    break;
            }

            if (gamepadId != Guid.Empty)
            {
                retVal = new Joystick(directInput, gamepadId);

                retVal.Properties.BufferSize = 128;
                retVal.Acquire();
            }

            return retVal;
        }
        #endregion

        #region OnTimerTick
        private void OnTimerTick(object sender, EventArgs e)
        {
            //Debug.WriteLine("Tick");

            CheckConnection();

            if (IsDeviceConnected)
                m_controller.Poll();

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
            JoystickUpdate[] vData = m_controller.GetBufferedData();

            for (int i = 0; i < vData.Length; i++)
            {
                JoystickUpdate joystickUpdate = vData[i];
            }
        }
        #endregion

        #region RestartTime
        public void RestartTimer()
        {
            m_timer.Stop();
            m_timer.Interval = TimeSpan.FromMilliseconds(Settings.Default.RefreshRate);
            m_timer.Start();
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