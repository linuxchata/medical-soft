namespace Contracts
{
    /// <summary>
    /// Represent WPF Window.
    /// </summary>
    public interface IWindow
    {
        /// <summary>
        /// Gets or sets the data context for an element when it participates in data binding.
        /// </summary>
        object DataContext { get; set; }

        /// <summary>
        /// Opens a window and returns only when the newly opened window is closed.
        /// </summary>
        /// <returns>Returns value that signifies how a window was closed by the user.</returns>
        bool? ShowDialog();

        /// <summary>
        /// Manually closes a System.Windows.Window.
        /// </summary>
        void Close();
    }
}
