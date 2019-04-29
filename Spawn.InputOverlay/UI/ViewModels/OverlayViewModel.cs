﻿#region Using
using SharpDX.XInput;
using Spawn.InputOverlay.Input;
using Spawn.InputOverlay.Properties;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
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

        #region Member Variables
        private double m_dblWindowHeight;
        private double m_dblWindowWidth;
        private Color m_windowBackgroundColor;
        private OverlayShape m_selectedShape;
        private Color m_accelerateColor;
        private Color m_brakeColor;
        private Color m_steerColor;
        private Color m_segmentBackgroundColor;
        private Visibility m_noDeviceLabelVisibility;
        private SolidColorBrush m_noDeviceLabelBrush;
        private ResizeMode m_resizeMode;
        private string m_strToggleResizeGridHeader;
        private bool m_blnIsDeviceConnected;
        private bool m_blnUseTriggerForAccelerating;
        private bool m_blnUseTriggerForBraking;

        private OverlayShape? m_currentShape;
        private IInputHandler m_inputHandler;
        private bool m_blnIsAccelerateButtonPressed;
        private bool m_blnIsBrakeButtonPressed;
        #endregion

        #region Properties
        #region AppName
        public string AppName => "PadViz (XONE Edition)";
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
        public SolidColorBrush WindowBackgroundBrush
        {
            get => new SolidColorBrush(WindowBackgroundColor);
        }
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
        public SolidColorBrush AccelerateBrush
        {
            get => new SolidColorBrush(m_blnIsAccelerateButtonPressed ? AccelerateColor : SegmentBackgroundColor);
        }
        #endregion

        #region BrakeColor
        public Color BrakeColor
        {
            get => m_brakeColor;
            set => Set(ref m_brakeColor, value);
        }
        #endregion

        #region BrakeBrush
        public SolidColorBrush BrakeBrush
        {
            get => new SolidColorBrush(m_blnIsBrakeButtonPressed ? BrakeColor : SegmentBackgroundColor);
        }
        #endregion

        #region SteerColor
        public Color SteerColor
        {
            get => m_steerColor;
            set => Set(ref m_steerColor, value);
        }
        #endregion

        #region SteerBrush
        public SolidColorBrush SteerBrush
        {
            get => new SolidColorBrush(SteerColor);
        }
        #endregion

        #region SegmentBackgroundColor
        public Color SegmentBackgroundColor
        {
            get => m_segmentBackgroundColor;
            set => Set(ref m_segmentBackgroundColor, value);
        }
        #endregion

        #region SegmentBackgroundBrush
        public SolidColorBrush SegmentBackgroundBrush
        {
            get => new SolidColorBrush(SegmentBackgroundColor);
        }
        #endregion

        #region NoDeviceLabelVisibility
        public Visibility NoDeviceLabelVisibility
        {
            get => m_noDeviceLabelVisibility;
            set => Set(ref m_noDeviceLabelVisibility, value);
        }
        #endregion

        #region NoDeviceLabelBrush
        public SolidColorBrush NoDeviceLabelBrush
        {
            get => m_noDeviceLabelBrush;
            set => Set(ref m_noDeviceLabelBrush, value);
        }
        #endregion

        #region ResizeMode
        public ResizeMode ResizeMode
        {
            get => m_resizeMode;
            set => Set(ref m_resizeMode, value);
        }
        #endregion

        #region ToggleResizeGridHeader
        public string ToggleResizeGridHeader
        {
            get => m_strToggleResizeGridHeader;
            set => Set(ref m_strToggleResizeGridHeader, value);
        }
        #endregion

        #region IsDeviceConnected
        public bool IsDeviceConnected
        {
            get => m_blnIsDeviceConnected;
            set => Set(ref m_blnIsDeviceConnected, value);
        }
        #endregion

        #region UseTriggerForAccelerating
        public bool UseTriggerForAccelerating
        {
            get => m_blnUseTriggerForAccelerating;
            set => Set(ref m_blnUseTriggerForAccelerating, value);
        }
        #endregion

        #region UseTriggerForBraking
        public bool UseTriggerForBraking
        {
            get => m_blnUseTriggerForBraking;
            set => Set(ref m_blnUseTriggerForBraking, value);
        }
        #endregion

        #region ToggleResizeGripCommand
        public ICommand ToggleResizeGripCommand => new RelayCommand(ToggleResizeGrip);
        #endregion

        #region ResetSizeCommand
        public ICommand ResetSizeCommand => new RelayCommand(ResetSize,
            () => WindowHeight != DefaultWindowHeight || WindowWidth != DefaultWindowWidth);
        #endregion

        #region OpenAboutWindowCommand
        public ICommand OpenAboutWindowCommand => new RelayCommand(OpenAboutWindow);
        #endregion

        #region CloseCommand
        public ICommand CloseCommand => new RelayCommand(Close);
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
        #endregion

        #region Ctor
        public OverlayViewModel() => Initialize();
        #endregion

        #region Initialize
        private void Initialize()
        {
            App.Current.Exit += (s, e) => SaveSettings();

            PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(WindowBackgroundColor):
                        RaisePropertyChangedEvent(nameof(WindowBackgroundBrush));
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
                }
            };

            m_inputHandler = new XboxInputHandler();
            m_inputHandler.DeviceConnected += OnDeviceConnected;
            m_inputHandler.DeviceDisconnected += OnDeviceDisconnected;
            m_inputHandler.InputUpdated += OnInputUpdated;

            LoadValues();
        }
        #endregion

        #region LoadValues
        private void LoadValues()
        {
            ResetSize();

            WindowBackgroundColor = Settings.Default.WindowBackgroundColor;
            SelectedShape = OverlayShape.None;
            AccelerateColor = Settings.Default.AccelerateColor;
            BrakeColor = Settings.Default.BrakeColor;
            SteerColor = Settings.Default.SteerColor;
            SegmentBackgroundColor = Settings.Default.SegmentBackgroundColor;
            NoDeviceLabelVisibility = Visibility.Visible;
            NoDeviceLabelBrush = new SolidColorBrush(Colors.Black);
            ResizeMode = ResizeMode.NoResize;
            ToggleResizeGridHeader = "Show resize grip";
            IsDeviceConnected = false;
            UseTriggerForAccelerating = Settings.Default.UseTriggerForAccelerating;
            UseTriggerForBraking = Settings.Default.UseTriggerForBraking;
        }
        #endregion

        #region ToggleResizeGrip
        private void ToggleResizeGrip()
        {
            switch (ResizeMode)
            {
                case ResizeMode.NoResize:
                    ResizeMode = ResizeMode.CanResizeWithGrip;
                    ToggleResizeGridHeader = "Hide resize grip";
                    break;

                case ResizeMode.CanResizeWithGrip:
                    ResizeMode = ResizeMode.NoResize;
                    ToggleResizeGridHeader = "Show resize grip";
                    break;
            }
        }
        #endregion

        #region ResetSize
        private void ResetSize()
        {
            WindowHeight = DefaultWindowHeight;
            WindowWidth = DefaultWindowWidth;
        }
        #endregion

        #region OpenAboutWindow
        private void OpenAboutWindow()
        {
            //TODO
        }
        #endregion

        #region Close
        private void Close() => App.Current.Shutdown();
        #endregion

        #region SaveSettings
        private void SaveSettings()
        {
            Settings.Default.WindowBackgroundColor = WindowBackgroundColor;
            Settings.Default.Shape = SelectedShape;
            Settings.Default.AccelerateColor = AccelerateColor;
            Settings.Default.BrakeColor = BrakeColor;
            Settings.Default.SteerColor = SteerColor;
            Settings.Default.SegmentBackgroundColor = SegmentBackgroundColor;
            Settings.Default.UseTriggerForAccelerating = UseTriggerForAccelerating;
            Settings.Default.UseTriggerForBraking = UseTriggerForBraking;

            Settings.Default.Save();
        }
        #endregion

        #region InputManager Stuff
        #region OnDeviceConnected
        private void OnDeviceConnected(object sender, System.EventArgs e)
        {
            Debug.WriteLine("Device connected");

            NoDeviceLabelVisibility = Visibility.Collapsed;
            SelectedShape = m_currentShape ?? Settings.Default.Shape;
            IsDeviceConnected = true;
        }
        #endregion

        #region OnDeviceDisconnected
        private void OnDeviceDisconnected(object sender, System.EventArgs e)
        {
            Debug.WriteLine("Device disconnected");

            m_currentShape = SelectedShape;
            SelectedShape = OverlayShape.None;
            NoDeviceLabelVisibility = Visibility.Visible;
            IsDeviceConnected = false;
        }
        #endregion

        #region OnInputUpdated
        private void OnInputUpdated(object sender, XboxOneInputEventArgs e)
        {
            Debug.WriteLine("Input updated");

            m_blnIsAccelerateButtonPressed = UseTriggerForAccelerating
                ? e.DeviceState.RightTrigger != 0
                : e.DeviceState.Buttons.HasFlag(GamepadButtonFlags.A);

            m_blnIsBrakeButtonPressed = UseTriggerForBraking
                ? e.DeviceState.LeftTrigger != 0
                : e.DeviceState.Buttons.HasFlag(GamepadButtonFlags.B);

            RaisePropertyChangedEvent(nameof(AccelerateBrush));
            RaisePropertyChangedEvent(nameof(BrakeBrush));
        }
        #endregion
        #endregion
    }
}