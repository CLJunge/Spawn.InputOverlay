namespace Spawn.InputOverlay.UI.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        #region Member Variables
        private string m_strWindowTitle;
        #endregion

        #region Properties
        #region WindowTitle
        public string WindowTitle
        {
            get => m_strWindowTitle;
            set => Set(ref m_strWindowTitle, value);
        }
        #endregion
        #endregion

        #region MyRegion
        public AboutViewModel() => Initialize();
        #endregion

        #region Initialize
        private void Initialize()
        {
            WindowTitle = $"{App.AppName} - About";
        }
        #endregion
    }
}