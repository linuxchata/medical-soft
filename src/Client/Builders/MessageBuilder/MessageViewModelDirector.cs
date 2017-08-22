using System;
using Client.Builders.MessageBuilder.ConcreteBuilders;
using Client.ViewModel;
using Common.Enumeration;
using Microsoft.Practices.Unity;

namespace Client.Builders.MessageBuilder
{
    /// <summary>
    /// Represents director for message view model.
    /// </summary>
    public class MessageViewModelDirector : IMessageViewModelDirector
    {
        private readonly IUnityContainer unityContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageViewModelDirector"/> class.
        /// </summary>
        /// <param name="unityContainer">Unity container.</param>
        public MessageViewModelDirector(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        /// <summary>
        /// Construct instance of message view model.
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <returns>Returns instance of message view model.</returns>
        public MessageViewModel Construct(MessageType messageType)
        {
            var concreteBuilder = this.unityContainer.Resolve<IConcreteMessageViewModelBuilder>(messageType.ToString());
            if (concreteBuilder != null)
            {
                return concreteBuilder.Create();
            }

            var errorMessage = string.Format("Concrete builder of type {0} was not registered.", messageType);
            throw new InvalidOperationException(errorMessage);
        }
    }
}