using System.Windows.Input;

namespace Contracts.ViewModel
{
    /// <summary>
    /// Represents view model interface for about box.
    /// </summary>
    public interface IAboutViewModel
    {
        /// <summary>
        /// Gets show command.
        /// </summary>
        ICommand ShowCommand { get; }
    }
}
