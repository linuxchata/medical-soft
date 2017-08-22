using System;
using Contracts;
using Contracts.ViewModel;
using Logger;
using Microsoft.Practices.Unity;

namespace Common.Builder
{
    /// <summary>
    /// Represent view builder.
    /// </summary>
    public class ViewBuilder : IViewBuilder
    {
        private readonly IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewBuilder"/> class.
        /// </summary>
        public ViewBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewBuilder"/> class.
        /// </summary>
        /// <param name="container">Unity container.</param>
        public ViewBuilder(IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Build view.
        /// </summary>
        /// <typeparam name="TView">Type of the view.</typeparam>
        /// <returns>Returns build view.</returns>
        public TView Build<TView>()
        {
            try
            {
                return this.container.Resolve<TView>();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw;
            }
        }

        /// <summary>
        /// Build view.
        /// </summary>
        /// <typeparam name="TView">Type of the view.</typeparam>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <param name="viewModel">Instance of the view model.</param>
        /// <returns>Returns built view.</returns>
        public TView Build<TView, TViewModel>(TViewModel viewModel)
            where TView : IWindow, new()
            where TViewModel : IRequestCloseViewModel
        {
            try
            {
                var view = new TView();
                viewModel.RequestClose += (s, e) => view.Close();
                view.DataContext = viewModel;
                return view;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw;
            }
        }

        /// <summary>
        /// Build view.
        /// </summary>
        /// <typeparam name="TView">Type of the view.</typeparam>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <returns>Returns built view.</returns>
        public TView Build<TView, TViewModel>()
            where TView : IWindow, new()
            where TViewModel : class, IRequestCloseViewModel
        {
            try
            {
                // Build via view model builder
                var viewModel = this.container.Resolve<TViewModel>();
                var view = new TView();
                if (viewModel != null)
                {
                    viewModel.RequestClose += (s, e) => view.Close();
                    view.DataContext = viewModel;
                }

                return view;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw;
            }
        }
    }
}
