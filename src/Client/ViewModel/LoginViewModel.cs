using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Contracts;
using Client.Contracts.Dialogs;
using Client.Providers;
using Client.ViewModel.Dialogs;
using Client.Views.Dialogs;
using Common;
using Common.Builder;
using Common.Enumeration;
using Common.Events;
using Common.ViewModel;
using DataAccess;
using Logger;
using Models;
using Utilities.EventAggregator;

namespace Client.ViewModel
{
    /// <summary>
    /// Represents view model for login.
    /// </summary>
    public sealed class LoginViewModel : ViewModelMainBase<LoginModel>, ILoginViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IEventAggregator eventAggregator;

        private readonly IViewModelBuilder viewModelBuilder;

        private readonly IViewBuilder viewBuilder;

        private readonly IMessageBoxProvider messageBoxProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="eventAggregator">Event aggregator.</param>
        /// <param name="viewModelBuilder">View model builder.</param>
        /// <param name="viewBuilder">View builder.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        public LoginViewModel(
            IUnitOfWork unitOfWork,
            IEventAggregator eventAggregator,
            IViewModelBuilder viewModelBuilder,
            IViewBuilder viewBuilder,
            IMessageBoxProvider messageBoxProvider)
        {
            this.unitOfWork = unitOfWork;
            this.eventAggregator = eventAggregator;
            this.viewModelBuilder = viewModelBuilder;
            this.viewBuilder = viewBuilder;
            this.messageBoxProvider = messageBoxProvider;

            Task.Factory.StartNewWithDefaultCulture(this.UpdateData);
        }

        /// <summary>
        /// Load information about education.
        /// </summary>
        public override void UpdateData()
        {
            this.Status = LoadingStatus.Loading;

            var logins = this.unitOfWork.LoginRepository.GetAllExceptDeleted();
            this.Model = new ObservableCollection<LoginModel>(logins);

            this.OnPropertyChanged(() => this.Count);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Subscribe method.
        /// </summary>
        public override void Subscribe()
        {
            base.Subscribe();
            this.eventAggregator.Subscribe<StaffChangedEvent>(this.UpdateDataAsync);
            this.eventAggregator.Subscribe<StaffDeletedEvent>(this.UpdateDataAsync);
        }

        /// <summary>
        /// Unsubscribe method.
        /// </summary>
        public override void Unsubscribe()
        {
            base.Unsubscribe();
            this.eventAggregator.Unsubscribe<StaffChangedEvent>();
            this.eventAggregator.Unsubscribe<StaffDeletedEvent>();
        }

        /// <summary>
        /// Add/Edit login dialog.
        /// </summary>
        /// <param name="mode">The mode (Add/Edit).</param>
        protected override void AddEditDialog(WorkModeType mode)
        {
            try
            {
                switch (mode)
                {
                    case WorkModeType.Add:
                        this.AddItem(mode);
                        break;
                    case WorkModeType.Edit:
                        this.EditItem(mode);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected mode type.");
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Delete login.
        /// </summary>
        protected override void Delete()
        {
            if (this.IsDeletionConfirmedByUser())
            {
                if (this.DeleteItemFromDatasource())
                {
                    this.DeleteItemFromCollection();
                }

                this.OnPropertyChanged(() => this.Count);
            }
        }

        private void AddItem(WorkModeType mode)
        {
            var dialogViewModel = this.viewModelBuilder.Build<LoginDialogViewModel>(
                new ResolverParameter(ParameterName.Mode, mode));

            this.viewBuilder.Build<LoginDialogView, ILoginDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Added)
            {
                this.Model.Insert(0, dialogViewModel.Model);
            }

            this.OnPropertyChanged(() => this.Count);
        }

        private void EditItem(WorkModeType mode)
        {
            var dialogViewModel = this.viewModelBuilder.Build<LoginDialogViewModel>(
                new ResolverParameter(ParameterName.Mode, mode),
                new ResolverParameter(ParameterName.Model, this.SelectedItem));

            this.viewBuilder.Build<LoginDialogView, ILoginDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Updated)
            {
                dialogViewModel.Model.Map(this.SelectedItem);
            }
        }

        private bool IsDeletionConfirmedByUser()
        {
            return this.messageBoxProvider.ConfirmDelete() == MessageBoxResult.Yes;
        }

        private bool DeleteItemFromDatasource()
        {
            this.unitOfWork.LoginRepository.TryHide(this.SelectedItem.Id);
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
