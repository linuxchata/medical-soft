using System.Windows.Input;
using Client.ViewModel;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for PositionView
    /// </summary>
    public partial class PositionView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionView"/> class.
        /// </summary>
        public PositionView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// On Position double click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnPositionDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as PositionViewModel;

            if (vm != null)
            {
                vm.EditDialogCommand.Execute(null);
            }
        }
    }
}
