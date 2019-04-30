#region Using
using NLog;
using SharpDX.DirectInput;
using System;
using System.Linq;
#endregion

namespace Spawn.InputOverlay.Input
{
    public class DirectInputHandler : InputHandlerBase
    {
        #region Constants
        public const int LeftStickXMaxValue = 65535;
        public const double LeftStickXIdleValue = LeftStickXMaxValue / 2.0;
        #endregion

        #region Logger
        protected override Logger Log => s_logger;
        private static readonly Logger s_logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Member Variables
        private Joystick m_controller;
        private DirectInputDevice m_deviceType;

        private double m_dblLeftStickXValue;
        private bool m_blnIsSquareButtonPressed;
        private bool m_blnIsCrossButtonPressed;
        private bool m_blnIsLeftTriggerPressed;
        private bool m_blnIsRightTriggerPressed;
        private bool m_blnIsDPadLeftPressed;
        private bool m_blnIsDPadRightPressed;
        #endregion

        #region Properties
        public override bool IsDeviceConnected => m_controller != null;
        #endregion

        #region Ctor
        public DirectInputHandler()
        {
            m_connectionTimer.Tick += OnConnectionTimerTick;
            m_inputTimer.Tick += OnDataTimerTick;

            m_deviceType = DirectInputDevice.None;
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
                    m_deviceType = DirectInputDevice.None;

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

        #region FindController
        private Joystick FindController()
        {
            Joystick retVal = null;

            DirectInput directInput = new DirectInput();
            Guid gamepadId = Guid.Empty;

            //DualShock4
            foreach (DeviceInstance deviceInstance in directInput.GetDevices(DeviceType.FirstPerson, DeviceEnumerationFlags.AttachedOnly))
            {
                gamepadId = deviceInstance.InstanceGuid;

                if (gamepadId != Guid.Empty)
                {
                    m_deviceType = DirectInputDevice.DualShock4;

                    break;
                }
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

        #region CheckInput
        private bool CheckInput()
        {
            bool blnRet = false;

            try
            {
                m_controller.Poll();

                JoystickUpdate[] vData = m_controller.GetBufferedData();

                switch (m_deviceType)
                {
                    case DirectInputDevice.DualShock4:
                        HandleDualShock4Input(vData);
                        break;
                }

                blnRet = true;
            }
            catch (Exception ex)
            {
                Log.Debug(ex, "No device connected!");
            }

            return blnRet;
        }
        #endregion

        #region HandleDualShock4Input
        private void HandleDualShock4Input(JoystickUpdate[] vData)
        {
            JoystickUpdate leftStickXData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.X);
            JoystickUpdate crossButtonData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.Buttons1);
            JoystickUpdate squareButtonData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.Buttons0);
            JoystickUpdate leftTriggerData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.Buttons6);
            JoystickUpdate rightTriggerData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.Buttons7);
            JoystickUpdate dPadData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.PointOfViewControllers0);

            if (leftStickXData.Timestamp != 0)
                m_dblLeftStickXValue = Math.Round(leftStickXData.Value / LeftStickXIdleValue, 4) - 1;

            if (crossButtonData.Timestamp != 0)
                m_blnIsCrossButtonPressed = crossButtonData.Value == 128;

            if (squareButtonData.Timestamp != 0)
                m_blnIsSquareButtonPressed = squareButtonData.Value == 128;

            if (leftTriggerData.Timestamp != 0)
                m_blnIsLeftTriggerPressed = leftTriggerData.Value == 128;

            if (rightTriggerData.Timestamp != 0)
                m_blnIsRightTriggerPressed = rightTriggerData.Value == 128;

            if (dPadData.Timestamp != 0)
            {
                switch (dPadData.Value)
                {
                    case 22500:
                    case 27000:
                    case 31500:
                        m_blnIsDPadLeftPressed = true;
                        break;

                    case 4500:
                    case 9000:
                    case 13500:
                        m_blnIsDPadRightPressed = true;
                        break;

                    default:
                        m_blnIsDPadLeftPressed = false;
                        m_blnIsDPadRightPressed = false;
                        break;
                }
            }

            InputEventArgs args = new InputEventArgs(m_dblLeftStickXValue,
                m_blnIsDPadLeftPressed, m_blnIsDPadRightPressed,
                m_blnIsLeftTriggerPressed, m_blnIsRightTriggerPressed,
                m_blnIsSquareButtonPressed, m_blnIsCrossButtonPressed);

            RaiseInputUpdatedEvent(this, args);
        }
        #endregion
    }
}