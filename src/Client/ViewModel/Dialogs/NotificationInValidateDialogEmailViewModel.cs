using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Client.Contracts.Dialogs;
using Client.Providers;
using Common.Enumeration;
using Common.ViewModel;
using DataAccess;
using Logger;
using Models;
using Utilities.Resource;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model for invalidate email view.
    /// </summary>
    public sealed class NotificationInValidateDialogEmailViewModel :
        ViewModelDialogBase2<InvalidateEmailModel>,
        INotificationInValidateDialogEmailViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IResourceHandler resourceHandler;

        private readonly IMessageBoxProvider messageBoxProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationInValidateDialogEmailViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="resourceHandler">Resource handler.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        public NotificationInValidateDialogEmailViewModel(
            IUnitOfWork unitOfWork,
            IResourceHandler resourceHandler,
            IMessageBoxProvider messageBoxProvider)
        {
            this.unitOfWork = unitOfWork;
            this.resourceHandler = resourceHandler;
            this.messageBoxProvider = messageBoxProvider;

            this.Model = new InvalidateEmailModel
            {
                Amount = 0
            };
        }

        /// <summary>
        /// Import e-mail addresses.
        /// </summary>
        protected override void Handle()
        {
            this.Status = LoadingStatus.Loading;

            var handled = 0;

            var listOfEmails = this.GetListOfEmailAddresses();

            var receivers = this.unitOfWork.PatientRepository.GetAllExceptDeleted().ToList();

            foreach (var email in listOfEmails)
            {
                this.InvalidateEmailForReceivers(email, receivers, ref handled);
            }

            this.Status = LoadingStatus.Loaded;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => this.ShowSummaryForHandledEmails(handled)));
        }

        private IEnumerable<string> GetListOfEmailAddresses()
        {
            return this.Model.Emails.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private void InvalidateEmailForReceivers(string email, IEnumerable<PatientModel> receivers, ref int handled)
        {
            var listOfReceivers = receivers.Where(a => string.Equals(a.Email, email, StringComparison.OrdinalIgnoreCase));
            foreach (var receiver in listOfReceivers)
            {
                if (receiver.IsEmailChecked)
                {
                    receiver.IsEmailChecked = false;
                    this.unitOfWork.PatientRepository.Update(receiver);

                    Log.Debug("Receivers's {0} (patient id is {1}) email {2} was invalidated.", Log.Args(receiver.FullName, receiver.Id, receiver.Email));

                    handled++;
                }
            }

            this.unitOfWork.Save();
            Log.Debug("{0} email addresses have been invalidated.", Log.Args(handled));
        }

        private void ShowSummaryForHandledEmails(int amount)
        {
            var text = amount > 0
                ? string.Format(this.resourceHandler.GetValue("MessageBoxAmountOfEmailHaveBeenInvalidated"), amount)
                : this.resourceHandler.GetValue("MessageBoxAmountOfEmailHaveBeenInvalidatedIsNull");

            var header = this.resourceHandler.GetValue("MessageBoxAmountOfEmailHaveBeenInvalidatedHeader");

            this.messageBoxProvider.Show(header, text, MessageType.Information);
            this.CloseDialog();
        }
    }
}
