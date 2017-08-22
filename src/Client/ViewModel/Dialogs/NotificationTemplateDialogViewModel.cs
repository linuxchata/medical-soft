using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Providers;
using Common.Communication;
using Common.Enumeration;
using Common.ViewModel;
using Contracts.ViewModel.Dialogs;
using DataAccess;
using Models;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model class for notification template view.
    /// </summary>
    public sealed class NotificationTemplateDialogViewModel :
        ViewModelDialogBase2<NotificationTemplateModel>,
        INotificationTemplateDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMessageBoxProvider messageBoxProvider;

        private string culture;

        private WpfEditor.WebEditor webEditor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTemplateDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="model">Notification template model.</param>
        public NotificationTemplateDialogViewModel(
            IUnitOfWork unitOfWork,
            IMessageBoxProvider messageBoxProvider,
            WorkModeType mode,
            NotificationTemplateModel model = null)
            : base(mode)
        {
            this.unitOfWork = unitOfWork;
            this.messageBoxProvider = messageBoxProvider;

            Task.Factory.StartNewWithDefaultCulture(() => this.Load(model));
        }

        /// <summary>
        /// Gets or sets culture name.
        /// </summary>
        public string Culture
        {
            get
            {
                return this.culture;
            }

            set
            {
                this.culture = value;
                this.OnPropertyChanged(() => this.Culture);
            }
        }

        /// <summary>
        /// Gets or sets web editor.
        /// </summary>
        public WpfEditor.WebEditor WebEditor
        {
            get
            {
                return this.webEditor;
            }

            set
            {
                this.webEditor = value;
                this.OnPropertyChanged(() => this.WebEditor);
            }
        }

        /// <summary>
        /// Load information about templates.
        /// </summary>
        /// <param name="notificationTemplateModel">Template model.</param>
        protected override void Load(NotificationTemplateModel notificationTemplateModel)
        {
            this.Status = LoadingStatus.Loading;

            this.Culture = Thread.CurrentThread.CurrentCulture.Name;

            this.LoadModel(notificationTemplateModel);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Save template.
        /// </summary>
        protected override void Handle()
        {
            this.Status = LoadingStatus.Loading;

            this.UpdateBodyOfTheHtmlModel();

            var response = this.SaveChanges();
            if (response.IsSuccessful)
            {
                this.UpdateIdOfAddedItem(response);
            }
            else
            {
                this.HandleFailure();
            }

            this.CloseDialog();
        }

        private void LoadModel(NotificationTemplateModel notificationTemplateModel)
        {
            if (this.Mode == WorkModeType.Add)
            {
                this.Model = new NotificationTemplateModel();
            }
            else if (this.Mode == WorkModeType.Edit)
            {
                this.Model = notificationTemplateModel.Map();
            }
        }

        private void UpdateBodyOfTheHtmlModel()
        {
            if (this.WebEditor != null)
            {
                var html = this.WebEditor.GetHtml();
                this.Model.Body = html;
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

        private void AddItem()
        {
            this.unitOfWork.NotificationTemplateRepository.Add(this.Model);
            this.Status = LoadingStatus.Added;
        }

        private void EditItem()
        {
            this.unitOfWork.NotificationTemplateRepository.Update(this.Model);
            this.Status = LoadingStatus.Updated;
        }

        private void UpdateIdOfAddedItem(SaveChangesResponse response)
        {
            if (this.Status == LoadingStatus.Added)
            {
                this.Model.Id = response.TryGetValue(DatabaseEntity.NotificationTemplates.ToString());
            }
        }

        private void HandleFailure()
        {
            this.Status = LoadingStatus.Failed;
            this.messageBoxProvider.CannotBeSavedDueToError();
        }
    }
}
