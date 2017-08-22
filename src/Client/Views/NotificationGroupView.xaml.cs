using System.Windows.Input;
using Client.ViewModel;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for NotificationGroupView
    /// </summary>
    public partial class NotificationGroupView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationGroupView"/> class.
        /// </summary>
        public NotificationGroupView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// On Notification Group double click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnNotificationGroupDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as NotificationGroupViewModel;

            if (vm != null)
            {
                vm.EditDialogCommand.Execute(null);
            }
        }
    }
}
