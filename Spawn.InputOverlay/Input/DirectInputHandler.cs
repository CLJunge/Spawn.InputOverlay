#region Using
using SharpDX.DirectInput;
using System;
#endregion

namespace Spawn.InputOverlay.Input
{
    public class DirectInputHandler : InputHandlerBase
    {
        #region Member Variables
        private Joystick m_controller;
        #endregion

        #region Properties
        public override bool IsDeviceConnected => m_controller != null;
        #endregion

        #region Ctor
        public DirectInputHandler()
        {
            m_connectionTimer.Tick += OnConnectionTimerTick;
            m_inputTimer.Tick += OnDataTimerTick;
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
                if (!CheckInput())
                {
                    m_inputTimer.Stop();
                    m_controller = null;

                    RaiseDeviceDisconnectedEvent(this, EventArgs.Empty);

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
                RaiseDeviceConnectedEvent(this, EventArgs.Empty);

                m_connectionTimer.Stop();
                m_inputTimer.Start();
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
                    //TODO
                }

                blnRet = true;
            }
            catch (Exception ex)
            {
                s_logger.Debug(ex, "No device connected!");
            }

            return blnRet;
        }
        #endregion
    }
}