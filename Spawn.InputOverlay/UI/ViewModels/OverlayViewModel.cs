#region Using
using Spawn.InputOverlay.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
#endregion

namespace Spawn.InputOverlay.UI.ViewModels
{
    public class OverlayViewModel : INotifyPropertyChanged
    {
        #region EventHandler
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChangedEvent([CallerMemberName]string strPropertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        #endregion

        #region Member Variables
        private readonly IInputHandler m_inputHandler;
        private SolidColorBrush m_windowBackground;
        private OverlayShape m_selectedShape;
        private SolidColorBrush m_accelerateBrush;
        private SolidColorBrush m_brakeBrush;
        private Visibility m_noDeviceLabelVisibility;
        private SolidColorBrush m_noDeviceLabelBrush;
        private ResizeMode m_resizeMode;
        #endregion

        #region Properties
        #region WindowBackground
        public SolidColorBrush WindowBackground
        {
            get => m_windowBackground;
            set
            {
                if (value != m_windowBackground)
                {
                    m_windowBackground = value;

                    RaisePropertyChangedEvent();
                }
            }
        }
        #endregion

        #region SelectedShape
        public OverlayShape SelectedShape
        {
            get => m_selectedShape;
            set
            {
                if (value != m_selectedShape)
                {
                    m_selectedShape = value;

                    RaisePropertyChangedEvent();
                }
            }
        }
        #endregion

        #region AccelerateBrush
        public SolidColorBrush AccelerateBrush
        {
            get => m_accelerateBrush;
            set
            {
                if (value != m_accelerateBrush)
                {
                    m_accelerateBrush = value;

                    RaisePropertyChangedEvent();
                }
            }
        }
        #endregion

        #region BrakeBrush
        public SolidColorBrush BrakeBrush
        {
            get => m_brakeBrush;
            set
            {
                if (value != m_brakeBrush)
                {
                    m_brakeBrush = value;

                    RaisePropertyChangedEvent();
                }
            }
        }
        #endregion

        #region NoDeviceLabelVisibility
        public Visibility NoDeviceLabelVisibility
        {
            get => m_noDeviceLabelVisibility;
            set
            {
                if (value != m_noDeviceLabelVisibility)
                {
                    m_noDeviceLabelVisibility = value;

                    RaisePropertyChangedEvent();
                }
            }
        }
        #endregion

        #region NoDeviceLabelBrush
        public SolidColorBrush NoDeviceLabelBrush
        {
            get => m_noDeviceLabelBrush;
            set
            {
                if (value != m_noDeviceLabelBrush)
                {
                    m_noDeviceLabelBrush = value;

                    RaisePropertyChangedEvent();
                }
            }
        }
        #endregion

        #region ToggleResizeGripCommand
        public ICommand ToggleResizeGripCommand => new RelayCommand(ToggleResizeGrip);
        #endregion

        #region ResizeMode
        public ResizeMode ResizeMode
        {
            get => m_resizeMode;
            set
            {
                if (value != m_resizeMode)
                {
                    m_resizeMode = value;

                    RaisePropertyChangedEvent();
                }
            }
        }
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
            ResizeMode = ResizeMode.CanResize;
        }
        #endregion

        #region ToggleResizeGrip
        private void ToggleResizeGrip()
        {
            switch (ResizeMode)
            {
                case ResizeMode.CanResize:
                    ResizeMode = ResizeMode.CanResizeWithGrip;
                    break;

                case ResizeMode.CanResizeWithGrip:
                    ResizeMode = ResizeMode.CanResize;
                    break;
            }
        }
        #endregion
    }
}