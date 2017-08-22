namespace Utilities.EventAggregator
{
    /// <summary>
    /// Represents base class for subscriber.
    /// </summary>
    internal abstract class SubscriberBase
    {
        /// <summary>
        /// Execute action.
        /// </summary>
        public abstract void ExecuteAction();
    }
}