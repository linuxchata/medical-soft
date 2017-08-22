using System.Windows.Input;

namespace Contracts.ViewModel.Base
{
    /// <summary>
    /// Represents dialog base view model.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public interface IViewModelDialogBase2<T> : IViewModelDialogBase<T>
    {
        /// <summary>
        /// Gets handle command.
        /// </summary>
        ICommand HandleCommand { get; }
    }
}
