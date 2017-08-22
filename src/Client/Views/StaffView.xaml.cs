using System.Windows.Input;
using Client.ViewModel;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for StaffView
    /// </summary>
    public partial class StaffView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaffView"/> class.
        /// </summary>
        public StaffView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// On Staff double click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnStaffDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as StaffViewModel;

            if (vm != null)
            {
                vm.EditDialogCommand.Execute(null);
            }
        }
    }
}
