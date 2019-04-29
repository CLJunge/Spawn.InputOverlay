#region Using
using Spawn.InputOverlay.Input;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
#endregion

namespace Spawn.InputOverlay.UI.ViewModels
{
    public class OverlayViewModel : ViewModelBase
    {
        #region Member Variables
        private readonly IInputHandler m_inputHandler;
        private SolidColorBrush m_windowBackground;
        private OverlayShape m_selectedShape;
        private SolidColorBrush m_accelerateBrush;
        private SolidColorBrush m_brakeBrush;
        private Visibility m_noDeviceLabelVisibility;
        private SolidColorBrush m_noDeviceLabelBrush;
        private ResizeMode m_resizeMode;
        private string m_strToggleResizeGridHeader;
        #endregion

        #region Properties
        #region WindowBackground
        public SolidColorBrush WindowBackground
        {
            get => m_windowBackground;
            set => Set(ref m_windowBackground, value);
        }
        #endregion

        #region SelectedShape
        public OverlayShape SelectedShape
        {
            get => m_selectedShape;
            set => Set(ref m_selectedShape, value);
        }
        #endregion

        #region AccelerateBrush
        public SolidColorBrush AccelerateBrush
        {
            get => m_accelerateBrush;
            set => Set(ref m_accelerateBrush, value);
        }
        #endregion

        #region BrakeBrush
        public SolidColorBrush BrakeBrush
        {
            get => m_brakeBrush;
            set => Set(ref m_brakeBrush, value);
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

        #region ToggleResizeGripCommand
        public ICommand ToggleResizeGripCommand => new RelayCommand(ToggleResizeGrip);
        #endregion

        #region CloseCommand
        public ICommand CloseCommand => new RelayCommand(Close);
        #endregion

        #region AvailableShapes
        public OverlayShape[] AvailableShapes => new OverlayShape[]
        {
            OverlayShape.None,
            OverlayShape.Eye,
            OverlayShape.Trapez
        };
        #endregion
        #endregion

        #region Ctor
        public OverlayViewModel()
        {
            m_inputHandler = new XboxOneInputHandler();

            //m_inputHandler.DeviceConnected += (s, e) => Debug.WriteLine("Device connected");
            //m_inputHandler.DeviceDisconnected += (s, e) => Debug.WriteLine("Device disconnected");
            //m_inputHandler.InputUpdated += (s, e) => Debug.WriteLine(e.LeftStickX);

            //WindowBackground = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
            LoadDefaultValues();
        }
        #endregion

        #region LoadDefaultValues
        private void LoadDefaultValues()
        {
            WindowBackground = new SolidColorBrush(Colors.Magenta);
            SelectedShape = OverlayShape.None;
            AccelerateBrush = new SolidColorBrush(Colors.GreenYellow);
            BrakeBrush = new SolidColorBrush(Colors.Red);
            NoDeviceLabelVisibility = Visibility.Visible;
            NoDeviceLabelBrush = new SolidColorBrush(Colors.Black);
            ResizeMode = ResizeMode.NoResize;
            ToggleResizeGridHeader = "Show resize grip";
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

        #region Close
        private void Close() => App.Current.Shutdown();
        #endregion
    }
}