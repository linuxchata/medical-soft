using Contracts.ViewModel;
using Microsoft.Practices.Unity;

namespace Common.Builder
{
    /// <summary>
    /// Represent view model builder.
    /// </summary>
    public class ViewModelBuilder : IViewModelBuilder
    {
        private readonly IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBuilder"/> class.
        /// </summary>
        /// <param name="container">Unity container.</param>
        public ViewModelBuilder(IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Build view model.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <returns>Returns view model instance.</returns>
        public TViewModel Build<TViewModel>()
            where TViewModel : IRequestCloseViewModel
        {
            return this.container.Resolve<TViewModel>();
        }

        /// <summary>
        /// Build view model.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <param name="resolverParameters">List of the parameters.</param>
        /// <returns>Returns view model instance.</returns>
        public TViewModel Build<TViewModel>(params ResolverParameter[] resolverParameters)
            where TViewModel : IRequestCloseViewModel
        {
            var @params = new ResolverOverride[resolverParameters.Length];
            for (var i = 0; i < resolverParameters.Length; i++)
            {
                var resolverParameter = resolverParameters[i];
                var parameter = new ParameterOverride(resolverParameter.ParameterName, resolverParameter.ParameterValue);
                @params[i] = parameter;
            }

            return this.container.Resolve<TViewModel>(@params);
        }
    }
}