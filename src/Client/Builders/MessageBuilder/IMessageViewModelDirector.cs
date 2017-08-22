using Client.ViewModel;
using Common.Enumeration;

namespace Client.Builders.MessageBuilder
{
    /// <summary>
    /// Represents director for message view model.
    /// </summary>
    public interface IMessageViewModelDirector
    {
        /// <summary>
        /// Construct instance of message view model.
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <returns>Returns instance of message view model.</returns>
        MessageViewModel Construct(MessageType messageType);
    }
}