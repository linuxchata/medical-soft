using System.Windows.Input;

namespace Contracts.ViewModel
{
    /// <summary>
    /// Represents view model interface for backup.
    /// </summary>
    public interface IBackupViewModel
    {
        /// <summary>
        /// Gets run command.
        /// </summary>
        ICommand RunCommand { get; }
    }
}
