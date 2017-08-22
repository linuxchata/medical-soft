using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Client.Contracts.Dialogs;
using Client.Providers;
using Common.Commands;
using Common.Enumeration;
using Common.ViewModel;
using DataAccess;
using Models;
using Models.Enumeration;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model for notification list manage dialog.
    /// </summary>
    public sealed class NotificationListManageDialogViewModel :
        ViewModelDialogBase3<PatientModel>,
        INotificationListManageDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMessageBoxProvider messageBoxProvider;

        private List<NotificationListModel> notifications;

        private List<string> domains;

        private string selectedDomain;

        private bool checkAllReceivers;

        private ICommand selectedDomainCancelCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationListManageDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="id">Identification of the notification group.</param> 
        public NotificationListManageDialogViewModel(
            IUnitOfWork unitOfWork,
            IMessageBoxProvider messageBoxProvider,
            int id = 0)
        {
            this.unitOfWork = unitOfWork;
            this.messageBoxProvider = messageBoxProvider;

            this.Id = id;

            this.notifications = new List<NotificationListModel>();
            this.domains = new List<string>();

            Task.Factory.StartNewWithDefaultCulture(this.Load);
        }

        /// <summary>
        /// Gets count of items in the model for receiver.
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
        /// Gets or sets list of all domains.
        /// </summary>
        public List<string> Domains
        {
            get
            {
                return this.domains;
            }

            set
            {
                this.domains = value;
                this.OnPropertyChanged(() => this.Domains);
            }
        }

        /// <summary>
        /// Gets or sets selected status.
        /// </summary>
        public string SelectedDomain
        {
            get
            {
                return this.selectedDomain;
            }

            set
            {
                this.selectedDomain = value;

                this.FilterReceiversBySelectedDomainIfAny();

                this.UpdateAllReceiversSelection();

                this.OnPropertyChanged(() => this.SelectedDomain);
                this.OnPropertyChanged(() => this.CheckAllReceiver);
                this.OnPropertyChanged(() => this.Count);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the all receivers selected/deselected.
        /// </summary>
        public bool CheckAllReceiver
        {
            get
            {
                return this.checkAllReceivers;
            }

            set
            {
                this.checkAllReceivers = value;

                this.SelectUnselectAllReceivers();

                this.OnPropertyChanged(() => this.CheckAllReceiver);
            }
        }

        /// <summary>
        /// Gets cancel selected domain command.
        /// </summary>
        public ICommand SelectedDomainCancelCommand
        {
            get
            {
                return this.selectedDomainCancelCommand ?? (this.selectedDomainCancelCommand = new CommonCommand(
                    param => { this.SelectedDomain = null; },
                    param => this.SelectedDomain != null));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the handle command is enabled.
        /// </summary>
        protected override bool CanHandle
        {
            get
            {
                return this.Status != LoadingStatus.Loading;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the cancel command is enabled.
        /// </summary>
        protected override bool CanCancel
        {
            get
            {
                return this.Status != LoadingStatus.Loading;
            }
        }

        /// <summary>
        /// Subscribe on window close event to prevent window closing during the save operation.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = this.Status == LoadingStatus.Loading;
        }

        /// <summary>
        /// Load information about notification list.
        /// </summary>
        protected override void Load()
        {
            this.Status = LoadingStatus.Loading;

            this.LoadReceivers();

            this.GetEmailAddressesDomains();

            this.LoadNotifications();

            if (this.ListModel.Any())
            {
                this.checkAllReceivers = this.AreAllReceiversSelected();
                this.OnPropertyChanged(() => this.CheckAllReceiver);
            }

            this.OnPropertyChanged(() => this.Count);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Save notification list.
        /// </summary>
        protected override void Handle()
        {
            Task.Factory.StartNewWithDefaultCulture(this.Save);
        }

        private void FilterReceiversBySelectedDomainIfAny()
        {
            if (!this.selectedDomain.IsNullOrEmpty())
            {
                var receivers = this.ListModel.Where(a => a.Email.Substring(a.Email.IndexOf('@') + 1)
                    .Equals(this.selectedDomain, StringComparison.InvariantCultureIgnoreCase));

                this.FilteredModel = new ObservableCollection<PatientModel>(receivers);
            }
            else
            {
                this.FilteredModel = new ObservableCollection<PatientModel>(this.ListModel);
            }
        }

        private void UpdateAllReceiversSelection()
        {
            this.checkAllReceivers = this.FilteredModel.All(a => a.IsSelected);
        }

        private void SelectUnselectAllReceivers()
        {
            if (this.Status != LoadingStatus.Loading)
            {
                foreach (var receiver in this.FilteredModel)
                {
                    receiver.IsSelected = this.checkAllReceivers;
                }
            }
        }

        private void LoadReceivers()
        {
            var receivers = this.unitOfWork.PatientRepository.GetAllExceptDeleted()
                .Where(a => !a.Email.IsNullOrEmpty() && a.IsEmailNotificationAllowed && a.IsEmailChecked);

            this.ListModel = new ObservableCollection<PatientModel>(receivers);
            this.FilteredModel = this.ListModel;
        }

        private void GetEmailAddressesDomains()
        {
            var emailAddressesDomains = new HashSet<string>();
            foreach (var receiver in this.ListModel)
            {
                var domain = this.GetDomainFromEmailAddress(receiver);
                if (!emailAddressesDomains.Contains(domain))
                {
                    emailAddressesDomains.Add(domain);
                }
            }

            this.Domains = emailAddressesDomains.OrderBy(a => a).ToList();
        }

        private string GetDomainFromEmailAddress(PatientModel receiver)
        {
            var startIndex = receiver.Email.IndexOf('@');
            var length = receiver.Email.Length - 1 - startIndex;

            return receiver.Email.Substring(startIndex + 1, length).ToLower();
        }

        private void LoadNotifications()
        {
            this.notifications = this.unitOfWork.NotificationListRepository.GetAllExceptDeletedByGroupId(this.Id).ToList();
        }

        private bool AreAllReceiversSelected()
        {
            var allReceiversWereSelected = true;

            foreach (var receiver in this.ListModel)
            {
                var receiverSelected = this.notifications.FirstOrDefault(a => a.PatientId == receiver.Id);

                if (receiverSelected != null)
                {
                    receiver.IsSelected = true;

                    // Receiver that has already received notification in some group
                    // cannot be deleted from notification list for that group.
                    if (receiverSelected.SendDate != null)
                    {
                        receiver.IsLocked = true;
                    }
                }
                else
                {
                    allReceiversWereSelected = false;
                }
            }

            return allReceiversWereSelected;
        }

        private void Save()
        {
            this.Status = LoadingStatus.Loading;

            var group = this.unitOfWork.NotificationGroupRepository.GetById(this.Id);

            // If group was processed or in processing, changes cannot be saved.
            if (this.IsGroupNotProcessingOrNotProcessed(group))
            {
                this.RemoveUnselectedReceivers();

                this.AddSelectedReceivers();

                this.unitOfWork.Save();

                this.CloseDialog();
            }
            else
            {
                this.HandleFailure();
            }
        }

        private bool IsGroupNotProcessingOrNotProcessed(NotificationGroupModel group)
        {
            return group.Status != (int)NotificationGroupStatus.Processing ||
                   group.Status != (int)NotificationGroupStatus.Processed;
        }

        private void RemoveUnselectedReceivers()
        {
            var receiversToDelete = this.ListModel.Where(a => !a.IsSelected).ToList();

            foreach (var listItem in this.notifications)
            {
                // Receivers that has already received notification in the group
                // cannot be deleted from notification list for that group.
                if (listItem.SendDate == null)
                {
                    var receiverToDelete = receiversToDelete.FirstOrDefault(a => a.Id == listItem.PatientId);

                    if (receiverToDelete != null)
                    {
                        this.unitOfWork.NotificationListRepository.Delete(listItem.Id);
                    }
                }
            }
        }

        private void AddSelectedReceivers()
        {
            var receiversToAdd = this.ListModel.Where(a => a.IsSelected).ToList();

            foreach (var receiver in receiversToAdd)
            {
                var receiverToAdd = this.notifications.FirstOrDefault(a => a.PatientId == receiver.Id);

                if (receiverToAdd == null)
                {
                    var listItem = new NotificationListModel
                    {
                        GroupId = this.Id,
                        PatientId = receiver.Id
                    };

                    this.unitOfWork.NotificationListRepository.Add(listItem);
                }
            }
        }

        private void HandleFailure()
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.ApplicationIdle,
                new Action(() =>
                {
                    this.messageBoxProvider.ListCannotBeSaved();
                    this.OnRequestClose();
                }));
        }
    }
}
