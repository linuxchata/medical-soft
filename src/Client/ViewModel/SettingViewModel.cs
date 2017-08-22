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
    /// Represents view model for setting.
    /// </summary>
    public sealed class SettingViewModel : ViewModelNotifyBase, ISettingViewModel
    {
        private readonly IViewBuilder viewBuilder;

        private ICommand showCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel"/> class.
        /// </summary>
        /// <param name="viewBuilder">View builder.</param>
        public SettingViewModel(IViewBuilder viewBuilder)
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
                    param => this.ShowSetting(),
                    param => true));
            }
        }

        private void ShowSetting()
        {
            this.viewBuilder.Build<SettingDialogView, ISettingDialogViewModel>().ShowDialog();
        }
    }
}
