using System.Collections.Generic;
using Contracts.ViewModel.Base;

namespace Contracts.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model interface for patient dialog.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    /// <typeparam name="T2">Common class that represents the sex model.</typeparam>
    public interface IPatientDialogViewModelBase<T, T2> : IViewModelDialogBase2<T>
    {
        /// <summary>
        /// Gets all sex.
        /// </summary>
        List<T2> Genders { get; }
    }
}
