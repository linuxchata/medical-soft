using System.Windows.Input;
using Client.Contracts.Dialogs;
using Client.Views.Dialogs;
using Common.Builder;
using Common.Commands;
using Common.ViewModel;
using Contracts.ViewModel;

namespace Client.ViewModel
{
    /// <summary>
    /// Represents view model for about information.
    /// </summary>
    public sealed class AboutViewModel : ViewModelNotifyBase, IAboutViewModel
    {
        private readonly IViewBuilder viewBuilder;

        private ICommand showCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutViewModel"/> class.
        /// </summary>
        /// <param name="viewBuilder">View builder.</param>
        public AboutViewModel(IViewBuilder viewBuilder)
        {
            this.viewBuilder = viewBuilder;
        }

        /// <summary>
        /// Gets show command.
        /// </summary>
        public ICommand ShowCommand
        {
            get
            {
                return this.showCommand ?? (this.showCommand = new CommonCommand(
                    param => this.ShowAboutbox(),
                    param => true));
            }
        }

        private void ShowAboutbox()
        {
            this.viewBuilder.Build<AboutDialogView, IAboutDialogViewModel>().ShowDialog();
        }
    }
}
