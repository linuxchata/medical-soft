using System;

namespace Utilities.EventAggregator
{
    /// <summary>
    /// Represents class for subscriber.
    /// </summary>
    /// <typeparam name="T">Type if the event.</typeparam>
    /// <typeparam name="TP">Type of the parameter.</typeparam>
    internal sealed class Subscriber<T, TP> : SubscriberBase
    {
        private readonly T action;

        private readonly TP parameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscriber{T,TP}"/> class.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        public Subscriber(T action)
        {
            this.action = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscriber{T,TP}"/> class.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="param">Action's parameter.</param>
        public Subscriber(T action, TP param)
        {
            this.action = action;
            this.parameter = param;
        }

        /// <summary>
        /// Execute action.
        /// </summary>
        public override void ExecuteAction()
        {
            var act = this.action as Delegate;

            if (act != null)
            {
                if (this.parameter != null)
                {
                    act.DynamicInvoke(this.parameter);
                }
                else
                {
                    act.DynamicInvoke();
                }
            }
        }
    }
}