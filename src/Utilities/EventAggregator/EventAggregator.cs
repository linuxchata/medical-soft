using System;
using System.Collections.Generic;
using Logger;

namespace Utilities.EventAggregator
{
    /// <summary>
    /// Represents event aggregator.
    /// </summary>
    public sealed class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, List<SubscriberBase>> listOfSubscribes;

        private readonly object sync;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregator"/> class.
        /// </summary>
        public EventAggregator()
        {
            var hashCode = Guid.NewGuid();

            this.sync = new object();

            this.listOfSubscribes = new Dictionary<Type, List<SubscriberBase>>();

            Log.Debug("Event aggregator was initialized. Hash code {0}", Log.Args(hashCode));
        }

        /// <summary>
        /// Subscribe to event.
        /// </summary>
        /// <typeparam name="T">Type of the event.</typeparam>
        /// <param name="action">Action for event.</param>
        public void Subscribe<T>(Action action)
        {
            var subscriber = new Subscriber<object, object>(action);

            this.AddSubscriber(subscriber, this.GetEventType<T>());
        }

        /// <summary>
        /// Subscribe to event.
        /// </summary>
        /// <typeparam name="T">Type of the event.</typeparam>
        /// <typeparam name="TP">Type of the parameter.</typeparam>
        /// <param name="action">Action for event.</param>
        /// <param name="param">The parameter.</param>
        public void Subscribe<T, TP>(Action<TP> action, TP param)
        {
            var subscriber = new Subscriber<Action<TP>, TP>(action, param);

            this.AddSubscriber(subscriber, this.GetEventType<T>());
        }

        /// <summary>
        /// Unsubscribe from event.
        /// </summary>
        /// <typeparam name="T">Type of the event.</typeparam>
        public void Unsubscribe<T>()
        {
            var type = this.GetEventType<T>();

            lock (this.sync)
            {
                if (this.listOfSubscribes.ContainsKey(type))
                {
                    this.listOfSubscribes.Remove(type);

                    Log.Debug("Unsubscribe from event {0}.", Log.Args(type));
                }
            }
        }

        /// <summary>
        /// Publish event.
        /// </summary>
        /// <typeparam name="T">Type of the event.</typeparam>
        public void Publish<T>()
        {
            var type = this.GetEventType<T>();

            Log.Debug("Publish event {0}.", Log.Args(type));

            lock (this.sync)
            {
                if (this.listOfSubscribes.ContainsKey(type))
                {
                    var actions = this.listOfSubscribes[type];

                    Log.Debug("Count of subscribes {0}.", Log.Args(actions.Count));

                    foreach (var action in actions)
                    {
                        action.ExecuteAction();
                    }
                }
            }
        }

        private Type GetEventType<T>()
        {
            var type = typeof(T);

            var isAggregationEvent = typeof(IAggregationEvent).IsAssignableFrom(type);

            if (isAggregationEvent)
            {
                return type;
            }

            throw new ArgumentException(@"Aggregation event has to implement IAggregationEvent interface.", type.ToString());
        }

        private void AddSubscriber(SubscriberBase subscriber, Type type)
        {
            lock (this.sync)
            {
                if (this.listOfSubscribes.ContainsKey(type))
                {
                    this.listOfSubscribes[type].Add(subscriber);

                    Log.Debug("Add subscriber for the event {0}.", Log.Args(type));
                }
                else
                {
                    this.listOfSubscribes.Add(
                        type,
                        new List<SubscriberBase>
                        {
                            subscriber
                        });

                    Log.Debug("Add a new event {0}.", Log.Args(type));
                }
            }
        }
    }
}