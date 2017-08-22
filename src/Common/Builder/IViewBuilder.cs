using Contracts;
using Contracts.ViewModel;

namespace Common.Builder
{
    /// <summary>
    /// Represent view builder interface.
    /// </summary>
    public interface IViewBuilder
    {
        /// <summary>
        /// Build view.
        /// </summary>
        /// <typeparam name="TView">Type of the view.</typeparam>
        /// <returns>Returns build view.</returns>
        TView Build<TView>();

        /// <summary>
        /// Build view.
        /// </summary>
        /// <typeparam name="TView">Type of the view.</typeparam>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <param name="viewModel">Instance of the view model.</param>
        /// <returns>Returns built view.</returns>
        TView Build<TView, TViewModel>(TViewModel viewModel)
            where TView : IWindow, new()
            where TViewModel : IRequestCloseViewModel;

        /// <summary>
        /// Build view.
        /// </summary>
        /// <typeparam name="TView">Type of the view.</typeparam>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <returns>Returns built view.</returns>
        TView Build<TView, TViewModel>()
            where TView : IWindow, new()
            where TViewModel : class, IRequestCloseViewModel;
    }
}
