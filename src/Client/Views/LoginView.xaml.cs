using System.Windows.Input;
using Client.ViewModel;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for LoginView
    /// </summary>
    public partial class LoginView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginView"/> class.
        /// </summary>
        public LoginView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// On Login double click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnLoginDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as LoginViewModel;

            if (vm != null)
            {
                vm.EditDialogCommand.Execute(null);
            }
        }
    }
}
