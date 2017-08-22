using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Contracts;
using Client.Contracts.Dialogs;
using Client.Providers;
using Client.ViewModel.Dialogs;
using Client.Views.Dialogs;
using Common;
using Common.Builder;
using Common.Commands;
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
    /// Represents view model for staff.
    /// </summary>
    public sealed class StaffViewModel : ViewModelMainBase<StaffModel>, IStaffViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IEventAggregator eventAggregator;

        private readonly IViewModelBuilder viewModelBuilder;

        private readonly IViewBuilder viewBuilder;

        private readonly IMessageBoxProvider messageBoxProvider;

        private ObservableCollection<StaffModel> filteredModel;

        private bool isSearchByFirstChartsEnabled = true;

        private string searchExpression;

        private ICommand searchCancelCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="eventAggregator">Event aggregator.</param>
        /// <param name="viewModelBuilder">View model builder.</param>
        /// <param name="viewBuilder">View builder.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        public StaffViewModel(
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
        /// Gets or sets filtered model.
        /// </summary>
        public ObservableCollection<StaffModel> FilteredModel
        {
            get
            {
                return this.filteredModel;
            }

            set
            {
                this.filteredModel = value;
                this.OnPropertyChanged(() => this.FilteredModel);
            }
        }

        /// <summary>
        /// Gets count of items in the model.
        /// </summary>
        public override int Count
        {
            get
            {
                var count = 0;

                if (this.FilteredModel != null)
                {
                    count = this.FilteredModel.Count;
                }

                return count;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the staff is searched by first charts enabled.
        /// </summary>
        public bool IsSearchByFirstChartsEnabled
        {
            get
            {
                return this.isSearchByFirstChartsEnabled;
            }

            set
            {
                this.isSearchByFirstChartsEnabled = value;

                this.SearchStaff();

                this.OnPropertyChanged(() => this.IsSearchByFirstChartsEnabled);
            }
        }

        /// <summary>
        /// Gets or sets search expression.
        /// </summary>
        public string SearchExpression
        {
            get
            {
                return this.searchExpression;
            }

            set
            {
                this.searchExpression = value;

                // If search expression is empty clean search expression.
                if (this.searchExpression.IsNullOrEmpty())
                {
                    this.searchExpression = null;
                }

                this.SearchStaff();

                this.OnPropertyChanged(() => this.SearchExpression);
            }
        }

        /// <summary>
        /// Gets cancel search command.
        /// </summary>
        public ICommand SearchCancelCommand
        {
            get
            {
                return this.searchCancelCommand ?? (this.searchCancelCommand = new CommonCommand(
                    param => this.SearchCancel(),
                    param => this.SearchExpression != null));
            }
        }

        /// <summary>
        /// Load information about staff.
        /// </summary>
        public override void UpdateData()
        {
            this.Status = LoadingStatus.Loading;

            var staff = this.unitOfWork.StaffRepository.GetAllExceptDeleted();
            this.Model = new ObservableCollection<StaffModel>(staff);

            this.SearchStaff();

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Add/Edit staff dialog.
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

                this.SearchStaff();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Delete staff.
        /// </summary>
        protected override void Delete()
        {
            if (this.IsDeletionConfirmedByUser())
            {
                this.DeleteItemFromDatasource();
                this.DeleteItemFromCollection();

                this.eventAggregator.Publish<StaffDeletedEvent>();

                this.OnPropertyChanged(() => this.Count);
            }
        }

        private void SearchStaff()
        {
            if (this.SearchExpression.IsNullOrEmpty())
            {
                this.FilteredModel = this.Model;
            }
            else
            {
                var filteredStaff = !this.isSearchByFirstChartsEnabled ? this.ContainSearch() : this.StartWithSearch();
                this.FilteredModel = new ObservableCollection<StaffModel>(filteredStaff);
            }

            this.OnPropertyChanged(() => this.Count);
        }

        private void SearchCancel()
        {
            this.SearchExpression = null;
        }

        private IEnumerable<StaffModel> ContainSearch()
        {
            var q = from c in this.Model
                    where c.SurName.ContainsIgnoreCase(this.SearchExpression) ||
                          c.FirstName.ContainsIgnoreCase(this.SearchExpression) ||
                          c.MiddleName.ContainsIgnoreCase(this.SearchExpression)
                    select c;
            return q;
        }

        private IEnumerable<StaffModel> StartWithSearch()
        {
            var q = from c in this.Model
                    where c.SurName.StartsWithIgnoreCase(this.SearchExpression) ||
                          c.FirstName.StartsWithIgnoreCase(this.SearchExpression) ||
                          c.MiddleName.StartsWithIgnoreCase(this.SearchExpression)
                    select c;
            return q;
        }

        private void AddItem(WorkModeType mode)
        {
            var dialogViewModel = this.viewModelBuilder.Build<StaffDialogViewModel>(
                new ResolverParameter(ParameterName.Mode, mode));

            this.viewBuilder.Build<StaffDialogView, IStaffDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Added)
            {
                this.Model.Insert(0, dialogViewModel.Model);
                this.eventAggregator.Publish<StaffAddedEvent>();
            }
        }

        private void EditItem(WorkModeType mode)
        {
            var dialogViewModel = this.viewModelBuilder.Build<StaffDialogViewModel>(
                new ResolverParameter(ParameterName.Mode, mode),
                new ResolverParameter(ParameterName.Model, this.SelectedItem));

            this.viewBuilder.Build<StaffDialogView, IStaffDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Updated)
            {
                dialogViewModel.Model.Map(this.SelectedItem);
                this.eventAggregator.Publish<StaffChangedEvent>();
            }
        }

        private bool IsDeletionConfirmedByUser()
        {
            return this.messageBoxProvider.ConfirmDelete() == MessageBoxResult.Yes;
        }

        private void DeleteItemFromDatasource()
        {
            this.unitOfWork.StaffRepository.TryHide(this.SelectedItem.Id);
            this.unitOfWork.LoginRepository.DeactivateAndHideLoginsByStaffId(this.SelectedItem.Id);
            this.unitOfWork.Save();
        }

        private void DeleteItemFromCollection()
        {
            var itemToDelete = this.Model.FirstOrDefault(a => a.Id == this.SelectedItem.Id);
            if (itemToDelete != null)
            {
                this.Model.Remove(itemToDelete);
                this.FilteredModel.Remove(itemToDelete);
            }
        }
    }
}
