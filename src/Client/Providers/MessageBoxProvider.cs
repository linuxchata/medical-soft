using System;
using System.Windows;
using System.Windows.Threading;
using Client.Builders.MessageBuilder;
using Client.ViewModel;
using Client.Views.Dialogs;
using Common.Builder;
using Common.Enumeration;
using Contracts;
using Utilities.Resource;
using MessageBoxResult = Common.Enumeration.MessageBoxResult;

namespace Client.Providers
{
    /// <summary>
    /// Represents class to handle custom message boxes.
    /// </summary>
    public sealed class MessageBoxProvider : IMessageBoxProvider
    {
        private readonly IMessageViewModelDirector messageViewModelDirector;

        private readonly IViewBuilder viewBuilder;
        
        private readonly IResourceHandler resourceHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxProvider"/> class.
        /// </summary>
        /// <param name="messageViewModelDirector">Message view model factory.</param>
        /// <param name="viewBuilder">View builder.</param>
        /// <param name="resourceHandler">Resource handler.</param>
        public MessageBoxProvider(
            IMessageViewModelDirector messageViewModelDirector,
            IViewBuilder viewBuilder,
            IResourceHandler resourceHandler)
        {
            this.messageViewModelDirector = messageViewModelDirector;
            this.viewBuilder = viewBuilder;
            this.resourceHandler = resourceHandler;
        }

        /// <summary>
        /// Show message box.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="text">The text.</param>
        /// <param name="type">The type.</param>
        /// <returns>Returns message box.</returns>
        public MessageBoxResult Show(string header, string text, MessageType type)
        {
            var viewModel = this.messageViewModelDirector.Construct(type);
            viewModel.Populate(header, text, type);

            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.ApplicationIdle,
                new Action(
                    () =>
                    {
                        this.viewBuilder.Build<StyledMessageDialog, MessageViewModel>(viewModel).ShowDialog();
                    }));

            return viewModel.StyledMessageBoxResult;
        }

        /// <summary>
        /// Check is user confirm save the entry.
        /// </summary>
        /// <returns>Returns clicked message box button.</returns>
        public MessageBoxResult ConfirmSave()
        {
            var text = this.resourceHandler.GetValue("MessageBoxSaveGroupConfirmation");
            var header = this.resourceHandler.GetValue("MessageBoxSaveGroupHeader");

            return this.Show(header, text, MessageType.Question);
        }

        /// <summary>
        /// Show message that the entry was not saved due to error.
        /// </summary>
        public void CannotBeSavedDueToError()
        {
            var text = this.resourceHandler.GetValue("MessageBoxSavingFailed");
            var header = this.resourceHandler.GetValue("MessageBoxSavingFailedHeader");

            this.Show(header, text, MessageType.Error);
        }

        /// <summary>
        /// Show message that the photo was not uploaded due to error.
        /// </summary>
        public void CannotBeUploadedDueToError()
        {
            var text = this.resourceHandler.GetValue("PatientDialogPhotoUploadErrorText");
            var header = this.resourceHandler.GetValue("PatientDialogPhotoUploadErrorTitle");

            this.Show(header, text, MessageType.Error);
        }

        /// <summary>
        /// Check is user confirm deletion of the entry.
        /// </summary>
        /// <returns>Returns clicked message box button.</returns>
        public MessageBoxResult ConfirmDelete()
        {
            var text = this.resourceHandler.GetValue("MessageBoxDeleteConfirmation");
            var header = this.resourceHandler.GetValue("MessageBoxDeleteHeader");

            return this.Show(header, text, MessageType.Question);
        }

        /// <summary>
        /// Show message that the entry cannot be deleted.
        /// </summary>
        public void CannotBeDeleted()
        {
            var text = this.resourceHandler.GetValue("MessageBoxTheEntryCannotBeDeleted");
            var header = this.resourceHandler.GetValue("MessageBoxDeleteHeader");

            this.Show(header, text, MessageType.Warning);
        }

        /// <summary>
        /// Show message that settings cannot be saved.
        /// </summary>
        public void SettingsCannotBeSaved()
        {
            var text = this.resourceHandler.GetValue("MessageBoxSettingsCannotBeSaved");
            var header = this.resourceHandler.GetValue("MessageBoxSettingsCannotBeSavedHeader");

            this.Show(header, text, MessageType.Warning);
        }

        /// <summary>
        /// Show message that the template cannot be deleted since delivery of the e-mail has been started.
        /// </summary>
        public void TemplateCannotBeDeletedSinceGroupIsProcessing()
        {
            var text = this.resourceHandler.GetValue("NotificationTemplateCannotBeDeleted");
            var header = this.resourceHandler.GetValue("MessageBoxDeleteHeader");

            this.Show(header, text, MessageType.Warning);
        }

        /// <summary>
        /// Show message that changes cannot be saved since delivery of the e-mail has been started or processed.
        /// </summary>
        public void ListCannotBeSaved()
        {
            var text = this.resourceHandler.GetValue("MessageBoxNotificationListCannotBeSaved");
            var header = this.resourceHandler.GetValue("MessageBoxNotificationListCannotBeSavedHeader");

            this.Show(header, text, MessageType.Warning);
        }

        /// <summary>
        /// Check is user confirm cancel processing.
        /// </summary>
        /// <returns>Returns clicked message box button.</returns>
        public MessageBoxResult ConfirmCancelDelivery()
        {
            var text = this.resourceHandler.GetValue("NotificationGroupCancelDeliveryConfirmation");
            var header = this.resourceHandler.GetValue("NotificationGroupCancelDeliveryHeader");

            return this.Show(header, text, MessageType.Question);
        }
    }
}
