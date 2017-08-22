using System.Collections.Generic;
using System.Windows.Input;
using Contracts.ViewModel.Base;

namespace Contracts.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model interface for notification list dialog.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    /// <typeparam name="TStatus">Common class that represents the status model.</typeparam>
    public interface INotificationListDialogViewModelBase<T, TStatus> : IViewModelDialogBase3<T>, ISubscribable
    {
        /// <summary>
        /// Gets list of all statuses.
        /// </summary>
        List<TStatus> StatusItems { get; }

        /// <summary>
        /// Gets or sets selected status.
        /// </summary>
        TStatus SelectedStatus { get; set; }

        /// <summary>
        /// Gets cancel selected status command.
        /// </summary>
        ICommand SelectedStatusCancelCommand { get; }
    }
}
