namespace Contracts.ViewModel.Base
{
    /// <summary>
    /// Represents interface for subscribing.
    /// </summary>
    public interface ISubscribable
    {
        /// <summary>
        /// Subscribe method.
        /// </summary>
        void Subscribe();

        /// <summary>
        /// Unsubscribe method.
        /// </summary>
        void Unsubscribe();
    }
}
