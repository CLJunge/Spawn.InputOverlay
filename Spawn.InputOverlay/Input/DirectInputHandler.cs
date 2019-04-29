#region Using
using SharpDX.DirectInput;
using Spawn.InputOverlay.Properties;
using System;
using System.Diagnostics;
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
        private Joystick m_controller;
        private readonly DispatcherTimer m_connectionTimer;
        private readonly DispatcherTimer m_dataTimer;
        #endregion

        #region Properties
        public bool IsDeviceConnected => m_controller != null;
        #endregion

        #region Ctor
        public DirectInputHandler()
        {
            m_connectionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100),
            };
            m_connectionTimer.Tick += OnConnectionTimerTick;

            m_dataTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(Settings.Default.RefreshRate),
            };
            m_dataTimer.Tick += OnDataTimerTick;

            m_connectionTimer.Start();
        }
        #endregion

        #region FindController
        private Joystick FindController()
        {
            Joystick retVal = null;

            DirectInput directInput = new DirectInput();
            Guid gamepadId = Guid.Empty;

            //Playstation 4
            foreach (DeviceInstance deviceInstance in directInput.GetDevices(DeviceType.FirstPerson, DeviceEnumerationFlags.AttachedOnly))
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

        #region OnDataTimerTick
        private void OnDataTimerTick(object sender, EventArgs e)
        {
            if (IsDeviceConnected)
            {
                bool blnSuccess = CheckInput();

                if (!blnSuccess)
                {
                    m_controller = null;

                    DeviceDisconnected?.Invoke(this, EventArgs.Empty);

                    m_dataTimer.Stop();
                    m_connectionTimer.Start();
                }
            }
        }
        #endregion

        #region OnConnectionTimerTick
        private void OnConnectionTimerTick(object sender, EventArgs e)
        {
            if (!IsDeviceConnected)
                m_controller = FindController();

            if (IsDeviceConnected)
            {
                DeviceConnected?.Invoke(this, EventArgs.Empty);

                m_connectionTimer.Stop();
                m_dataTimer.Start();
            }
        }
        #endregion

        #region CheckInput
        private bool CheckInput()
        {
            bool blnRet = false;

            try
            {
                m_controller.Poll();

                JoystickUpdate[] vData = m_controller.GetBufferedData();

                for (int i = 0; i < vData.Length; i++)
                {
                    JoystickUpdate gamepadData = vData[i];
                }

                blnRet = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return blnRet;
        }
        #endregion

        #region RestartTime
        public void RestartTimer()
        {
            if (m_dataTimer != null)
            {
                m_dataTimer.Stop();
                m_dataTimer.Interval = TimeSpan.FromMilliseconds(Settings.Default.RefreshRate);
                m_dataTimer.Start();
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            m_connectionTimer?.Stop();
            m_dataTimer?.Stop();
        }
        #endregion
    }
}