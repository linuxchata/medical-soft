using Common.Enumeration;

namespace Client.Providers
{
    /// <summary>
    /// Represents interface to handle custom message boxes.
    /// </summary>
    public interface IMessageBoxProvider
    {
        /// <summary>
        /// Show message box.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="text">The text.</param>
        /// <param name="type">The type.</param>
        /// <returns>Returns message box.</returns>
        MessageBoxResult Show(string header, string text, MessageType type);

        /// <summary>
        /// Check is user confirm save the entry.
        /// </summary>
        /// <returns>Returns clicked message box button.</returns>
        MessageBoxResult ConfirmSave();

        /// <summary>
        /// Check is user confirm deletion of the entry.
        /// </summary>
        /// <returns>Returns clicked message box button.</returns>
        MessageBoxResult ConfirmDelete();

        /// <summary>
        /// Show message that the entry was not saved due to error.
        /// </summary>
        void CannotBeSavedDueToError();

        /// <summary>
        /// Show message that the entry cannot be deleted.
        /// </summary>
        void CannotBeDeleted();

        /// <summary>
        /// Show message that settings cannot be saved.
        /// </summary>
        void SettingsCannotBeSaved();

        /// <summary>
        /// Show message that the template cannot be deleted since delivery of the e-mail has been started.
        /// </summary>
        void TemplateCannotBeDeletedSinceGroupIsProcessing();

        /// <summary>
        /// Show message that changes cannot be saved since delivery of the e-mail has been started or processed.
        /// </summary>
        void ListCannotBeSaved();

        /// <summary>
        /// Show message that the photo was not uploaded due to error.
        /// </summary>
        void CannotBeUploadedDueToError();

        /// <summary>
        /// Check is user confirm cancel processing.
        /// </summary>
        /// <returns>Returns clicked message box button.</returns>
        MessageBoxResult ConfirmCancelDelivery();
    }
}
