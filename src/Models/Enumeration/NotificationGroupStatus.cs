namespace Models.Enumeration
{
    /// <summary>
    /// Represent notification group statuses.
    /// </summary>
    public enum NotificationGroupStatus
    {
        /// <summary>
        /// The group was not processed.
        /// </summary>
        NotProcessed = 1,

        /// <summary>
        /// The group is processing.
        /// </summary>
        Processing = 2,

        /// <summary>
        /// The group was canceled.
        /// </summary>
        Cancelled = 3,

        /// <summary>
        /// The group was processed.
        /// </summary>
        Processed = 4
    }
}
