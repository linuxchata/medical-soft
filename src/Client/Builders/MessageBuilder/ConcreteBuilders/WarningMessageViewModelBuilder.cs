using System.Windows;
using Client.ViewModel;
using Utilities.Resource;

namespace Client.Builders.MessageBuilder.ConcreteBuilders
{
    /// <summary>
    /// Represents warning message view model builder.
    /// </summary>
    public class WarningMessageViewModelBuilder : IConcreteMessageViewModelBuilder
    {
        private readonly IResourceHandler resourceHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="WarningMessageViewModelBuilder"/> class.
        /// </summary>
        /// <param name="resourceHandler">Resource handler.</param>
        public WarningMessageViewModelBuilder(IResourceHandler resourceHandler)
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
                CancelButtonVisibility = Visibility.Collapsed,
                OkButtonContent = this.resourceHandler.GetValue("StyledMessageBoxButtonOk"),
                ImageSource = "/Client;component/Images/Styled_Warning_48.png"
            };

            return viewModel;
        }
    }
}