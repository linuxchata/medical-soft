using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Contracts;
using Client.Providers;
using Client.ViewModel.Dialogs;
using Client.Views.Dialogs;
using Common;
using Common.Builder;
using Common.Enumeration;
using Common.Events;
using Common.ViewModel;
using Contracts.ViewModel.Dialogs;
using DataAccess;
using Logger;
using Models;
using Models.Enumeration;
using Utilities.EventAggregator;

namespace Client.ViewModel
{
    /// <summary>
    /// Represents view model for notification template.
    /// </summary>
    public sealed class NotificationTemplateViewModel : ViewModelMainBase<NotificationTemplateModel>, INotificationTemplateViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IEventAggregator eventAggregator;

        private readonly IViewModelBuilder viewModelBuilder;

        private readonly IViewBuilder viewBuilder;

        private readonly IMessageBoxProvider messageBoxProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTemplateViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="eventAggregator">Event aggregator.</param>
        /// <param name="viewModelBuilder">View model builder.</param>
        /// <param name="viewBuilder">View builder.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        public NotificationTemplateViewModel(
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
        /// Load information about notification template.
        /// </summary>
        public override void UpdateData()
        {
            this.Status = LoadingStatus.Loading;

            var templates = this.unitOfWork.NotificationTemplateRepository.GetAllExceptDeleted();
            this.Model = new ObservableCollection<NotificationTemplateModel>(templates);

            if (this.SelectedItem != null)
            {
                this.SelectedItem = null;
            }

            this.OnPropertyChanged(() => this.Count);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Add/Edit education notification template.
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
        /// Delete template.
        /// </summary>
        protected override void Delete()
        {
            if (this.IsDeletionConfirmedByUser())
            {
                if (this.TryDeleteItemFromDatasource())
                {
                    this.DeleteItemFromCollection();

                    this.eventAggregator.Publish<NotificationTemplateChangedEvent>();
                }

                this.OnPropertyChanged(() => this.Count);
            }
        }

        private void AddItem(WorkModeType mode)
        {
            var dialogViewModel = this.viewModelBuilder.Build<NotificationTemplateDialogViewModel>(
                new ResolverParameter(ParameterName.Mode, mode));

            this.viewBuilder.Build<NotificationTemplateDialogView, INotificationTemplateDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Added)
            {
                this.Model.Insert(0, dialogViewModel.Model);
            }

            this.OnPropertyChanged(() => this.Count);
        }

        private void EditItem(WorkModeType mode)
        {
            var dialogViewModel = this.viewModelBuilder.Build<NotificationTemplateDialogViewModel>(
                new ResolverParameter(ParameterName.Mode, mode),
                new ResolverParameter(ParameterName.Model, this.SelectedItem));

            this.viewBuilder.Build<NotificationTemplateDialogView, INotificationTemplateDialogViewModel>(dialogViewModel).ShowDialog();

            if (dialogViewModel.Status == LoadingStatus.Updated)
            {
                dialogViewModel.Model.Map(this.SelectedItem);
                this.eventAggregator.Publish<NotificationTemplateChangedEvent>();
            }
        }

        private bool IsDeletionConfirmedByUser()
        {
            return this.messageBoxProvider.ConfirmDelete() == MessageBoxResult.Yes;
        }

        private bool TryDeleteItemFromDatasource()
        {
            var groups = this.unitOfWork.NotificationGroupRepository.GetAllExceptDeleted()
                .Where(a => a.TemplateId == this.SelectedItem.Id);

            if (groups.Any(g => g.Status == (int)NotificationGroupStatus.Processing))
            {
                this.messageBoxProvider.TemplateCannotBeDeletedSinceGroupIsProcessing();
                return false;
            }

            var isTemplateHidden = this.unitOfWork.NotificationTemplateRepository.TryHide(this.SelectedItem.Id);
            if (!isTemplateHidden)
            {
                this.messageBoxProvider.CannotBeDeleted();
                return false;
            }

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
    }
}
