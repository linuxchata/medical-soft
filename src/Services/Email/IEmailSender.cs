using System.Collections.Generic;
using Models;

namespace Services.Email
{
    /// <summary>
    /// Allows to send e-mail by using the Simple Mail Transfer Protocol (SMTP).
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sets email delivery service.
        /// </summary>
        IEmailDeliveryService EmailDeliveryService { set; }

        /// <summary>
        /// Send e-mails.
        /// </summary>
        /// <param name="receivers">List of the receiver to send e-mail notification.</param>
        /// <param name="group">Notification group.</param>
        void SendEmail(IList<NotificationListModel> receivers, NotificationGroupModel group);
    }
}
