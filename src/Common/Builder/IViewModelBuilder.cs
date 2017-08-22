using Contracts.ViewModel;

namespace Common.Builder
{
    /// <summary>
    /// Represent view model builder interface.
    /// </summary>
    public interface IViewModelBuilder
    {
        /// <summary>
        /// Build view model.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <returns>Returns view model instance.</returns>
        TViewModel Build<TViewModel>()
            where TViewModel : IRequestCloseViewModel;

        /// <summary>
        /// Build view model.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <param name="resolverParameters">List of the parameters.</param>
        /// <returns>Returns view model instance.</returns>
        TViewModel Build<TViewModel>(params ResolverParameter[] resolverParameters)
            where TViewModel : IRequestCloseViewModel;
    }
}