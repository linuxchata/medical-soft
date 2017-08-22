using Contracts.ViewModel.Dialogs;
using Models;

namespace Client.Contracts.Dialogs
{
    /// <summary>
    /// Represents view model interface for reminder dialog.
    /// </summary>
    public interface IReminderDialogViewModel : IReminderDialogViewModelBase<ReminderModel, ItemModel, ReminderAlertModel>
    {
    }
}