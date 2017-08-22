using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Contracts.Dialogs;
using Client.Providers;
using Client.Views.Dialogs;
using Common;
using Common.Builder;
using Common.Commands;
using Common.Communication;
using Common.Enumeration;
using Common.ViewModel;
using DataAccess;
using Models;
using Models.Enumeration;
using Services.Email;
using Utilities.Resource;
using MessageBoxResult = Common.Enumeration.MessageBoxResult;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model class for notification group dialog.
    /// </summary>
    public sealed class NotificationGroupDialogViewModel :
        ViewModelDialogBase2<NotificationGroupModel>,
        INotificationGroupDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IViewModelBuilder viewModelBuilder;

        private readonly IViewBuilder viewBuilder;

        private readonly IResourceHandler resourceHandler;

        private readonly IMessageBoxProvider messageBoxProvider;

        private readonly IEmailDeliveryService emailDeliveryService;

        private List<ItemModel> templates;

        private List<string> hours;

        private List<string> minutes;

        private string selectedHour;

        private string selectedMinutes;

        private ICommand selectListCommand;

        private ICommand selectManageListCommand;

        private ICommand cancelDeliveryCommand;

        private ICommand restartDeliveryCommand;

        private ICommand restartDeliveryForFailedCommand;

        private bool canRestartDeliveryForFailedReceivers;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationGroupDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="viewModelBuilder">View model builder.</param>
        /// <param name="viewBuilder">View builder.</param>
        /// <param name="resourceHandler">Resource handler.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="emailDeliveryService">Email delivery service.</param>
        public NotificationGroupDialogViewModel(
            IUnitOfWork unitOfWork,
            IViewModelBuilder viewModelBuilder,
            IViewBuilder viewBuilder,
            IResourceHandler resourceHandler,
            IMessageBoxProvider messageBoxProvider,
            IEmailDeliveryService emailDeliveryService)
        {
            this.unitOfWork = unitOfWork;
            this.viewModelBuilder = viewModelBuilder;
            this.viewBuilder = viewBuilder;
            this.resourceHandler = resourceHandler;
            this.messageBoxProvider = messageBoxProvider;
            this.emailDeliveryService = emailDeliveryService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationGroupDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="viewModelBuilder">View model builder.</param>
        /// <param name="viewBuilder">View builder.</param>
        /// <param name="resourceHandler">Resource handler.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="emailDeliveryService">Email delivery service.</param>
        /// <param name="mode">Mode (Add/Edit).</param>
        /// <param name="model">Notification group model.</param>
        public NotificationGroupDialogViewModel(
            IUnitOfWork unitOfWork,
            IViewModelBuilder viewModelBuilder,
            IViewBuilder viewBuilder,
            IResourceHandler resourceHandler,
            IMessageBoxProvider messageBoxProvider,
            IEmailDeliveryService emailDeliveryService,
            WorkModeType mode,
            NotificationGroupModel model)
            : base(mode)
        {
            this.unitOfWork = unitOfWork;
            this.viewModelBuilder = viewModelBuilder;
            this.viewBuilder = viewBuilder;
            this.resourceHandler = resourceHandler;
            this.messageBoxProvider = messageBoxProvider;
            this.emailDeliveryService = emailDeliveryService;

            Task.Factory.StartNewWithDefaultCulture(() => this.Load(model));
        }

        /// <summary>
        /// Gets or sets list of all templates.
        /// </summary>
        public List<ItemModel> Templates
        {
            get
            {
                return this.templates;
            }

            set
            {
                this.templates = value;
                this.OnPropertyChanged(() => this.Templates);
            }
        }

        /// <summary>
        /// Gets or sets list of the hours.
        /// </summary>
        public List<string> Hours
        {
            get
            {
                return this.hours;
            }

            set
            {
                this.hours = value;
                this.OnPropertyChanged(() => this.Hours);
            }
        }

        /// <summary>
        /// Gets or sets list of the minutes.
        /// </summary>
        public List<string> Minutes
        {
            get
            {
                return this.minutes;
            }

            set
            {
                this.minutes = value;
                this.OnPropertyChanged(() => this.Minutes);
            }
        }

        /// <summary>
        /// Gets or sets selected hours.
        /// </summary>
        public string SelectedHour
        {
            get
            {
                return this.selectedHour;
            }

            set
            {
                this.selectedHour = value;
                this.OnPropertyChanged(() => this.SelectedHour);
            }
        }

        /// <summary>
        /// Gets or sets selected minutes.
        /// </summary>
        public string SelectedMinute
        {
            get
            {
                return this.selectedMinutes;
            }

            set
            {
                this.selectedMinutes = value;
                this.OnPropertyChanged(() => this.SelectedMinute);
            }
        }

        /// <summary>
        /// Gets select notification list command.
        /// </summary>
        public ICommand SelectListCommand
        {
            get
            {
                return this.selectListCommand ?? (this.selectListCommand = new CommonCommand(
                    param => this.InitNotificationListView(),
                    param => this.CanHandle));
            }
        }

        /// <summary>
        /// Gets select notification manage list command.
        /// </summary>
        public ICommand SelectManageListCommand
        {
            get
            {
                return this.selectManageListCommand ?? (this.selectManageListCommand = new CommonCommand(
                    param => this.InitNotificationListManageView(),
                    param => this.CanHandle
                             && (this.Model.Status != (int)NotificationGroupStatus.Processed
                                 && this.Model.Status != (int)NotificationGroupStatus.Processing)));
            }
        }

        /// <summary>
        /// Gets cancel delivery command.
        /// </summary>
        public ICommand CancelDeliveryCommand
        {
            get
            {
                return this.cancelDeliveryCommand ?? (this.cancelDeliveryCommand = new CommonCommand(
                    param => this.CancelDelivery(),
                    param => this.Model.Status == (int)NotificationGroupStatus.Processing));
            }
        }

        /// <summary>
        /// Gets restart delivery command.
        /// </summary>
        public ICommand RestartDeliveryCommand
        {
            get
            {
                return this.restartDeliveryCommand ?? (this.restartDeliveryCommand = new CommonCommand(
                    param => this.RestartDelivery(),
                    param => this.Model.Status == (int)NotificationGroupStatus.Cancelled));
            }
        }

        /// <summary>
        /// Gets restart delivery for failed receivers command.
        /// </summary>
        public ICommand RestartDeliveryForFailedCommand
        {
            get
            {
                return this.restartDeliveryForFailedCommand ?? (this.restartDeliveryForFailedCommand = new CommonCommand(
                    param => this.RestartDeliveryForFailedReceivers(),
                    param => this.CanRestartDeliveryForFailedReceivers()));
            }
        }

        /// <summary>
        /// Load information about group.
        /// </summary>
        /// <param name="notificationGroupModel">Group model.</param>
        protected override void Load(NotificationGroupModel notificationGroupModel)
        {
            this.Status = LoadingStatus.Loading;

            this.LoadModel(notificationGroupModel);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Save group.
        /// </summary>
        protected override void Handle()
        {
            this.Status = LoadingStatus.Loading;

            this.UpdateStatusOfDeliveryProcess();

            this.SetStartDate();

            this.SetSelectedTemplate();

            var response = this.SaveChanges();
            if (response.IsSuccessful)
            {
                this.UpdateIdOfAddedItem(response);
            }
            else
            {
                this.HandleFailure();
            }
        }

        /// <summary>
        /// Save group.
        /// </summary>
        /// <param name="parameter">Is dialog has to be closed after save.</param>
        protected override void Handle(object parameter)
        {
            this.Handle();

            if (parameter == null || Convert.ToBoolean(parameter))
            {
                this.CloseDialog();
            }
        }

        private void LoadModel(NotificationGroupModel notificationGroupModel)
        {
            this.Hours = TimeHelper.Hours;
            this.Minutes = TimeHelper.Minutes;

            this.Templates = this.unitOfWork.NotificationTemplateRepository.GetAllForList();

            if (this.Mode == WorkModeType.Add)
            {
                this.Model = new NotificationGroupModel
                {
                    Status = (int)NotificationGroupStatus.NotProcessed,
                    StartDate = DateTime.Now
                };

                this.SelectedHour = TimeHelper.Hours.FirstOrDefault();
                this.SelectedMinute = TimeHelper.Minutes.FirstOrDefault();
            }
            else if (this.Mode == WorkModeType.Edit)
            {
                this.Model = notificationGroupModel.Map();

                this.RefeshData();
            }

            this.RefreshGroupStatus();

            var receivers = this.unitOfWork.NotificationListRepository.GetAllExceptDeletedByGroupId(this.Model.Id)
                .Where(a => a.Status == (int)NotificationListStatus.Fail);
            this.canRestartDeliveryForFailedReceivers = receivers.Any();
        }

        private void UpdateStatusOfDeliveryProcess()
        {
            if (this.Model.Id != 0)
            {
                // Check whether processing has been started.
                var group = this.unitOfWork.NotificationGroupRepository.GetById(this.Model.Id);
                this.Model.Status = @group.Status;
                this.Model.StatusName = @group.StatusName;
            }
        }

        private void SetStartDate()
        {
            var startDate = this.Model.StartDate;
            var hour = Convert.ToInt32(this.SelectedHour);
            var minute = Convert.ToInt32(this.SelectedMinute);
            this.Model.StartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, hour, minute, 00);
        }

        private void SetSelectedTemplate()
        {
            var selectedTemplate = this.Templates.FirstOrDefault(a => a.Id == this.Model.TemplateId);
            if (selectedTemplate != null)
            {
                this.Model.Template = selectedTemplate.Name;
            }
        }

        private SaveChangesResponse SaveChanges()
        {
            if (this.Mode == WorkModeType.Add)
            {
                this.AddItem();
            }
            else if (this.Mode == WorkModeType.Edit)
            {
                this.EditItem();
            }

            return this.unitOfWork.Save();
        }

        private void EditItem()
        {
            this.unitOfWork.NotificationGroupRepository.Update(this.Model);
            this.Status = LoadingStatus.Updated;
        }

        private void AddItem()
        {
            this.unitOfWork.NotificationGroupRepository.Add(this.Model);
            this.Status = LoadingStatus.Added;
            this.Mode = WorkModeType.Edit;
        }

        private void UpdateIdOfAddedItem(SaveChangesResponse response)
        {
            if (this.Status == LoadingStatus.Added)
            {
                this.Model.Id = response.TryGetValue(DatabaseEntity.NotificationGroups.ToString());
            }
        }

        private void HandleFailure()
        {
            this.Status = LoadingStatus.Failed;
            this.messageBoxProvider.CannotBeSavedDueToError();
        }

        private void InitNotificationListView()
        {
            var viewModel = this.viewModelBuilder.Build<INotificationListDialogViewModel>(
                new ResolverParameter(ParameterName.Id, this.Model.Id));

            this.viewBuilder.Build<NotificationListDialogView, INotificationListDialogViewModel>(viewModel).ShowDialog();

            if (this.Model.Id != 0)
            {
                this.RefeshData();
                this.RefreshGroupStatus();
            }
        }

        private void InitNotificationListManageView()
        {
            Action loadNotificationListManageView = () =>
            {
                var viewModel = this.viewModelBuilder.Build<INotificationListManageDialogViewModel>(
                    new ResolverParameter(ParameterName.Id, this.Model.Id));
                var view = this.viewBuilder.Build<NotificationListManageDialogView, INotificationListManageDialogViewModel>(viewModel);
                view.ShowDialog();
            };

            if (this.Model.Id == 0)
            {
                if (this.messageBoxProvider.ConfirmSave() == MessageBoxResult.Yes)
                {
                    this.Handle(false);
                    loadNotificationListManageView();
                }
            }
            else
            {
                loadNotificationListManageView();
            }

            this.Status = LoadingStatus.Loaded;
        }

        private void RefeshData()
        {
            this.SelectedHour = this.Model.StartDate.ToString("HH");
            this.SelectedMinute = this.Model.StartDate.ToString("mm");
        }

        private void RestartDelivery()
        {
            var group = this.unitOfWork.NotificationGroupRepository.GetById(this.Model.Id);

            group.Status = (int)NotificationGroupStatus.NotProcessed;

            this.Model.Status = group.Status;

            this.unitOfWork.NotificationGroupRepository.Update(group);
            this.unitOfWork.Save();

            this.RefreshGroupStatus();
        }

        private void RestartDeliveryForFailedReceivers()
        {
            var receivers = this.unitOfWork.NotificationListRepository.GetAllExceptDeletedByGroupId(this.Model.Id)
                .Where(a => a.Status == (int)NotificationListStatus.Fail)
                .ToList();

            foreach (var receiver in receivers)
            {
                receiver.Status = (int)NotificationListStatus.NotSent;
                receiver.SendDate = null;
                receiver.ErrorDescription = null;
                this.unitOfWork.NotificationListRepository.Update(receiver);
            }

            this.unitOfWork.Save();

            this.RestartDelivery();
        }

        private bool CanRestartDeliveryForFailedReceivers()
        {
            return this.Model.Status == (int)NotificationGroupStatus.Processed
                && this.canRestartDeliveryForFailedReceivers;
        }

        private void CancelDelivery()
        {
            if (this.messageBoxProvider.ConfirmCancelDelivery() == MessageBoxResult.Yes)
            {
                var group = this.unitOfWork.NotificationGroupRepository.GetById(this.Model.Id);

                group.Status = (int)NotificationGroupStatus.Cancelled;
                group.CompletedDate = DateTime.Now;

                this.Model.Status = group.Status;
                this.Model.CompletedDate = group.CompletedDate;

                this.unitOfWork.NotificationGroupRepository.Update(group);
                this.unitOfWork.Save();

                this.emailDeliveryService.Cancel();
            }

            this.RefreshGroupStatus();
        }

        private void RefreshGroupStatus()
        {
            switch (this.Model.Status)
            {
                case (int)NotificationGroupStatus.NotProcessed:
                    this.Model.Result = this.resourceHandler.GetValue("NotificationGroupNotProcessed");
                    break;
                case (int)NotificationGroupStatus.Processing:
                    this.Model.Result = this.resourceHandler.GetValue("NotificationGroupProcessing");
                    break;
                case (int)NotificationGroupStatus.Processed:
                    this.Model.Result = string.Format(this.resourceHandler.GetValue("NotificationGroupProcessed"), this.Model.CompletedDate);
                    break;
                case (int)NotificationGroupStatus.Cancelled:
                    this.Model.Result = this.resourceHandler.GetValue("NotificationGroupCancelled");
                    break;
            }
        }
    }
}
