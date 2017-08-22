using Contracts.ViewModel.Dialogs;
using Models;

namespace Client.Contracts.Dialogs
{
    /// <summary>
    /// Represents view model interface for patient dialog.
    /// </summary>
    public interface IPatientDialogViewModel : IPatientDialogViewModelBase<PatientModel, GenderModel>
    {
    }
}