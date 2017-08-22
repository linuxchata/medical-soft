using System.Windows.Input;
using Client.ViewModel;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for NotificationTemplateView
    /// </summary>
    public partial class NotificationTemplateView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTemplateView"/> class.
        /// </summary>
        public NotificationTemplateView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// On Notification Template double click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnNotificationTemplateDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as NotificationTemplateViewModel;

            if (vm != null)
            {
                vm.EditDialogCommand.Execute(null);
            }
        }
    }
}
