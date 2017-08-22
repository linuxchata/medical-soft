using System;

namespace Utilities.EventAggregator
{
    /// <summary>
    /// Represents interface for event aggregator.
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// Subscribe to event.
        /// </summary>
        /// <typeparam name="T">Type of the event.</typeparam>
        /// <typeparam name="TP">Type of the parameter.</typeparam>
        /// <param name="action">Action for event.</param>
        /// <param name="param">The parameter.</param>
        void Subscribe<T, TP>(Action<TP> action, TP param);

        /// <summary>
        /// Subscribe to event.
        /// </summary>
        /// <typeparam name="T">Type of the event.</typeparam>
        /// <param name="action">Action for event.</param>
        void Subscribe<T>(Action action);

        /// <summary>
        /// Unsubscribe from event.
        /// </summary>
        /// <typeparam name="T">Type of the event.</typeparam>
        void Unsubscribe<T>();

        /// <summary>
        /// Publish event.
        /// </summary>
        /// <typeparam name="T">Type of the event.</typeparam>
        void Publish<T>();
    }
}