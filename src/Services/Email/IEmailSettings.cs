namespace Services.Email
{
    /// <summary>
    /// E-mail settings.
    /// </summary>
    public interface IEmailSettings
    {
        /// <summary>
        /// Gets a value indicating whether is Secure Sockets Layer enabled.
        /// </summary>
        bool SmtpEnableSsl { get; }

        /// <summary>
        /// Gets e-mail address.
        /// </summary>
        string SmtpFromAddress { get; }

        /// <summary>
        /// Gets user name.
        /// </summary>
        string SmtpUserName { get; }

        /// <summary>
        /// Gets password.
        /// </summary>
        string SmtpPassword { get; }

        /// <summary>
        /// Gets e-mail host.
        /// </summary>
        string SmtpHost { get; }

        /// <summary>
        /// Gets port.
        /// </summary>
        int SmtpPort { get; }

        /// <summary>
        /// Gets sending delay (time to sleep between e-mails).
        /// </summary>
        int SendingDelayInSeconds { get; }

        /// <summary>
        /// Refresh e-mail settings.
        /// </summary>
        void Refresh();
    }
}