#region Using
using NLog;
using SharpDX.XInput;
using System;
#endregion

namespace Spawn.InputOverlay.Input
{
    public class XInputHandler : InputHandlerBase
    {
        #region Logger
        protected override Logger Log => s_logger;
        private static readonly Logger s_logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Member Variables
        private readonly Controller m_controller;
        private bool m_blnPrevIsDeviceConntectedValue;
        private bool m_blnFiredInitialEvent;
        #endregion

        #region Properties
        public override bool IsDeviceConnected => (m_controller?.IsConnected ?? false);
        #endregion

        #region Ctor
        public XInputHandler(UserIndex userIndex = UserIndex.One)
            : base(InputType.XInput)
        {
            m_controller = new Controller(userIndex);

            m_connectionTimer.Tick += OnConnectionTimerTick;
            m_inputTimer.Tick += OnDataTimerTick;

            m_blnPrevIsDeviceConntectedValue = IsDeviceConnected;
        }
        #endregion

        #region OnDataTimerTick
        private void OnDataTimerTick(object sender, EventArgs e)
        {
            if (IsDeviceConnected && m_controller.GetState(out State state))
            {
                double dblLeftStickX = Math.Round(state.Gamepad.LeftThumbX / 32767f, 4);
                bool blnIsLeftTriggerPressed = state.Gamepad.LeftTrigger != 0;
                bool blnIsRightTriggerPressed = state.Gamepad.RightTrigger != 0;

                InputEventArgs args = new InputEventArgs(dblLeftStickX,
                    blnIsLeftTriggerPressed, blnIsRightTriggerPressed,
                    state.Gamepad.Buttons);

                RaiseInputUpdatedEvent(this, args);
            }
        }
        #endregion

        #region OnConnectionTimerTick
        private void OnConnectionTimerTick(object sender, EventArgs e)
        {
            Log.Trace("Checking device connection...");

            if (!m_blnFiredInitialEvent)
            {
                if (IsDeviceConnected)
                {
                    Log.Info("Device connected");

                    RaiseDeviceConnectedEvent(this, EventArgs.Empty);
                }

                m_blnFiredInitialEvent = true;
            }

            if (IsDeviceConnected != m_blnPrevIsDeviceConntectedValue)
            {
                if (IsDeviceConnected)
                {
                    Log.Info("Device connected");

                    RaiseDeviceConnectedEvent(this, EventArgs.Empty);
                }
                else
                {
                    Log.Info("Device disconnected");

                    RaiseDeviceDisconnectedEvent(this, EventArgs.Empty);
                }
            }

            if (IsDeviceConnected)
                m_inputTimer.Start();

            m_blnPrevIsDeviceConntectedValue = IsDeviceConnected;
        }
        #endregion
    }
}