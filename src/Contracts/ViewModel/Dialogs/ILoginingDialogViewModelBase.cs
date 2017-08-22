using Contracts.ViewModel.Base;

namespace Contracts.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model interface for login dialog.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public interface ILoginingDialogViewModelBase<T> : IViewModelDialogBase2<T>
    {
        /// <summary>
        /// Gets or sets login message.
        /// </summary>
        string LoginMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the login is enabled.
        /// </summary>
        bool IsLoginEnabled { get; set; }
    }
}
