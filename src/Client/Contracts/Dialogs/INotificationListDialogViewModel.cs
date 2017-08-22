using Contracts.ViewModel.Dialogs;
using Models;

namespace Client.Contracts.Dialogs
{
    /// <summary>
    /// Represents view model interface for notification list dialog.
    /// </summary>
    public interface INotificationListDialogViewModel : INotificationListDialogViewModelBase<NotificationListModel, NotificationListStatusModel>
    {
    }
}