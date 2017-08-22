using System.Windows;
using System.Windows.Input;
using Client.ViewModel.Dialogs;
using Contracts;
using Microsoft.Practices.Unity;

namespace Client.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for LoginDialog
    /// </summary>
    public partial class LoginDialog : IWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginDialog"/> class.
        /// </summary>
        /// <param name="container">Unity container.</param>
        public LoginDialog(IUnityContainer container)
        {
            this.InitializeComponent();

            var loginViewModel = container.Resolve<LoginingDialogViewModel>();
            loginViewModel.RequestClose += (s, e) => this.Close();
            this.DataContext = loginViewModel;
        }

        /// <summary>
        /// On key down event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Event argument.</param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.Toggled)
            {
                this.LoginToolTip.PlacementTarget = this.TbPassword;
                this.LoginToolTip.IsOpen = true;
                this.LoginToolTip.Visibility = Visibility.Visible;
            }
            else
            {
                this.LoginToolTip.IsOpen = false;
                this.LoginToolTip.Visibility = Visibility.Collapsed;
            }
        }
    }
}
