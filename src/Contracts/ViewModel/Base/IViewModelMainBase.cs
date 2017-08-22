using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Contracts.ViewModel.Base
{
    /// <summary>
    /// Represents base main view model interface.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public interface IViewModelMainBase<T> : ISubscribable
    {
        /// <summary>
        /// Gets or sets model for T.
        /// </summary>
        ObservableCollection<T> Model { get; set; }

        /// <summary>
        /// Gets or sets currently selected item of T.
        /// </summary>
        T SelectedItem { get; set; }

        /// <summary>
        /// Gets add dialog command.
        /// </summary>
        ICommand AddDialogCommand { get; }

        /// <summary>
        /// Gets edit dialog command.
        /// </summary>
        ICommand EditDialogCommand { get; }

        /// <summary>
        /// Gets delete command.
        /// </summary>
        ICommand DeleteCommand { get; }

        /// <summary>
        /// Load information for model.
        /// </summary>
        void UpdateData();

        /// <summary>
        /// Load information for model.
        /// </summary>
        void UpdateDataAsync();
    }
}
