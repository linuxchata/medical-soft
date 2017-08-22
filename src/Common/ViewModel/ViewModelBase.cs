using System;
using Contracts.ViewModel.Base;

namespace Common.ViewModel
{
    /// <summary>
    /// Base class for view model.
    /// </summary>
    public abstract class ViewModelBase : ViewModelNotifyBase, IViewModelBase
    {
        /// <summary>
        /// Close event.
        /// </summary>
        public event EventHandler RequestClose;

        /// <summary>
        /// Close window.
        /// </summary>
        protected virtual void OnRequestClose()
        {
            if (this.RequestClose != null)
            {
                this.RequestClose(this, null);
            }
        }
    }
}
