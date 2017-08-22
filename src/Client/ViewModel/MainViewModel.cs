using System;
using Client.Providers;
using Common.Authentication;
using Common.ViewModel;
using Contracts.ViewModel.UserControls;
using Logger;
using Microsoft.Practices.Unity;

namespace Client.ViewModel
{
    /// <summary>
    /// Represents view model for Main view.
    /// </summary>
    public sealed class MainViewModel : ViewModelNotifyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="authenticationSession">Unit of work.</param>
        /// <param name="startupServicesProvider">Startup services provider.</param>
        public MainViewModel(
            IAuthenticationSession authenticationSession,
            IStartupServicesProvider startupServicesProvider)
        {
            Log.Debug("Start resolving main view model.");

#if DEBUG
            authenticationSession.CreateSession("DEBUG USER", 1, true, DateTime.Now);
#endif

            startupServicesProvider.StartServices();

            Log.Debug("Main view model was initialized.");
        }

        /// <summary>
        /// Gets or sets navigations panel view model.
        /// </summary>
        [Dependency]
        public INavigationsPanelViewModel NavigationsPanelViewModel { get; set; }
    }
}
