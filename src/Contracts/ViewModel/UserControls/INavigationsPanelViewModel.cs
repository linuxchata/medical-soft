using Utilities;

namespace Contracts.ViewModel.UserControls
{
    /// <summary>
    /// Represents view model for navigations panel.
    /// </summary>
    public interface INavigationsPanelViewModel
    {
        /// <summary>
        /// Gets or sets current view model.
        /// </summary>
        WeakReference<object> CurrentViewModel { get; set; }
    }
}
