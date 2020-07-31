using NLog;
using System.Windows.Media;

namespace Spawn.InputOverlay.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
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
        #endregion

        #region Initialize
        protected override void Initialize()
        {
            WindowTitle = $"{AppName} v{App.AppVersion}";
            WindowBackgroundColor = Colors.Magenta;

            ResetSize();
        }
        #endregion

        #region ResetSize
        private void ResetSize()
        {
            //s_logger.Trace("Reset window size");

            WindowHeight = DefaultWindowHeight;
            WindowWidth = DefaultWindowWidth;
        }
        #endregion
    }
}