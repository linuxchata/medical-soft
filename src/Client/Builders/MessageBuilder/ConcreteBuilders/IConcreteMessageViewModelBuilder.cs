using Client.ViewModel;

namespace Client.Builders.MessageBuilder.ConcreteBuilders
{
    /// <summary>
    /// Represents concrete message view model builder.
    /// </summary>
    public interface IConcreteMessageViewModelBuilder
    {
        /// <summary>
        /// Create instance of message view model.
        /// </summary>
        /// <returns>Return instance of message view model.</returns>
        MessageViewModel Create();
    }
}
