using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
using MessageBoxResult = Common.Enumeration.MessageBoxResult;

namespace Client.ViewModel
{
    /// <summary>
    /// Represents view model for reminder.
    /// </summary>
    public sealed class ReminderViewModel : ViewModelMainBase<ReminderModel>, IReminderViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IEventAggregator eventAggregator;

        private readonly IViewModelBuilder viewModelBuilder;

        private readonly IViewBuilder viewBuilder;

        private readonly IMessageBoxProvider messageBoxProvider;

        private ObservableCollection<ReminderModel> filteredModel;

        private List<ReminderFilterModel> filterItems;

        private ReminderFilterModel selectedFilter;

        private DateTime? selectedFilterDate;

        private Visibility isFilterDateVisiblee;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="eventAggregator">Event aggregator.</param>
        /// <param name="viewModelBuilder">View model builder.</param>
        /// <param name="viewBuilder">View builder.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        public ReminderViewModel(
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
        public ObservableCollection<ReminderModel> FilteredModel
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
        /// Gets items for reminder filter.
        /// </summary>
        public List<ReminderFilterModel> FilterItems
        {
            get
            {
                if (this.filterItems == null)
                {
                    this.filterItems = this.unitOfWork.ReminderFilterRepository.GetAll().ToList();

                    this.SelectedFilter = this.filterItems.Single(a => a.Id == (int)ReminderFilterType.SelectFilter);
                }

                return this.filterItems;
            }
        }

        /// <summary>
        /// Gets or sets selected item for reminder filter.
        /// </summary>
        public ReminderFilterModel SelectedFilter
        {
            get
            {
                return this.selectedFilter;
            }

            set
            {
                this.selectedFilter = value;

                this.SetFilterPropertiesForView();

                this.SearchReminder();

                this.OnPropertyChanged(() => this.SelectedFilter);
            }
        }

        /// <summary>
        /// Gets or sets selected filter date.
        /// </summary>
        public DateTime? SelectedFilterDate
        {
            get
            {
                return this.selectedFilterDate;
            }

            set
            {
                this.selectedFilterDate = value;

                this.SearchReminder();

                this.OnPropertyChanged(() => this.SelectedFilterDate);
            }
        }

        /// <summary>
        /// Gets or sets filter date picker visibility.
        /// </summary>
        public Visibility IsFilterDateVisible
        {
            get
            {
                return this.isFilterDateVisiblee;
            }

            set
            {
                this.isFilterDateVisiblee = value;
                this.OnPropertyChanged(() => this.IsFilterDateVisible);
            }
        }

        /// <summary>
        /// Load information about reminder.
        /// </summary>
        public override void UpdateData()
        {
            this.Status = LoadingStatus.Loading;

            var reminders = this.unitOfWork.ReminderRepository.GetAllExceptDeleted();
            this.Model = new ObservableCollection<ReminderModel>(reminders);
            this.FilteredModel = this.Model;

            if (this.SelectedItem != null)
            {
                this.SelectedItem = null;
            }

            this.OnPropertyChanged(() => this.Count);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Subscribe method.
        /// </summary>
        public override void Subscribe()
        {
            base.Subscribe();
            this.eventAggregator.Subscribe<ReminderChangedEvent>(this.UpdateDataAsync);
            this.eventAggregator.Subscribe<PatientChangedEvent>(this.UpdateDataAsync);
            this.eventAggregator.Subscribe<StaffChangedEvent>(this.UpdateDataAsync);
            this.eventAggregator.Subscribe<StaffDeletedEvent>(this.UpdateDataAsync);
        }

        /// <summary>
        /// Unsubscribe method.
        /// </summary>
        public override void Unsubscribe()
        {
            base.Unsubscribe();
            this.eventAggregator.Unsubscribe<ReminderChangedEvent>();
            this.eventAggregator.Unsubscribe<PatientChangedEvent>();
            this.eventAggregator.Unsubscribe<StaffChangedEvent>();
            this.eventAggregator.Unsubscribe<StaffDeletedEvent>();
        }

        /// <summary>
        /// Add/Edit reminder dialog.
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

                this.SearchReminder();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Delete reminder.
        /// </summary>
        protected override void Delete()
        {
            if (this.IsDeletionConfirmedByUser())
            {
                this.DeleteItemFromDatasource();
                this.DeleteItemFromCollection();

                this.OnPropertyChanged(() => this.Count);
            }
        }

        private void SetFilterPropertiesForView()
        {
            if (this.selectedFilter.Id == (int)ReminderFilterType.SelectedDate)
            {
                this.SelectedFilterDate = DateTime.Now;
                this.IsFilterDateVisible = Visibility.Visible;
            }
            else
            {
                this.SelectedFilterDate = null;
                this.IsFilterDateVisible = Visibility.Collapsed;
            }
        }

        private void SearchReminder()
        {
            var reminderFilterType = this.GetReminderFilterTypeIfSelected();
            if (reminderFilterType.HasValue)
            {
                IEnumerable<ReminderModel> filteredReminders;

                switch (reminderFilterType.Value)
                {
                    case ReminderFilterType.CurrentYear:
                        filteredReminders = this.FilterByCurrentYear();
                        break;
                    case ReminderFilterType.CurrentMonth:
                        filteredReminders = this.FilterByCurrentMonth();
                        break;
                    case ReminderFilterType.CurrentWeek:
                        filteredReminders = this.FilterByCurrentWeek();
                        break;
                    case ReminderFilterType.SelectedDate:
                        filteredReminders = this.FilterBySelectedDate();
                        break;
                    default:
                        filteredReminders = this.Model;
                        break;
                }

                if (filteredReminders != null)
                {
                    this.FilteredModel = new ObservableCollection<ReminderModel>(filteredReminders);
                }

                this.OnPropertyChanged(() => this.Count);
            }
        }

        private ReminderFilterType? GetReminderFilterTypeIfSelected()
        {
            var reminderFilterType = this.selectedFilter != null ? this.SelectedFilter.Id : (int?)null;
            var isFilterSet = reminderFilterType.HasValue && Enum.IsDefined(typeof(ReminderFilterType), reminderFilterType);

            if (isFilterSet)
            {
                return (ReminderFilterType)reminderFilterType.Value;
            }

            return null;
        }

        private IEnumerable<ReminderModel> FilterByCurrentYear()
        {
            var startDate = new DateTime(DateTime.Now.Year, 1, 1);

            var endDate = new DateTime(DateTime.Now.Year, 12, 31);

            return this.Model.Where(a => a.Date.Date >= startDate.Date && a.Date.Date <= endDate.Date);
        }

        private IEnumerable<ReminderModel> FilterByCurrentMonth()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, daysInMonth);

            return this.Model.Where(a => a.Date.Date >= startDate.Date && a.Date.Date <= endDate.Date);
        }

        private IEnumerable<ReminderModel> FilterByCurrentWeek()
        {
            var firstDayOfWeek = DateTime.Now.GetFirstDayOfWeek();

            var lastDayOfWeek = DateTime.Now.GetLastDayOfWeek();

            return this.Model.Where(a => a.Date.Date >= firstDayOfWeek.Date && a.Date.Date <= lastDayOfWeek.Date);
        }

        private IEnumerable<ReminderModel> FilterBySelectedDate()
        {
            if (this.SelectedFilterDate.HasValue)
            {
                return this.Model.Where(a => a.Date.Date == this.SelectedFilterDate.Value.Date);
            }

            return this.Model;
        }

        private void AddItem(WorkModeType mode)
        {
            var dialogViewModel = this.viewModelBuilder.Build<ReminderDialogViewModel>(
                new ResolverParameter(ParameterName.Mode, mode));

            this.viewBuilder.Build<ReminderDialogView, IReminderDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Added)
            {
                this.Model.Insert(0, dialogViewModel.Model);
            }
        }

        private void EditItem(WorkModeType mode)
        {
            var dialogViewModel = this.viewModelBuilder.Build<ReminderDialogViewModel>(
                new ResolverParameter(ParameterName.Mode, mode),
                new ResolverParameter(ParameterName.Model, this.SelectedItem));

            this.viewBuilder.Build<ReminderDialogView, IReminderDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Updated)
            {
                dialogViewModel.Model.Map(this.SelectedItem);
            }
        }

        private bool IsDeletionConfirmedByUser()
        {
            return this.messageBoxProvider.ConfirmDelete() == MessageBoxResult.Yes;
        }

        private void DeleteItemFromDatasource()
        {
            this.unitOfWork.ReminderRepository.TryHide(this.SelectedItem.Id);
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
