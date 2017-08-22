namespace Models.Enumeration
{
    /// <summary>
    /// Represent notification list statuses.
    /// </summary>
    public enum NotificationListStatus
    {
        /// <summary>
        /// E-mail was not sent.
        /// </summary>
        NotSent = 1,

        /// <summary>
        /// E-mail was sent.
        /// </summary>
        Success = 2,

        /// <summary>
        /// E-mail send failed.
        /// </summary>
        Fail = 3
    }
}
