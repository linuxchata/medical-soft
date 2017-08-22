using System.Windows;
using System.Windows.Input;
using Common.Commands;
using Common.Enumeration;
using Common.ViewModel;

namespace Client.ViewModel
{
    /// <summary>
    /// Represents view model for Message view.
    /// </summary>
    public sealed class MessageViewModel : ViewModelBase
    {
        private ICommand confirmationCommand;

        private ICommand cancelCommand;

        /// <summary>
        /// Gets or sets image source.
        /// </summary>
        public string ImageSource { get; set; }

        /// <summary>
        /// Gets or sets message header.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets message body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets type of the message.
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// Gets or sets visibility of the OK/Yes button.
        /// </summary>
        public Visibility OkButtonVisibility { get; set; }

        /// <summary>
        /// Gets or sets visibility of the Cancel/No button.
        /// </summary>
        public Visibility CancelButtonVisibility { get; set; }

        /// <summary>
        /// Gets or sets content of the OK/Yes button.
        /// </summary>
        public string OkButtonContent { get; set; }

        /// <summary>
        /// Gets or sets content of the Cancel/No button.
        /// </summary>
        public string CancelButtonContent { get; set; }

        /// <summary>
        /// Gets or sets message box result.
        /// </summary>
        public Common.Enumeration.MessageBoxResult StyledMessageBoxResult { get; set; }

        #region Commands

        /// <summary>
        /// Gets confirmation (OK/Yes) command.
        /// </summary>
        public ICommand ConfirmationCommand
        {
            get
            {
                return this.confirmationCommand ?? (this.confirmationCommand = new CommonCommand(
                    param => this.ConfirmationLogic(),
                    param => true));
            }
        }

        /// <summary>
        /// Gets cancellation (Cancel/No) command.
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return this.cancelCommand ?? (this.cancelCommand = new CommonCommand(
                    param => this.CancelationLogic(),
                    param => true));
            }
        }

        #endregion

        /// <summary>
        /// Populate view model.
        /// </summary>
        /// <param name="messageHeader">Message header.</param>
        /// <param name="messageBody">Message body.</param>
        /// <param name="type">Type of the message.</param>
        public void Populate(string messageHeader, string messageBody, MessageType type)
        {
            this.Header = messageHeader;
            this.Body = messageBody;
            this.MessageType = type;
        }

        private void ConfirmationLogic()
        {
            switch (this.MessageType)
            {
                case MessageType.Information:
                case MessageType.Warning:
                case MessageType.Error:
                    this.StyledMessageBoxResult = Common.Enumeration.MessageBoxResult.Ok;
                    break;
                case MessageType.Question:
                    this.StyledMessageBoxResult = Common.Enumeration.MessageBoxResult.Yes;
                    break;
                default:
                    this.StyledMessageBoxResult = Common.Enumeration.MessageBoxResult.None;
                    break;
            }

            this.OnRequestClose();
        }

        private void CancelationLogic()
        {
            switch (this.MessageType)
            {
                case MessageType.Information:
                case MessageType.Warning:
                case MessageType.Error:
                    this.StyledMessageBoxResult = Common.Enumeration.MessageBoxResult.Cancel;
                    break;
                case MessageType.Question:
                    this.StyledMessageBoxResult = Common.Enumeration.MessageBoxResult.No;
                    break;
                default:
                    this.StyledMessageBoxResult = Common.Enumeration.MessageBoxResult.None;
                    break;
            }

            this.OnRequestClose();
        }
    }
}
