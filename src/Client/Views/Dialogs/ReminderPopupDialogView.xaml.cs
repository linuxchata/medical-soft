using System.Windows;
using Contracts;

namespace Client.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for ReminderPopupDialogView
    /// </summary>
    public partial class ReminderPopupDialogView : IWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderPopupDialogView"/> class.
        /// </summary>
        public ReminderPopupDialogView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Windows load event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Event handler.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
            var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

            this.Left = corner.X - this.ActualWidth - 10;
            this.Top = corner.Y - this.ActualHeight - 10;
        }
    }
}
