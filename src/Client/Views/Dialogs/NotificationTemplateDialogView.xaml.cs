using Contracts;

namespace Client.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for NotificationTemplateDialogView
    /// </summary>
    public partial class NotificationTemplateDialogView : IWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTemplateDialogView"/> class.
        /// </summary>
        public NotificationTemplateDialogView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Data context changed event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void Window_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            var viewModel = this.DataContext as ViewModel.Dialogs.NotificationTemplateDialogViewModel;
            viewModel.WebEditor = this.WebEditor;
        }
    }
}
