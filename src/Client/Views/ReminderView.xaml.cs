using System.Windows.Input;
using Client.ViewModel;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for ReminderView
    /// </summary>
    public partial class ReminderView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderView"/> class.
        /// </summary>
        public ReminderView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// On Reminder double click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnReminderDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as ReminderViewModel;

            if (vm != null)
            {
                vm.EditDialogCommand.Execute(null);
            }
        }
    }
}
