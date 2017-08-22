using System;
using System.Reflection;
using Client.Contracts.Dialogs;
using Common.ViewModel;
using DataAccess;
using Models;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model for about dialog.
    /// </summary>
    public sealed class AboutDialogViewModel : ViewModelDialogBase<AboutModel>, IAboutDialogViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        public AboutDialogViewModel(IUnitOfWork unitOfWork)
        {
            var assemblyVersion = GetAssemblyVersion();
            var databaseVersion = GetDatabaseVersion(unitOfWork);

            this.Model = new AboutModel
            {
                ProductVersion = assemblyVersion.ToString(),
                DatabaseVersion = databaseVersion.UpdateVersion
            };
        }

        private static Version GetAssemblyVersion()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var executingAssemblyName = executingAssembly.GetName();
            return executingAssemblyName.Version;
        }

        private static SystemUpdatesModel GetDatabaseVersion(IUnitOfWork unitOfWork)
        {
            return unitOfWork.SystemUpdatesRepository.GetDatabaseVersion();
        }
    }
}
