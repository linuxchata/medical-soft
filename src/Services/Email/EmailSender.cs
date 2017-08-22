using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using Common.Events;
using DataAccess;
using Logger;
using Models;
using Models.Enumeration;
using Utilities;
using Utilities.EventAggregator;
using Utilities.Resource;

namespace Services.Email
{
    /// <summary>
    /// Allows to send e-mail by using the Simple Mail Transfer Protocol (SMTP).
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IEventAggregator eventAggregator;

        private readonly IResourceHandler resourceHandler;

        private readonly IEmailSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSender"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="eventAggregator">Event aggregator.</param>
        /// <param name="resourceHandler">Resource handler.</param>
        /// <param name="settings">E-mail settings.</param>
        public EmailSender(
            IUnitOfWork unitOfWork,
            IEventAggregator eventAggregator,
            IResourceHandler resourceHandler,
            IEmailSettings settings)
        {
            this.unitOfWork = unitOfWork;
            this.eventAggregator = eventAggregator;
            this.resourceHandler = resourceHandler;
            this.settings = settings;
        }

        /// <summary>
        /// Gets or sets email delivery service.
        /// </summary>
        public IEmailDeliveryService EmailDeliveryService { private get; set; }

        /// <summary>
        /// Send e-mails.
        /// </summary>
        /// <param name="receivers">List of the receiver to send e-mail notification.</param>
        /// <param name="group">Notification group.</param>
        public void SendEmail(IList<NotificationListModel> receivers, NotificationGroupModel group)
        {
            this.settings.Refresh();

            if (receivers.Count == 0)
            {
                this.ProcessGroupWithoutReceivers(group);
                return;
            }

            this.SetProcessingStatusForGroup(group);

            try
            {
                Log.Debug("Start sending emails to receivers with {0} (id is {1}) template.", Log.Args(group.Template, group.TemplateId));

                var mailMessage = this.CreateAndFillMailMessage(group.TemplateId);
                var smtpClient = this.InitializeSmtpClient();

                foreach (var receiver in receivers)
                {
                    if (this.EmailDeliveryService.IsCancellationRequested)
                    {
                        return;
                    }

                    if (receiver.SendDate != null)
                    {
                        continue;
                    }

                    var addressToSend = new MailAddress(receiver.PatientEmail);
                    mailMessage.To.Add(addressToSend);

                    try
                    {
                        receiver.StartDate = DateTime.Now;

                        smtpClient.Send(mailMessage);

                        this.SetSuccessStatusForReceiver(receiver);

                        this.LogSuccessfulResult(receiver);
                    }
                    catch (Exception ex)
                    {
                        Log.Exception(ex);
                        this.SetErrorStatusForReceiver(receiver, ex.Message);
                    }
                    finally
                    {
                        mailMessage.To.Remove(addressToSend);

                        this.UpdateReceiverStatusAndFireEvent(receiver);
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(this.settings.SendingDelayInSeconds));
                }

                this.SetProcessedStatusForGroup(group);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        private void SetProcessingStatusForGroup(NotificationGroupModel group)
        {
            group.Status = (int)NotificationGroupStatus.Processing;

            this.UpdateGroupStatusAndFireEvent(group);
        }

        private void SetProcessedStatusForGroup(NotificationGroupModel group)
        {
            group.Status = (int)NotificationGroupStatus.Processed;
            group.CompletedDate = DateTime.Now;

            this.UpdateGroupStatusAndFireEvent(group);
        }

        private void UpdateGroupStatusAndFireEvent(NotificationGroupModel group)
        {
            this.unitOfWork.NotificationGroupRepository.Update(group);
            this.unitOfWork.Save();

            this.eventAggregator.Publish<NotificationGroupStatusChangedEvent>();
        }

        private void ProcessGroupWithoutReceivers(NotificationGroupModel group)
        {
            group.Status = (int)NotificationGroupStatus.Processed;
            this.unitOfWork.NotificationGroupRepository.Update(group);
            this.unitOfWork.Save();

            this.eventAggregator.Publish<NotificationGroupStatusChangedEvent>();

            Log.Debug("There are no active e-mail receivers. Group delivery has been processed.");
        }

        private MailMessage CreateAndFillMailMessage(int templateId)
        {
            var template = this.unitOfWork.NotificationTemplateRepository.GetById(templateId);

            var mailMessage = this.CreateMailMessage(template);

            var htmlObject = HtmlHelper.ParseHtml(template.Body);
            var alternateView = AlternateView.CreateAlternateViewFromString(htmlObject.Item2.Html, null, htmlObject.Item2.Type);

            foreach (var item in htmlObject.Item1)
            {
                var imageData = Convert.FromBase64String(item.Content);
                var linkedResource = new LinkedResource(new MemoryStream(imageData), item.Type)
                {
                    ContentId = item.ContentId,
                    TransferEncoding = TransferEncoding.Base64
                };
                alternateView.LinkedResources.Add(linkedResource);
            }

            mailMessage.AlternateViews.Add(alternateView);
            return mailMessage;
        }

        private MailMessage CreateMailMessage(NotificationTemplateModel template)
        {
            return new MailMessage
            {
                From = new MailAddress(this.settings.SmtpFromAddress),
                Subject = template.Title,
                IsBodyHtml = true
            };
        }

        private SmtpClient InitializeSmtpClient()
        {
            var smtpClient = new SmtpClient(this.settings.SmtpHost, this.settings.SmtpPort)
            {
                EnableSsl = this.settings.SmtpEnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(this.settings.SmtpUserName, this.settings.SmtpPassword),
            };
            return smtpClient;
        }

        private void SetSuccessStatusForReceiver(NotificationListModel receiver)
        {
            receiver.SendDate = DateTime.Now;
            receiver.Status = (int)NotificationListStatus.Success;
        }

        private void LogSuccessfulResult(NotificationListModel receiver)
        {
            var resultMessage = this.resourceHandler.GetValue("NotificationEmailSuccessfulResult");
            Log.Debug(resultMessage, Log.Args(receiver.PatientName, receiver.PatientEmail, DateTime.Now));
        }

        private void SetErrorStatusForReceiver(NotificationListModel receiver, string exceptionMessage)
        {
            receiver.SendDate = DateTime.Now;
            receiver.Status = (int)NotificationListStatus.Fail;
            receiver.ErrorDescription = exceptionMessage;
        }

        private void UpdateReceiverStatusAndFireEvent(NotificationListModel receiver)
        {
            this.unitOfWork.NotificationListRepository.Update(receiver);
            this.unitOfWork.Save();

            this.eventAggregator.Publish<NotificationListChangedEvent>();
        }
    }
}
