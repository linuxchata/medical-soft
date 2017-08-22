using System.Collections.ObjectModel;

namespace Contracts.ViewModel.Base
{
    /// <summary>
    /// Represents dialog base view model interface.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public interface IViewModelDialogBase3<T> : IViewModelDialogBase2<T>
    {
        /// <summary>
        /// Gets or sets list model.
        /// </summary>
        ObservableCollection<T> ListModel { get; set; }

        /// <summary>
        /// Gets or sets filtered list model.
        /// </summary>
        ObservableCollection<T> FilteredModel { get; set; }
    }
}
