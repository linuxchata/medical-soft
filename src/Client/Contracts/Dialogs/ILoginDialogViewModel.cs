using Contracts.ViewModel.Dialogs;
using Models;

namespace Client.Contracts.Dialogs
{
    /// <summary>
    /// Represents view model interface for login dialog.
    /// </summary>
    public interface ILoginDialogViewModel : ILoginDialogViewModelBase<LoginModel, RoleModel, StaffModel>
    {
    }
}