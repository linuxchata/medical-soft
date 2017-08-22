using System.Collections.Generic;
using System.Windows.Input;
using Contracts.ViewModel.Base;

namespace Contracts.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model interface for notification list dialog.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public interface INotificationListManageDialogViewModelBase<T> : IViewModelDialogBase3<T>
    {
        /// <summary>
        /// Gets or sets list of all domains.
        /// </summary>
        List<string> Domains { get; set; }

        /// <summary>
        /// Gets or sets selected status.
        /// </summary>
        string SelectedDomain { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the all receivers selected/unselected.
        /// </summary>
        bool CheckAllReceiver { get; set; }

        /// <summary>
        /// Gets cancel selected domain command.
        /// </summary>
        ICommand SelectedDomainCancelCommand { get; }
    }
}
