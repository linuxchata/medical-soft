using System.Collections.Generic;
using Contracts.ViewModel.Base;

namespace Contracts.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model interface for login dialog.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    /// <typeparam name="T2">Common class that represents the role model.</typeparam>
    /// <typeparam name="T3">Common class that represents the staff model.</typeparam>
    public interface ILoginDialogViewModelBase<T, T2, T3> : IViewModelDialogBase2<T>
        where T : class
        where T2 : class
        where T3 : class
    {
        /// <summary>
        /// Gets all roles.
        /// </summary>
        List<T2> Roles { get; }

        /// <summary>
        /// Gets or sets currently selected role.
        /// </summary>
        T2 SelectedRole { get; set; }

        /// <summary>
        /// Gets all staff.
        /// </summary>
        List<T3> Staff { get; }

        /// <summary>
        /// Gets or sets currently selected staff.
        /// </summary>
        T3 SelectedStaff { get; set; }
    }
}
