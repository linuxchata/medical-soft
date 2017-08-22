using System.Windows;
using Client.ViewModel;
using Utilities.Resource;

namespace Client.Builders.MessageBuilder.ConcreteBuilders
{
    /// <summary>
    /// Represents default message view model builder.
    /// </summary>
    public class DefaultMessageViewModelBuilder : IConcreteMessageViewModelBuilder
    {
        private readonly IResourceHandler resourceHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMessageViewModelBuilder"/> class.
        /// </summary>
        /// <param name="resourceHandler">Resource handler.</param>
        public DefaultMessageViewModelBuilder(IResourceHandler resourceHandler)
        {
            this.resourceHandler = resourceHandler;
        }

        /// <summary>
        /// Create instance of message view model.
        /// </summary>
        /// <returns>Return instance of message view model.</returns>
        public MessageViewModel Create()
        {
            var viewModel = new MessageViewModel
            {
                OkButtonVisibility = Visibility.Visible,
                CancelButtonVisibility = Visibility.Visible,
                OkButtonContent = this.resourceHandler.GetValue("StyledMessageBoxButtonOk"),
                CancelButtonContent = this.resourceHandler.GetValue("StyledMessageBoxButtonCancel")
            };

            return viewModel;
        }
    }
}