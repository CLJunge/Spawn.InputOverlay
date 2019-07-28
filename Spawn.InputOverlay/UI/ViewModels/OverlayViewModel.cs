#region Using
using NLog;
using SharpDX.XInput;
using Spawn.InputOverlay.Input;
using Spawn.InputOverlay.Properties;
using System;
using System.Windows;
using System.Windows.Media;
#endregion

namespace Spawn.InputOverlay.UI.ViewModels
{
    public class OverlayViewModel : ViewModelBase
    {
        #region Constants
        private const double DefaultWindowHeight = 250;
        private const double DefaultWindowWidth = 400;
        #endregion

        #region Logger
        private static readonly Logger s_logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Member Variables
        private string m_strWindowTitle;
        private double m_dblWindowHeight;
        private double m_dblWindowWidth;
        private Color m_windowBackgroundColor;
        private OverlayShape m_selectedShape;
        private Color m_accelerateColor;
        private Color m_brakeColor;
        private Color m_steerColor;
        private Color m_segmentBackgroundColor;
        private Visibility m_noDeviceLabelVisibility;
        private ResizeMode m_resizeMode;
        private WindowStyle m_windowStyle;
        private string m_strToggleResizeGridHeader;
        private string m_strToggleWindowBorderHeader;
        private bool m_blnIsDeviceConnected;
        private int m_nRefreshRate;
        private float m_fLeftOffset;
        private float m_fRightOffset;
        private float m_fDeadZone;
        private bool m_blnUseDPadForSteering;
        private Color m_noDeviceLabelColor;
        private GamepadButtons m_accelerateButton;
        private GamepadButtons m_brakeButton;

        private OverlayShape? m_currentShape;
        private bool m_blnIsAccelerateButtonPressed;
        private bool m_blnIsBrakeButtonPressed;
        #endregion

        #region Properties
        #region WindowTitle
        public string WindowTitle
        {
            get => m_strWindowTitle;
            set => Set(ref m_strWindowTitle, value);
        }
        #endregion

        #region AppName
        public string AppName => App.AppName;
        #endregion

        #region WindowHeight
        public double WindowHeight
        {
            get => m_dblWindowHeight;
            set => Set(ref m_dblWindowHeight, value);
        }
        #endregion

        #region WindowWidth
        public double WindowWidth
        {
            get => m_dblWindowWidth;
            set => Set(ref m_dblWindowWidth, value);
        }
        #endregion

        #region WindowBackgroundColor
        public Color WindowBackgroundColor
        {
            get => m_windowBackgroundColor;
            set => Set(ref m_windowBackgroundColor, value);
        }
        #endregion

        #region WindowBackgroundBrush
        public SolidColorBrush WindowBackgroundBrush => new SolidColorBrush(WindowBackgroundColor);
        #endregion

        #region SelectedShape
        public OverlayShape SelectedShape
        {
            get => m_selectedShape;
            set => Set(ref m_selectedShape, value);
        }
        #endregion

        #region AccelerateColor
        public Color AccelerateColor
        {
            get => m_accelerateColor;
            set => Set(ref m_accelerateColor, value);
        }
        #endregion

        #region AccelerateBrush
        public SolidColorBrush AccelerateBrush => new SolidColorBrush(m_blnIsAccelerateButtonPressed ? AccelerateColor : SegmentBackgroundColor);
        #endregion

        #region BrakeColor
        public Color BrakeColor
        {
            get => m_brakeColor;
            set => Set(ref m_brakeColor, value);
        }
        #endregion

        #region BrakeBrush
        public SolidColorBrush BrakeBrush => new SolidColorBrush(m_blnIsBrakeButtonPressed ? BrakeColor : SegmentBackgroundColor);
        #endregion

        #region SteerColor
        public Color SteerColor
        {
            get => m_steerColor;
            set => Set(ref m_steerColor, value);
        }
        #endregion

        #region SteerBrush
        public SolidColorBrush SteerBrush => new SolidColorBrush(SteerColor);
        #endregion

        #region SegmentBackgroundColor
        public Color SegmentBackgroundColor
        {
            get => m_segmentBackgroundColor;
            set => Set(ref m_segmentBackgroundColor, value);
        }
        #endregion

        #region SegmentBackgroundBrush
        public SolidColorBrush SegmentBackgroundBrush => new SolidColorBrush(SegmentBackgroundColor);
        #endregion

        #region NoDeviceLabelVisibility
        public Visibility NoDeviceLabelVisibility
        {
            get => m_noDeviceLabelVisibility;
            set => Set(ref m_noDeviceLabelVisibility, value);
        }
        #endregion

        #region ResizeMode
        public ResizeMode ResizeMode
        {
            get => m_resizeMode;
            set => Set(ref m_resizeMode, value);
        }
        #endregion

        #region WindowStyle
        public WindowStyle WindowStyle
        {
            get => m_windowStyle;
            set => Set(ref m_windowStyle, value);
        }
        #endregion

        #region ToggleResizeGridHeader
        public string ToggleResizeGridHeader
        {
            get => m_strToggleResizeGridHeader;
            set => Set(ref m_strToggleResizeGridHeader, value);
        }
        #endregion

        #region ToggleWindowBorderHeader
        public string ToggleWindowBorderHeader
        {
            get => m_strToggleWindowBorderHeader;
            set => Set(ref m_strToggleWindowBorderHeader, value);
        }
        #endregion

        #region IsDeviceConnected
        public bool IsDeviceConnected
        {
            get => m_blnIsDeviceConnected;
            set => Set(ref m_blnIsDeviceConnected, value);
        }
        #endregion

        #region RefreshRate
        public int RefreshRate
        {
            get => m_nRefreshRate;
            set => Set(ref m_nRefreshRate, value);
        }
        #endregion

        #region LeftOffset
        public float LeftOffset
        {
            get => m_fLeftOffset;
            set => Set(ref m_fLeftOffset, value);
        }
        #endregion

        #region RightOffset
        public float RightOffset
        {
            get => m_fRightOffset;
            set => Set(ref m_fRightOffset, value);
        }
        #endregion

        #region DeadZone
        public float DeadZone
        {
            get => m_fDeadZone;
            set => Set(ref m_fDeadZone, value);
        }
        #endregion

        #region UseDPadForSteering
        public bool UseDPadForSteering
        {
            get => m_blnUseDPadForSteering;
            set => Set(ref m_blnUseDPadForSteering, value);
        }
        #endregion

        #region NoDeviceLabelColor
        public Color NoDeviceLabelColor
        {
            get => m_noDeviceLabelColor;
            set => Set(ref m_noDeviceLabelColor, value);
        }
        #endregion

        #region NoDeviceLabelBrush
        public SolidColorBrush NoDeviceLabelBrush => new SolidColorBrush(NoDeviceLabelColor);
        #endregion

        #region AccelerateButton
        public GamepadButtons AccelerateButton
        {
            get => m_accelerateButton;
            set => Set(ref m_accelerateButton, value);
        }
        #endregion

        #region BrakeButton
        public GamepadButtons BrakeButton
        {
            get => m_brakeButton;
            set => Set(ref m_brakeButton, value);
        }
        #endregion

        #region ToggleResizeGripCommand
        public System.Windows.Input.ICommand ToggleResizeGripCommand => new RelayCommand(ToggleResizeGrip);
        #endregion

        #region ToggleWindowBorderCommand
        public System.Windows.Input.ICommand ToggleWindowBorderCommand => new RelayCommand(ToggleWindowBorder);
        #endregion

        #region ResetSizeCommand
        public System.Windows.Input.ICommand ResetSizeCommand => new RelayCommand(ResetSize,
            () => WindowHeight != DefaultWindowHeight || WindowWidth != DefaultWindowWidth);
        #endregion

        #region OpenAboutWindowCommand
        public System.Windows.Input.ICommand OpenAboutWindowCommand => new RelayCommand(OpenAboutWindow);
        #endregion

        #region CloseCommand
        public System.Windows.Input.ICommand CloseCommand => new RelayCommand(Close);
        #endregion

        #region AvailableShapes
        public OverlayShape[] AvailableShapes => new OverlayShape[]
        {
            OverlayShape.None,
            OverlayShape.Eye,
            OverlayShape.CatEye,
            OverlayShape.Trapez
        };
        #endregion

        #region AvailableButtons
        public GamepadButtons[] AvailableButtons => new GamepadButtons[]
        {
            GamepadButtons.A,
            GamepadButtons.B,
            GamepadButtons.X,
            GamepadButtons.Y,
            GamepadButtons.LeftShoulder,
            GamepadButtons.RightShoulder,
            GamepadButtons.LeftTrigger,
            GamepadButtons.RightTrigger
        };
        #endregion
        #endregion

        #region Ctor
        public OverlayViewModel() => Initialize();
        #endregion

        #region Initialize
        private void Initialize()
        {
            s_logger.Trace("Initializing...");

            PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(WindowBackgroundColor):
                        RaisePropertyChangedEvent(nameof(WindowBackgroundBrush));
                        RaisePropertyChangedEvent(nameof(NoDeviceLabelBrush));
                        break;

                    case nameof(AccelerateColor):
                        RaisePropertyChangedEvent(nameof(AccelerateBrush));
                        break;

                    case nameof(BrakeColor):
                        RaisePropertyChangedEvent(nameof(BrakeBrush));
                        break;

                    case nameof(SteerColor):
                        RaisePropertyChangedEvent(nameof(SteerBrush));
                        break;

                    case nameof(SegmentBackgroundColor):
                        RaisePropertyChangedEvent(nameof(SegmentBackgroundBrush));
                        break;

                    case nameof(NoDeviceLabelColor):
                        RaisePropertyChangedEvent(nameof(NoDeviceLabelBrush));
                        break;

                    case nameof(RefreshRate):
                        DeviceManager.Instance.RestartInputTimer();
                        break;
                }
            };

            DeviceManager.Instance.DeviceConnected += OnDeviceConnected;
            DeviceManager.Instance.DeviceDisconnected += OnDeviceDisconnected;
            DeviceManager.Instance.StartConnectionTimers();

            LoadValues();
        }
        #endregion

        #region LoadValues
        private void LoadValues()
        {
            ResetSize();

            WindowTitle = $"{AppName} v{App.AppVersion}";
            WindowBackgroundColor = Settings.Default.WindowBackgroundColor == Colors.Transparent ? Colors.Magenta : Settings.Default.WindowBackgroundColor;
            SelectedShape = OverlayShape.None;
            AccelerateColor = Settings.Default.AccelerateColor;
            BrakeColor = Settings.Default.BrakeColor;
            SteerColor = Settings.Default.SteerColor;
            SegmentBackgroundColor = Settings.Default.SegmentBackgroundColor;
            NoDeviceLabelVisibility = Visibility.Visible;
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.None;
            ToggleResizeGridHeader = "Show resize grip";
            ToggleWindowBorderHeader = "Show window border";
            IsDeviceConnected = false;
            RefreshRate = Settings.Default.RefreshRate;
            LeftOffset = 0;
            RightOffset = 0;
            DeadZone = 0;
            UseDPadForSteering = Settings.Default.UseDPadForSteering;
            NoDeviceLabelColor = Settings.Default.NoDeviceLabelColor;
            AccelerateButton = Settings.Default.AccelerateButton;
            BrakeButton = Settings.Default.BrakeButton;

            s_logger.Debug("Loaded initial values");
        }
        #endregion

        #region ToggleResizeGrip
        private void ToggleResizeGrip()
        {
            switch (ResizeMode)
            {
                case ResizeMode.NoResize:
                    s_logger.Debug("Showing resize grip");

                    ResizeMode = ResizeMode.CanResizeWithGrip;
                    ToggleResizeGridHeader = "Hide resize grip";
                    break;

                case ResizeMode.CanResizeWithGrip:
                    s_logger.Debug("Hiding resize grip");

                    ResizeMode = ResizeMode.NoResize;
                    ToggleResizeGridHeader = "Show resize grip";
                    break;
            }

            s_logger.Info("Toggeled resize grip");
        }
        #endregion

        #region ToggleWindowBorder
        private void ToggleWindowBorder()
        {
            switch (WindowStyle)
            {
                case WindowStyle.None:
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    ToggleWindowBorderHeader = "Hide window border";
                    break;

                case WindowStyle.SingleBorderWindow:
                    WindowStyle = WindowStyle.None;
                    ToggleWindowBorderHeader = "Show window border";
                    break;
            }

            s_logger.Info("Toggeled window border");
        }
        #endregion

        #region ResetSize
        private void ResetSize()
        {
            s_logger.Trace("Reset window size");

            WindowHeight = DefaultWindowHeight;
            WindowWidth = DefaultWindowWidth;
        }
        #endregion

        #region OpenAboutWindow
        private void OpenAboutWindow()
        {
            s_logger.Trace("Opening about window...");
            //TODO
        }
        #endregion

        #region Close
        private void Close()
        {
            s_logger.Trace("Closing...");

            DeviceManager.Instance.DeviceConnected -= OnDeviceConnected;
            DeviceManager.Instance.DeviceDisconnected -= OnDeviceDisconnected;
            DeviceManager.Instance.Dispose();

            SaveSettings();

            Application.Current.Shutdown();
        }
        #endregion

        #region SaveSettings
        private void SaveSettings()
        {
            s_logger.Debug("Saving settings...");
            s_logger.Debug("WindowBackgroundColor: {0}", WindowBackgroundColor);
            s_logger.Debug("Shape: {0}", SelectedShape);
            s_logger.Debug("AccelerateColor: {0}", AccelerateColor);
            s_logger.Debug("BrakeColor: {0}", BrakeColor);
            s_logger.Debug("SteerColor: {0}", SteerColor);
            s_logger.Debug("SegmentBackgroundColor: {0}", SegmentBackgroundColor);
            s_logger.Debug("RefreshRate: {0}", RefreshRate);
            s_logger.Debug("UseDPadForSteering {0}", UseDPadForSteering);
            s_logger.Debug("NoDeviceLabelColor {0}", NoDeviceLabelColor);
            s_logger.Debug("AccelerateButton {0}", AccelerateButton);
            s_logger.Debug("BrakeButton {0}", BrakeButton);

            Settings.Default.WindowBackgroundColor = WindowBackgroundColor;
            Settings.Default.Shape = SelectedShape == OverlayShape.None ? (m_currentShape ?? SelectedShape) : SelectedShape;
            Settings.Default.AccelerateColor = AccelerateColor;
            Settings.Default.BrakeColor = BrakeColor;
            Settings.Default.SteerColor = SteerColor;
            Settings.Default.SegmentBackgroundColor = SegmentBackgroundColor;
            Settings.Default.RefreshRate = RefreshRate;
            Settings.Default.UseDPadForSteering = UseDPadForSteering;
            Settings.Default.NoDeviceLabelColor = NoDeviceLabelColor;
            Settings.Default.AccelerateButton = AccelerateButton;
            Settings.Default.BrakeButton = BrakeButton;

            Settings.Default.Save();

            s_logger.Info("Saved settings successfully");
        }
        #endregion

        #region DeviceManager Stuff
        #region OnDeviceConnected
        private void OnDeviceConnected(object sender, EventArgs e)
        {
            IInputHandler inputHandler = sender as IInputHandler;
            inputHandler.InputUpdated += OnInputUpdated;
            s_logger.Debug("Using {0}", inputHandler.GetType().Name);

            NoDeviceLabelVisibility = Visibility.Collapsed;
            SelectedShape = m_currentShape ?? Settings.Default.Shape;
            IsDeviceConnected = true;
        }
        #endregion

        #region OnDeviceDisconnected
        private void OnDeviceDisconnected(object sender, EventArgs e)
        {
            IInputHandler inputHandler = sender as IInputHandler;
            inputHandler.InputUpdated -= OnInputUpdated;

            m_currentShape = SelectedShape;
            SelectedShape = OverlayShape.None;
            NoDeviceLabelVisibility = Visibility.Visible;
            IsDeviceConnected = false;
        }
        #endregion
        #endregion

        #region OnInputUpdated
        private void OnInputUpdated(object sender, InputEventArgs e)
        {
            //s_logger.Debug("Updating input...");

            m_blnIsAccelerateButtonPressed = (AccelerateButton != GamepadButtons.LeftTrigger && AccelerateButton != GamepadButtons.RightTrigger && e.PressedButtons.HasFlag((GamepadButtonFlags)(int)AccelerateButton)) || (AccelerateButton == GamepadButtons.RightTrigger ? e.IsRightTriggerPressed : (AccelerateButton == GamepadButtons.LeftTrigger ? e.IsLeftTriggerPressed : false));
            m_blnIsBrakeButtonPressed = (BrakeButton != GamepadButtons.RightTrigger && BrakeButton != GamepadButtons.LeftTrigger && e.PressedButtons.HasFlag((GamepadButtonFlags)(int)BrakeButton)) || (BrakeButton == GamepadButtons.LeftTrigger ? e.IsLeftTriggerPressed : (BrakeButton == GamepadButtons.RightTrigger ? e.IsRightTriggerPressed : false));

            if (UseDPadForSteering)
            {
                LeftOffset = e.PressedButtons.HasFlag(GamepadButtonFlags.DPadLeft) ? 1 : 0;
                RightOffset = e.PressedButtons.HasFlag(GamepadButtonFlags.DPadRight) ? 1 : 0;
            }
            else
            {
                if (e.LeftStickX < -DeadZone)
                {
                    LeftOffset = Math.Abs(1 / (1 - DeadZone) * ((float)e.LeftStickX + DeadZone));
                    RightOffset = 0;
                }
                else if (e.LeftStickX > DeadZone)
                {
                    RightOffset = Math.Abs(1 / (1 - DeadZone) * ((float)e.LeftStickX - DeadZone));
                    LeftOffset = 0;
                }
                else
                {
                    LeftOffset = RightOffset = 0;
                }
            }

            RaisePropertyChangedEvent(nameof(AccelerateBrush));
            RaisePropertyChangedEvent(nameof(BrakeBrush));
        }
        #endregion
    }
}