using System;
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
using Models.Enumeration;
using Utilities.EventAggregator;

namespace Client.ViewModel
{
    /// <summary>
    /// Represents view model for notification group.
    /// </summary>
    public sealed class NotificationGroupViewModel : ViewModelMainBase<NotificationGroupModel>, INotificationGroupViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IEventAggregator eventAggregator;

        private readonly IViewModelBuilder viewModelBuilder;

        private readonly IViewBuilder viewBuilder;

        private readonly IMessageBoxProvider messageBoxProvider;

        private readonly IApplicationSettings applicationSettings;

        private ICommand inValidateEmailsCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationGroupViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="eventAggregator">Event aggregator.</param>
        /// <param name="viewModelBuilder">View model builder.</param>
        /// <param name="viewBuilder">View builder.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="applicationSettings">Application settings.</param>
        public NotificationGroupViewModel(
            IUnitOfWork unitOfWork,
            IEventAggregator eventAggregator,
            IViewModelBuilder viewModelBuilder,
            IViewBuilder viewBuilder,
            IMessageBoxProvider messageBoxProvider,
            IApplicationSettings applicationSettings)
        {
            this.unitOfWork = unitOfWork;
            this.eventAggregator = eventAggregator;
            this.viewModelBuilder = viewModelBuilder;
            this.viewBuilder = viewBuilder;
            this.messageBoxProvider = messageBoxProvider;
            this.applicationSettings = applicationSettings;

            Task.Factory.StartNewWithDefaultCulture(this.UpdateData);
        }

        /// <summary>
        /// Gets invalidate command.
        /// </summary>
        public ICommand InValidateEmailsCommand
        {
            get
            {
                return this.inValidateEmailsCommand ?? (this.inValidateEmailsCommand = new CommonCommand(
                    param => this.InvalidateEmails(),
                    param => true));
            }
        }

        /// <summary>
        /// Gets a value indicating whether application is run under client only mode.
        /// </summary>
        public bool IsClientMode
        {
            get
            {
                return this.applicationSettings.RunningMode == RunningMode.ClientOnly;
            }
        }

        /// <summary>
        /// Load information about notification group.
        /// </summary>
        public override void UpdateData()
        {
            this.Status = LoadingStatus.Loading;

            var notificationGroups = this.unitOfWork.NotificationGroupRepository.GetAllExceptDeleted();
            this.Model = new ObservableCollection<NotificationGroupModel>(notificationGroups);

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
            this.eventAggregator.Subscribe<NotificationTemplateChangedEvent>(this.UpdateDataAsync);
            this.eventAggregator.Subscribe<NotificationGroupStatusChangedEvent>(this.UpdateDataAsync);
        }

        /// <summary>
        /// Unsubscribe method.
        /// </summary>
        public override void Unsubscribe()
        {
            base.Unsubscribe();
            this.eventAggregator.Unsubscribe<NotificationTemplateChangedEvent>();
            this.eventAggregator.Unsubscribe<NotificationGroupStatusChangedEvent>();
        }

        /// <summary>
        /// Add/Edit education notification group.
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
        /// Delete group.
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

        private void AddItem(WorkModeType mode)
        {
            var dialogViewModel = this.viewModelBuilder.Build<NotificationGroupDialogViewModel>(
                new ResolverParameter(ParameterName.Mode, mode));

            this.viewBuilder.Build<NotificationGroupDialogView, NotificationGroupDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Added || dialogViewModel.Status == LoadingStatus.Updated)
            {
                // Workaround. Status name is localized via database.
                var group = this.unitOfWork.NotificationGroupRepository.GetById(dialogViewModel.Model.Id);
                dialogViewModel.Model.StatusName = group.StatusName;

                this.Model.Insert(0, dialogViewModel.Model);
            }

            this.OnPropertyChanged(() => this.Count);
        }

        private void EditItem(WorkModeType mode)
        {
            var dialogViewModel = this.viewModelBuilder.Build<NotificationGroupDialogViewModel>(
                new ResolverParameter(ParameterName.Mode, mode),
                new ResolverParameter(ParameterName.Model, this.SelectedItem));

            this.viewBuilder.Build<NotificationGroupDialogView, NotificationGroupDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Updated)
            {
                dialogViewModel.Model.Map(this.SelectedItem);
            }
        }

        private bool IsDeletionConfirmedByUser()
        {
            return this.messageBoxProvider.ConfirmDelete() == MessageBoxResult.Yes;
        }

        private bool TryDeleteItemFromDatasource()
        {
            var group = this.unitOfWork.NotificationGroupRepository.GetById(this.SelectedItem.Id);

            if (group.Status == (int)NotificationGroupStatus.Processing)
            {
                this.messageBoxProvider.TemplateCannotBeDeletedSinceGroupIsProcessing();
                return false;
            }

            this.unitOfWork.NotificationGroupRepository.TryHide(this.SelectedItem.Id);
            var response = this.unitOfWork.Save();
            return response.IsSuccessful;
        }

        private void DeleteItemFromCollection()
        {
            var itemToDelete = this.Model.FirstOrDefault(a => a.Id == this.SelectedItem.Id);
            if (itemToDelete != null)
            {
                this.Model.Remove(itemToDelete);
            }
        }

        private void InvalidateEmails()
        {
            this.viewBuilder.Build<NotificationInValidateEmailDialogView, INotificationInValidateDialogEmailViewModel>().ShowDialog();

            this.eventAggregator.Publish<NotificationGroupChangedEvent>();
        }
    }
}
