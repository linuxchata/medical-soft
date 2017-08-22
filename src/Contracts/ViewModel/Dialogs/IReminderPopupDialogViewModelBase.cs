using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using Contracts.ViewModel.Base;

namespace Contracts.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model interface for reminder popup dialog.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public interface IReminderPopupDialogViewModelBase<T> : IViewModelDialogBase<T>
    {
        /// <summary>
        /// Gets or sets reminder's model.
        /// </summary>
        List<T> Reminders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the next reminder visible.
        /// </summary>
        bool IsNextReminderExists { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the previous reminder visible.
        /// </summary>
        bool IsPreviousReminderExists { get; set; }

        /// <summary>
        /// Gets next reminder command.
        /// </summary>
        ICommand NextReminderCommand { get; }

        /// <summary>
        /// Gets previous reminder command.
        /// </summary>
        ICommand PreviousReminderCommand { get; }

        /// <summary>
        /// Gets complete reminder command.
        /// </summary>
        ICommand CompleteReminderCommand { get; }

        /// <summary>
        /// Check for active notifications.
        /// </summary>
        /// <returns>Returns is active notifications exist.</returns>
        bool CheckForActiveNotifications();

        /// <summary>
        /// Set ManualResetEvent object.
        /// </summary>
        /// <param name="resetEvent">ManualResetEvent object.</param>
        void SetManualResetEvent(ManualResetEvent resetEvent);
    }
}
