using System.Windows.Input;

namespace Contracts.ViewModel.Base
{
    /// <summary>
    /// Represents dialog base view model interface.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public interface IViewModelDialogBase<T> : IRequestCloseViewModel
    {
        /// <summary>
        /// Gets or sets model.
        /// </summary>
        T Model { get; set; }

        /// <summary>
        /// Gets cancel command.
        /// </summary>
        ICommand CancelCommand { get; }
    }
}
