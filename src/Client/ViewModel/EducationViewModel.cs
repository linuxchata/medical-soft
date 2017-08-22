using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Cache.Interface;
using Client.Contracts;
using Client.Contracts.Dialogs;
using Client.Providers;
using Client.ViewModel.Dialogs;
using Client.Views.Dialogs;
using Common;
using Common.Builder;
using Common.Enumeration;
using Common.ViewModel;
using DataAccess;
using Logger;
using Models;

namespace Client.ViewModel
{
    /// <summary>
    /// Represents view model for education.
    /// </summary>
    public sealed class EducationViewModel : ViewModelMainBase<EducationModel>, IEducationViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IViewModelBuilder viewModelBuilder;

        private readonly IViewBuilder viewBuilder;

        private readonly IMessageBoxProvider messageBoxProvider;

        private readonly IEducationCache educationCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="EducationViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="viewModelBuilder">View model builder.</param>
        /// <param name="viewBuilder">View builder.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="educationCache">Education cache.</param>
        public EducationViewModel(
            IUnitOfWork unitOfWork,
            IViewModelBuilder viewModelBuilder,
            IViewBuilder viewBuilder,
            IMessageBoxProvider messageBoxProvider,
            IEducationCache educationCache)
        {
            this.unitOfWork = unitOfWork;
            this.viewModelBuilder = viewModelBuilder;
            this.viewBuilder = viewBuilder;
            this.messageBoxProvider = messageBoxProvider;
            this.educationCache = educationCache;

            Task.Factory.StartNewWithDefaultCulture(this.UpdateData);
        }

        /// <summary>
        /// Load information about education.
        /// </summary>
        public override void UpdateData()
        {
            this.Status = LoadingStatus.Loading;

            var educations = this.unitOfWork.EducationRepository.GetAllExceptDeleted();
            this.Model = new ObservableCollection<EducationModel>(educations);

            this.OnPropertyChanged(() => this.Count);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Add/Edit education dialog.
        /// </summary>
        /// <param name="mode">The mode (Add/Edit).</param>
        protected override void AddEditDialog(WorkModeType mode)
        {
            try
            {
                EducationDialogViewModel dialogViewModel;

                switch (mode)
                {
                    case WorkModeType.Add:
                        this.AddItem(out dialogViewModel, mode);
                        break;
                    case WorkModeType.Edit:
                        this.EditItem(out dialogViewModel, mode);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected mode type.");
                }

                if (this.IsDataAddedOrUpdated(dialogViewModel))
                {
                    this.educationCache.Clear();
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Delete education.
        /// </summary>
        protected override void Delete()
        {
            if (this.IsDeletionConfirmedByUser())
            {
                if (this.TryDeleteItemFromDatasource())
                {
                    this.DeleteItemFromCollection();

                    this.educationCache.Clear();
                }

                this.OnPropertyChanged(() => this.Count);
            }
        }

        private void AddItem(out EducationDialogViewModel dialogViewModel, WorkModeType mode)
        {
            dialogViewModel = this.viewModelBuilder.Build<EducationDialogViewModel>(
                new ResolverParameter(ParameterName.Mode, mode));

            this.viewBuilder.Build<EducationDialogView, IEducationDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Added)
            {
                this.Model.Insert(0, dialogViewModel.Model);
            }

            this.OnPropertyChanged(() => this.Count);
        }

        private void EditItem(out EducationDialogViewModel dialogViewModel, WorkModeType mode)
        {
            dialogViewModel = this.viewModelBuilder.Build<EducationDialogViewModel>(
                new ResolverParameter(ParameterName.Mode, mode),
                new ResolverParameter(ParameterName.Model, this.SelectedItem));

            this.viewBuilder.Build<EducationDialogView, IEducationDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Updated)
            {
                dialogViewModel.Model.Map(this.SelectedItem);
            }
        }

        private bool IsDataAddedOrUpdated(EducationDialogViewModel dialogViewModel)
        {
            return dialogViewModel.Status == LoadingStatus.Added || dialogViewModel.Status == LoadingStatus.Updated;
        }

        private bool IsDeletionConfirmedByUser()
        {
            return this.messageBoxProvider.ConfirmDelete() == MessageBoxResult.Yes;
        }

        private bool TryDeleteItemFromDatasource()
        {
            var isEducationHidden = this.unitOfWork.EducationRepository.TryHide(this.SelectedItem.Id);
            if (!isEducationHidden)
            {
                this.messageBoxProvider.CannotBeDeleted();
                return false;
            }

            var response = this.unitOfWork.Save();
            if (!response.IsSuccessful)
            {
                this.messageBoxProvider.CannotBeDeleted();
                return false;
            }

            return true;
        }

        private void DeleteItemFromCollection()
        {
            var itemToDelete = this.Model.FirstOrDefault(a => a.Id == this.SelectedItem.Id);
            if (itemToDelete != null)
            {
                this.Model.Remove(itemToDelete);
            }
        }
    }
}
