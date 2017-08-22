using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Contracts.Dialogs;
using Common.Commands;
using Common.Enumeration;
using Common.Events;
using Common.ViewModel;
using DataAccess;
using Models;
using Utilities.EventAggregator;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model for notification list view.
    /// </summary>
    public sealed class NotificationListDialogViewModel :
        ViewModelDialogBase3<NotificationListModel>,
        INotificationListDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IEventAggregator eventAggregator;

        private List<NotificationListStatusModel> statusItems;

        private NotificationListStatusModel selectedStatus;

        private ICommand selectedStatusCancelCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationListDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="eventAggregator">Event aggregator.</param>
        /// <param name="id">Id of the record.</param>
        public NotificationListDialogViewModel(
            IUnitOfWork unitOfWork,
            IEventAggregator eventAggregator,
            int id = 0)
        {
            this.unitOfWork = unitOfWork;
            this.eventAggregator = eventAggregator;

            this.Id = id;

            this.Subscribe();

            Task.Factory.StartNewWithDefaultCulture(this.Load);
        }

        /// <summary>
        /// Gets or sets list of all statuses.
        /// </summary>
        public List<NotificationListStatusModel> StatusItems
        {
            get
            {
                return this.statusItems;
            }

            set
            {
                this.statusItems = value;
                this.OnPropertyChanged(() => this.StatusItems);
            }
        }

        /// <summary>
        /// Gets or sets selected status.
        /// </summary>
        public NotificationListStatusModel SelectedStatus
        {
            get
            {
                return this.selectedStatus;
            }

            set
            {
                this.selectedStatus = value;

                this.Filter();

                this.OnPropertyChanged(() => this.SelectedStatus);
                this.OnPropertyChanged(() => this.Count);
            }
        }

        /// <summary>
        /// Gets count of items in the filtered model.
        /// </summary>
        public int Count
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
        /// Gets cancel selected status command.
        /// </summary>
        public ICommand SelectedStatusCancelCommand
        {
            get
            {
                return this.selectedStatusCancelCommand ?? (this.selectedStatusCancelCommand = new CommonCommand(
                    param => { this.SelectedStatus = null; },
                    param => this.SelectedStatus != null));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the model is valid.
        /// </summary>
        protected override bool CanHandle
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Subscribe method.
        /// </summary>
        public void Subscribe()
        {
            this.eventAggregator.Subscribe<NotificationListChangedEvent>(this.Load);
        }

        /// <summary>
        /// Unsubscribe method.
        /// </summary>
        public void Unsubscribe()
        {
            this.eventAggregator.Unsubscribe<NotificationListChangedEvent>();
        }

        /// <summary>
        /// Load information about notification list.
        /// </summary>
        protected override void Load()
        {
            this.Status = LoadingStatus.Loading;

            this.StatusItems = this.unitOfWork.NotificationListStatusRepository.GetAll().ToList();

            var notifications = this.unitOfWork.NotificationListRepository.GetAllExceptDeletedByGroupId(this.Id);
            this.ListModel = new ObservableCollection<NotificationListModel>(notifications);
            this.FilteredModel = this.ListModel;

            this.Filter();

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Load information about notification list.
        /// </summary>
        protected override void Handle()
        {
            this.Load();
        }

        /// <summary>
        /// Close window.
        /// </summary>
        protected override void OnRequestClose()
        {
            this.Unsubscribe();

            base.OnRequestClose();
        }

        private void Filter()
        {
            if (this.SelectedStatus != null)
            {
                var list = this.ListModel.Where(a => a.Status == this.SelectedStatus.Id);
                this.FilteredModel = new ObservableCollection<NotificationListModel>(list);
            }
            else
            {
                this.FilteredModel = new ObservableCollection<NotificationListModel>(this.ListModel);
            }

            this.OnPropertyChanged(() => this.Count);
        }
    }
}
