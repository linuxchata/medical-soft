using System.Windows.Input;

namespace Contracts.ViewModel
{
    /// <summary>
    /// Represents view model interface for settings.
    /// </summary>
    public interface ISettingViewModel
    {
        /// <summary>
        /// Gets show command.
        /// </summary>
        ICommand ShowCommand { get; }
    }
}
