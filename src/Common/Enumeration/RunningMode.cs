namespace Common.Enumeration
{
    /// <summary>
    /// Represents running mode of the application (Server or Client Only)
    /// </summary>
    public enum RunningMode : byte
    {
        /// <summary>
        /// Application is under server mode.
        /// Scheduled backup, notification will be run on that machine.
        /// </summary>
        Server = 1,

        /// <summary>
        /// Application is under client only mode.
        /// </summary>
        ClientOnly = 2
    }
}
