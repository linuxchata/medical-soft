using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Services;
using DataAccess;
using Logger;
using Models;
using Models.Enumeration;

namespace Services.Email
{
    /// <summary>
    /// Represents e-mail delivery logic.
    /// </summary>
    public sealed class EmailDeliveryService : ServiceBase, IEmailDeliveryService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IEmailSender emailSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailDeliveryService"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="emailSender">Email sender.</param>
        public EmailDeliveryService(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            this.unitOfWork = unitOfWork;
            this.emailSender = emailSender;
            this.emailSender.EmailDeliveryService = this;
        }

        /// <summary>
        /// Represents email delivery logic.
        /// </summary>
        protected override void Do()
        {
            while (true)
            {
                IEnumerable<NotificationGroupModel> activeGroups;

                do
                {
                    activeGroups = this.GetActiveGroups();
                    if (activeGroups.Any())
                    {
                        this.HandleFirstActiveGroup(activeGroups);
                    }
                }
                while (activeGroups.Any());

                this.Sleep();
            }
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        /// <param name="disposing">Define whether managed objects have to be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose();
        }

        private List<NotificationGroupModel> GetActiveGroups()
        {
            var activeGroups = this.unitOfWork.NotificationGroupRepository.GetActiveExceptDeleted().ToList();
            Log.Debug("There was(were) found {0} active notification group(s).", Log.Args(activeGroups.Count));

            return activeGroups;
        }

        private void HandleFirstActiveGroup(IEnumerable<NotificationGroupModel> activeGroups)
        {
            var group = activeGroups.FirstOrDefault();
            if (group != null)
            {
                Log.Debug("Start handling {0} notification group.", Log.Args(group.Description));

                var receivers = this.GetReceivers(group);

                this.emailSender.SendEmail(receivers, group);
                this.IsCancellationRequested = false;

                Log.Debug("Notification group {0} was handled.", Log.Args(group.Description));
            }
        }

        private List<NotificationListModel> GetReceivers(NotificationGroupModel group)
        {
            var receivers = this.unitOfWork.NotificationListRepository.GetAllExceptDeletedByGroupId(group.Id)
                .Where(a => a.Status == (int)NotificationListStatus.NotSent)
                .ToList();
            Log.Debug("There was(were) found {0} not handled receiver(s) in {1} group.", Log.Args(receivers.Count, group.Description));

            return receivers;
        }

        private void Sleep()
        {
            var time = TimeSpan.FromMinutes(1);
            Log.Debug("Going to sleep for {0} min.", Log.Args(time.Minutes));

            Thread.Sleep(time);
        }
    }
}
