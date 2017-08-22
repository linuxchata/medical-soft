using System;

namespace Contracts.ViewModel
{
    /// <summary>
    /// Represents close event.
    /// </summary>
    public interface IRequestCloseViewModel
    {
        /// <summary>
        /// Close event.
        /// </summary>
        event EventHandler RequestClose;
    }
}
