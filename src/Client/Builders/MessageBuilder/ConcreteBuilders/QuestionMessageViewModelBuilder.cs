using System.Windows;
using Client.ViewModel;
using Utilities.Resource;

namespace Client.Builders.MessageBuilder.ConcreteBuilders
{
    /// <summary>
    /// Represents question message view model builder.
    /// </summary>
    public class QuestionMessageViewModelBuilder : IConcreteMessageViewModelBuilder
    {
        private readonly IResourceHandler resourceHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionMessageViewModelBuilder"/> class.
        /// </summary>
        /// <param name="resourceHandler">Resource handler.</param>
        public QuestionMessageViewModelBuilder(IResourceHandler resourceHandler)
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
                OkButtonContent = this.resourceHandler.GetValue("StyledMessageBoxButtonYes"),
                CancelButtonContent = this.resourceHandler.GetValue("StyledMessageBoxButtonNo"),
                ImageSource = "/Client;component/Images/Styled_Help_48.png"
            };

            return viewModel;
        }
    }
}