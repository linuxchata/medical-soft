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
    /// Represents view model for position.
    /// </summary>
    public sealed class PositionViewModel : ViewModelMainBase<PositionModel>, IPositionViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IViewModelBuilder viewModelBuilder;

        private readonly IViewBuilder viewBuilder;

        private readonly IMessageBoxProvider messageBoxProvider;

        private readonly IPositionCache positionCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="viewModelBuilder">View model builder.</param>
        /// <param name="viewBuilder">View builder.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="positionCache">Position cache.</param>
        public PositionViewModel(
            IUnitOfWork unitOfWork,
            IViewModelBuilder viewModelBuilder,
            IViewBuilder viewBuilder,
            IMessageBoxProvider messageBoxProvider,
            IPositionCache positionCache)
        {
            this.unitOfWork = unitOfWork;
            this.viewModelBuilder = viewModelBuilder;
            this.viewBuilder = viewBuilder;
            this.messageBoxProvider = messageBoxProvider;
            this.positionCache = positionCache;

            Task.Factory.StartNewWithDefaultCulture(this.UpdateData);
        }

        /// <summary>
        /// Load information about position.
        /// </summary>
        public override void UpdateData()
        {
            this.Status = LoadingStatus.Loading;

            var positions = this.unitOfWork.PositionRepository.GetAllExceptDeleted();
            this.Model = new ObservableCollection<PositionModel>(positions);

            this.OnPropertyChanged(() => this.Count);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Add/Edit position dialog.
        /// </summary>
        /// <param name="mode">The mode (Add/Edit).</param>
        protected override void AddEditDialog(WorkModeType mode)
        {
            try
            {
                PositionDialogViewModel dialogViewModel;

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
                    this.positionCache.Clear();
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Delete position.
        /// </summary>
        protected override void Delete()
        {
            if (this.IsDeletionConfirmedByUser())
            {
                if (this.TryDeleteItemFromDatasource())
                {
                    this.DeleteItemFromCollection();
                }

                this.OnPropertyChanged(() => this.Count);
            }
        }

        private void AddItem(out PositionDialogViewModel dialogViewModel, WorkModeType mode)
        {
            dialogViewModel = this.viewModelBuilder.Build<PositionDialogViewModel>(new ResolverParameter(ParameterName.Mode, mode));

            this.viewBuilder.Build<PositionDialogView, IPositionDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Added)
            {
                this.Model.Insert(0, dialogViewModel.Model);
            }

            this.OnPropertyChanged(() => this.Count);
        }

        private void EditItem(out PositionDialogViewModel dialogViewModel, WorkModeType mode)
        {
            dialogViewModel = this.viewModelBuilder.Build<PositionDialogViewModel>(
                new ResolverParameter(ParameterName.Mode, mode),
                new ResolverParameter(ParameterName.Model, this.SelectedItem));

            this.viewBuilder.Build<PositionDialogView, IPositionDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Updated)
            {
                dialogViewModel.Model.Map(this.SelectedItem);
            }
        }

        private bool IsDataAddedOrUpdated(PositionDialogViewModel dialodViewModel)
        {
            return dialodViewModel.Status == LoadingStatus.Added || dialodViewModel.Status == LoadingStatus.Updated;
        }

        private bool IsDeletionConfirmedByUser()
        {
            return this.messageBoxProvider.ConfirmDelete() == MessageBoxResult.Yes;
        }

        private bool TryDeleteItemFromDatasource()
        {
            var result = true;

            var isPositionHidden = this.unitOfWork.PositionRepository.TryHide(this.SelectedItem.Id);
            if (!isPositionHidden)
            {
                this.messageBoxProvider.CannotBeDeleted();
                result = false;
            }

            var response = this.unitOfWork.Save();
            if (!response.IsSuccessful)
            {
                this.messageBoxProvider.CannotBeDeleted();
                result = false;
            }

            return result;
        }

        private void DeleteItemFromCollection()
        {
            var itemToDelete = this.Model.FirstOrDefault(a => a.Id == this.SelectedItem.Id);
            if (itemToDelete != null)
            {
                this.Model.Remove(itemToDelete);
            }

            this.positionCache.Clear();
        }
    }
}
