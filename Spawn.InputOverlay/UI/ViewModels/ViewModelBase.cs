#region Using
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
#endregion

namespace Spawn.InputOverlay.UI.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region EventHandler
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChangedEvent([CallerMemberName]string strPropertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        #endregion

        #region Set
        protected void Set<T>(ref T field, T newValue, [CallerMemberName]string strPropertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;

                RaisePropertyChangedEvent(strPropertyName);
            }
        }
        #endregion
    }
}