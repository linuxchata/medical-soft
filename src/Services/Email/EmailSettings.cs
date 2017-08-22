using Models.Enumeration;
using Services.Settings;

namespace Services.Email
{
    /// <summary>
    /// Represents class that contains e-mail settings.
    /// </summary>
    public class EmailSettings : IEmailSettings
    {
        private readonly ISettingsService settingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSettings"/> class.
        /// </summary>
        /// <param name="settingsService">Setting service.</param>
        public EmailSettings(ISettingsService settingsService)
        {
            this.settingsService = settingsService;

            this.GetSmtpSettings();
        }

        /// <summary>
        /// Gets a value indicating whether is Secure Sockets Layer enabled.
        /// </summary>
        public bool SmtpEnableSsl { get; private set; }

        /// <summary>
        /// Gets e-mail address.
        /// </summary>
        public string SmtpFromAddress { get; private set; }

        /// <summary>
        /// Gets user name.
        /// </summary>
        public string SmtpUserName { get; private set; }

        /// <summary>
        /// Gets password.
        /// </summary>
        public string SmtpPassword { get; private set; }

        /// <summary>
        /// Gets e-mail host.
        /// </summary>
        public string SmtpHost { get; private set; }

        /// <summary>
        /// Gets port.
        /// </summary>
        public int SmtpPort { get; private set; }

        /// <summary>
        /// Gets sending delay (time to sleep between e-mails).
        /// </summary>
        public int SendingDelayInSeconds { get; private set; }

        /// <summary>
        /// Refresh e-mail settings.
        /// </summary>
        public void Refresh()
        {
            this.GetSmtpSettings();
        }

        private void GetSmtpSettings()
        {
            this.SmtpEnableSsl = this.settingsService.GetBit(AvailableSettings.SmtpEnableSsl);
            this.SmtpFromAddress = this.settingsService.Get(AvailableSettings.SmtpFromAddress);
            this.SmtpUserName = this.settingsService.Get(AvailableSettings.SmtpUserName);
            this.SmtpPassword = this.settingsService.Get(AvailableSettings.SmtpPassword);
            this.SmtpHost = this.settingsService.Get(AvailableSettings.SmtpHost);
            this.SmtpPort = this.settingsService.GetInt(AvailableSettings.SmtpPort);
            this.SendingDelayInSeconds = this.settingsService.GetInt(AvailableSettings.EmailSendingDelayInSeconds);
        }
    }
}
