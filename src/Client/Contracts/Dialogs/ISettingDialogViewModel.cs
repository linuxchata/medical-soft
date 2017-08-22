using Contracts.ViewModel.Dialogs;
using Models;

namespace Client.Contracts.Dialogs
{
    /// <summary>
    /// Represents view model interface for about box dialog.
    /// </summary>
    public interface ISettingDialogViewModel : ISettingDialogViewModelBase<SettingCollectionModel, CultureModel>
    {
    }
}