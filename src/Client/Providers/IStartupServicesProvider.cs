namespace Client.Providers
{
    /// <summary>
    /// Represents interface to start startup services.
    /// </summary>
    public interface IStartupServicesProvider
    {
        /// <summary>
        /// Start startup services.
        /// </summary>
        void StartServices();
    }
}