namespace Spawn.InputOverlay.UI.ViewModels
{
    public class UpdateViewModel : ViewModelBase
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

        #region Ctor
        public UpdateViewModel() => Intialize();
        #endregion

        #region Intialize
        private void Intialize()
        {
            WindowTitle = $"{App.AppName} - New version available!";
        }
        #endregion
    }
}