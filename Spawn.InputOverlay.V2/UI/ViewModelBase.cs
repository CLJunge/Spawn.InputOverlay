#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
#endregion

namespace Spawn.InputOverlay.UI
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        #region EventHandler
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent([CallerMemberName] string strPropertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        #endregion

        #region Set
        protected void Set<T>(ref T field, T newValue, [CallerMemberName] string strPropertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;

                RaisePropertyChangedEvent(strPropertyName);
            }
        }
        #endregion

        #region Dispose
        public virtual void Dispose()
        {
        }
        #endregion
    }
}