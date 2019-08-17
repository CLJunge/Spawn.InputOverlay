#region Using
using NLog;
using SharpDX.DirectInput;
using SharpDX.XInput;
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
        private DirectInputDeviceType m_deviceType;

        private double m_dblLeftStickXValue;
        private bool m_blnIsCrossButtonPressed;
        private bool m_blnIsCircleButtonPressed;
        private bool m_blnIsSquareButtonPressed;
        private bool m_blnIsTriangleButtonPressed;
        private bool m_blnIsLeftShoulderPressed;
        private bool m_blnIsRightShoulderPressed;
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
            : base(InputType.DirectInput)
        {
            m_connectionTimer.Tick += OnConnectionTimerTick;
            m_inputTimer.Tick += OnInputTimerTick;

            m_deviceType = DirectInputDeviceType.None;
        }
        #endregion

        #region OnInputTimerTick
        private void OnInputTimerTick(object sender, EventArgs e)
        {
            if (IsDeviceConnected)
            {
                if (!CheckInput())
                {
                    Log.Info("Device disconnected");

                    m_inputTimer.Stop();
                    m_controller = null;
                    m_deviceType = DirectInputDeviceType.None;

                    RaiseDeviceDisconnectedEvent(this, EventArgs.Empty);

                    m_connectionTimer.Start();
                }
            }
        }
        #endregion

        #region OnConnectionTimerTick
        private void OnConnectionTimerTick(object sender, EventArgs e)
        {
            Log.Trace("Checking device connection...");

            if (!IsDeviceConnected)
                m_controller = FindController();

            if (IsDeviceConnected)
            {
                Log.Info("Device connected");

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
            foreach (DeviceInstance deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.FirstPerson, DeviceEnumerationFlags.AttachedOnly))
            {
                gamepadId = deviceInstance.InstanceGuid;

                if (gamepadId != Guid.Empty)
                {
                    m_deviceType = DirectInputDeviceType.DualShock4;

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
                    case DirectInputDeviceType.DualShock4:
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
            JoystickUpdate circleButtonData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.Buttons2);
            JoystickUpdate squareButtonData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.Buttons0);
            JoystickUpdate triangleButtonData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.Buttons3);
            JoystickUpdate leftShoulderData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.Buttons4);
            JoystickUpdate rightShoulderData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.Buttons5);
            JoystickUpdate leftTriggerData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.Buttons6);
            JoystickUpdate rightTriggerData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.Buttons7);
            JoystickUpdate dPadData = vData.FirstOrDefault(i => i.Offset == JoystickOffset.PointOfViewControllers0);

            if (leftStickXData.Timestamp != 0)
                m_dblLeftStickXValue = Math.Round(leftStickXData.Value / LeftStickXIdleValue, 4) - 1;

            if (crossButtonData.Timestamp != 0)
                m_blnIsCrossButtonPressed = crossButtonData.Value == 128;

            if (circleButtonData.Timestamp != 0)
                m_blnIsCircleButtonPressed = circleButtonData.Value == 128;

            if (squareButtonData.Timestamp != 0)
                m_blnIsSquareButtonPressed = squareButtonData.Value == 128;

            if (triangleButtonData.Timestamp != 0)
                m_blnIsTriangleButtonPressed = triangleButtonData.Value == 128;

            if (leftShoulderData.Timestamp != 0)
                m_blnIsLeftShoulderPressed = leftShoulderData.Value == 128;

            if (rightShoulderData.Timestamp != 0)
                m_blnIsRightShoulderPressed = rightShoulderData.Value == 128;

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
                (byte)(m_blnIsLeftTriggerPressed ? 255 : 0), (byte)(m_blnIsRightTriggerPressed ? 255 : 0),
                GetPressedButtons());

            RaiseInputUpdatedEvent(this, args);
        }
        #endregion

        #region GetPressedButtons
        private GamepadButtonFlags GetPressedButtons()
        {
            GamepadButtonFlags retVal = GamepadButtonFlags.None;

            if (m_blnIsCrossButtonPressed)
                retVal |= GamepadButtonFlags.A;

            if (m_blnIsCircleButtonPressed)
                retVal |= GamepadButtonFlags.B;

            if (m_blnIsSquareButtonPressed)
                retVal |= GamepadButtonFlags.X;

            if (m_blnIsTriangleButtonPressed)
                retVal |= GamepadButtonFlags.Y;

            if (m_blnIsLeftShoulderPressed)
                retVal |= GamepadButtonFlags.LeftThumb;

            if (m_blnIsRightShoulderPressed)
                retVal |= GamepadButtonFlags.RightThumb;

            if (m_blnIsDPadLeftPressed)
                retVal |= GamepadButtonFlags.DPadLeft;

            if (m_blnIsDPadRightPressed)
                retVal |= GamepadButtonFlags.DPadRight;

            return retVal;
        }
        #endregion
    }
}