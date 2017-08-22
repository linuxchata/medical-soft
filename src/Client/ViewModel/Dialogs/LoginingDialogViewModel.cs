using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Client.Contracts.Dialogs;
using Common.Authentication;
using Common.Builder;
using Common.ViewModel;
using DataAccess;
using Logger;
using Models;
using Utilities.Resource;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model for login view.
    /// </summary>
    public sealed class LoginingDialogViewModel : ViewModelDialogBase2<LoginModel>, ILoginingDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IAuthenticationSession authenticationSession;

        private readonly IViewBuilder viewBuilder;

        private readonly IResourceHandler resourceHandler;

        private string loginMessage;

        private bool isLoginEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginingDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="authenticationSession">Authentication session.</param>
        /// <param name="viewBuilder">View model builder.</param>
        /// <param name="resourceHandler">Resource handler.</param>
        public LoginingDialogViewModel(
            IUnitOfWork unitOfWork,
            IAuthenticationSession authenticationSession,
            IViewBuilder viewBuilder,
            IResourceHandler resourceHandler)
        {
            this.unitOfWork = unitOfWork;
            this.authenticationSession = authenticationSession;
            this.viewBuilder = viewBuilder;
            this.resourceHandler = resourceHandler;

            this.Model = new LoginModel
            {
                IsFullValidation = false
            };

            this.isLoginEnabled = true;
        }

        /// <summary> 
        /// Gets or sets login message.
        /// </summary>
        public string LoginMessage
        {
            get
            {
                return this.loginMessage;
            }

            set
            {
                this.loginMessage = value;
                this.OnPropertyChanged(() => this.LoginMessage);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the login is enabled.
        /// </summary>
        public bool IsLoginEnabled
        {
            get
            {
                return this.isLoginEnabled;
            }

            set
            {
                this.isLoginEnabled = value;
                this.OnPropertyChanged(() => this.IsLoginEnabled);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the handle command is enabled.
        /// </summary>
        protected override bool CanHandle
        {
            get
            {
                return this.IsLoginEnabled && this.Model.IsValid;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the cancel command is enabled.
        /// </summary>
        protected override bool CanCancel
        {
            get
            {
                return this.IsLoginEnabled;
            }
        }

        /// <summary>
        /// Login logic.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected override void Handle(object parameter)
        {
            try
            {
                this.LoginMessage = string.Empty;
                this.IsLoginEnabled = false;

                var loginId = -1;
                if (this.CheckLoginAndPassword(parameter, ref loginId))
                {
                    this.HandleSuccessfulLogin();

                    this.CreateSession(loginId);

                    this.CreateAndStartTaskForMainView();
                }
                else
                {
                    this.HandleLoginFailure();
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                this.IsLoginEnabled = true;
            }
        }

        private bool CheckLoginAndPassword(object parameter, ref int loginId)
        {
            Log.Debug("Checking password.");

            bool isLoginValid;
            var password = (PasswordBox)parameter;

            using (var ss = new SecureString())
            {
                var chars = password.Password.ToList();

                foreach (var t in chars)
                {
                    ss.AppendChar(t);
                }

                chars.Clear();

                isLoginValid = this.unitOfWork.LoginRepository.IsLoginValid(this.Model.LoginName, ss, ref loginId);
            }

            Log.Debug("Result of the password checking was received.");

            return isLoginValid;
        }

        private void HandleSuccessfulLogin()
        {
            this.LoginMessage = this.resourceHandler.GetValue("LoginDialogLogonSucceeded");
        }

        private void CreateSession(int loginId)
        {
            this.authenticationSession.CreateSession(this.Model.LoginName, loginId, true, DateTime.Now);
            Log.Debug("Session was created.");
        }

        private void CreateAndStartTaskForMainView()
        {
            var task = new Task(() => Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.ApplicationIdle,
                new Action(this.CreateMainView)));

            Log.Debug("Task to show main view was created.");

            task.Start();
        }

        private void CreateMainView()
        {
            var mainWindow = this.viewBuilder.Build<MainWindow>();
            Application.Current.MainWindow = mainWindow;
            Application.Current.MainWindow.Show();
            this.OnRequestClose();
        }

        private void HandleLoginFailure()
        {
            this.LoginMessage = this.resourceHandler.GetValue("LoginDialogLogonFailed");
            this.IsLoginEnabled = true;
        }
    }
}