using System.Collections.Generic;
using System.Windows.Input;
using Contracts.ViewModel.Base;

namespace Contracts.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model interface for notification group dialog.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    /// <typeparam name="TItem">Common class that represents the item model.</typeparam>
    public interface INotificationGroupDialogViewModelBase<T, TItem> : IViewModelDialogBase2<T>
    {
        /// <summary>
        /// Gets list of all templates.
        /// </summary>
        List<TItem> Templates { get; }

        /// <summary>
        /// Gets or sets selected hours.
        /// </summary>
        string SelectedHour { get; set; }

        /// <summary>
        /// Gets or sets selected minutes.
        /// </summary>
        string SelectedMinute { get; set; }

        /// <summary>
        /// Gets select notification list command.
        /// </summary>
        ICommand SelectListCommand { get; }

        /// <summary>
        /// Gets select notification manage list command.
        /// </summary>
        ICommand SelectManageListCommand { get; }

        /// <summary>
        /// Gets cancel delivery command.
        /// </summary>
        ICommand CancelDeliveryCommand { get; }

        /// <summary>
        /// Gets restart delivery command.
        /// </summary>
        ICommand RestartDeliveryCommand { get; }
    }
}
