using Contracts.ViewModel.Dialogs;
using Models;

namespace Client.Contracts.Dialogs
{
    /// <summary>
    /// Represents view model interface for notification group dialog.
    /// </summary>
    public interface INotificationGroupDialogViewModel : INotificationGroupDialogViewModelBase<NotificationGroupModel, ItemModel>
    {
    }
}