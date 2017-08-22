using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Cache.Interface;
using Client.Contracts.Dialogs;
using Client.Providers;
using Common.Communication;
using Common.Enumeration;
using Common.ViewModel;
using DataAccess;
using Models;
using Utilities.Resource;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model for login view.
    /// </summary>
    public sealed class LoginDialogViewModel : ViewModelDialogBase2<LoginModel>, ILoginDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IResourceHandler resourceHandler;

        private readonly IMessageBoxProvider messageBoxProvider;

        private readonly IRoleCache roleCache;

        private List<LoginModel> loginList;

        private RoleModel selectedRole;

        private List<RoleModel> roles;

        private StaffModel selectedStaff;

        private List<StaffModel> staff;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="resourceHandler">Resource handler.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="roleCache">Role cache.</param>
        /// <param name="mode">Mode (Add/Edit).</param>
        /// <param name="model">Login model.</param>
        public LoginDialogViewModel(
            IUnitOfWork unitOfWork,
            IResourceHandler resourceHandler,
            IMessageBoxProvider messageBoxProvider,
            IRoleCache roleCache,
            WorkModeType mode,
            LoginModel model = null)
            : base(mode)
        {
            this.unitOfWork = unitOfWork;
            this.resourceHandler = resourceHandler;
            this.messageBoxProvider = messageBoxProvider;
            this.roleCache = roleCache;

            Task.Factory.StartNewWithDefaultCulture(() => this.Load(model));
        }

        /// <summary>
        /// Gets or sets all roles.
        /// </summary>
        public List<RoleModel> Roles
        {
            get
            {
                return this.roles;
            }

            set
            {
                this.roles = value;
                this.OnPropertyChanged(() => this.Roles);
            }
        }

        /// <summary>
        /// Gets or sets currently selected role.
        /// </summary>
        public RoleModel SelectedRole
        {
            get
            {
                return this.selectedRole;
            }

            set
            {
                this.selectedRole = value;
                this.Model.RoleInSystem = this.selectedRole.Id;
                this.Model.RoleNameInSystem = this.selectedRole.Name;
                this.OnPropertyChanged(() => this.SelectedRole);
            }
        }

        /// <summary>
        /// Gets or sets all staff.
        /// </summary>
        public List<StaffModel> Staff
        {
            get
            {
                return this.staff;
            }

            set
            {
                this.staff = value;
                this.OnPropertyChanged(() => this.Staff);
            }
        }

        /// <summary>
        /// Gets or sets currently selected staff.
        /// </summary>
        public StaffModel SelectedStaff
        {
            get
            {
                return this.selectedStaff;
            }

            set
            {
                this.selectedStaff = value;
                this.Model.StaffId = this.selectedStaff.Id;
                this.Model.StaffName = this.selectedStaff.FullName;
                this.OnPropertyChanged(() => this.SelectedStaff);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the login information is valid.
        /// </summary>
        protected override bool CanHandle
        {
            get
            {
                return this.Validate() && this.Model != null && this.Model.IsValid;
            }
        }

        /// <summary>
        /// Load information about login.
        /// </summary>
        /// <param name="loginModel">Login model.</param>
        protected override void Load(LoginModel loginModel)
        {
            this.Status = LoadingStatus.Loading;

            this.LoadModels(loginModel);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Save login.
        /// </summary>
        protected override void Handle()
        {
            this.Status = LoadingStatus.Loading;

            var response = this.SaveChanges();
            if (response.IsSuccessful)
            {
                this.UpdateIdOfAddedItem(response);
            }
            else
            {
                this.HandleFailure();
            }

            this.CloseDialog();
        }

        private void LoadModels(LoginModel loginModel)
        {
            this.loginList = this.unitOfWork.LoginRepository.GetAllIncludeNotMapped().ToList();
            this.Roles = this.roleCache.Get();
            this.Staff = this.unitOfWork.StaffRepository.GetAllExceptDeleted().ToList();

            if (this.Mode == WorkModeType.Add)
            {
                this.LoadModelsForAddItem();
            }
            else if (this.Mode == WorkModeType.Edit)
            {
                this.LoadModelsForEditItem(loginModel);
            }
        }

        private void LoadModelsForAddItem()
        {
            this.Model = new LoginModel
            {
                IsFullValidation = true
            };

            if (this.Roles.Any())
            {
                this.SelectedRole = this.Roles.First();
            }

            if (this.Staff.Any())
            {
                this.SelectedStaff = this.Staff.First();
            }
        }

        private void LoadModelsForEditItem(LoginModel loginModel)
        {
            this.Model = loginModel.Map();

            // Allow user saving login with old name, but check if the name is unique among other logins.
            this.loginList.Remove(this.loginList.Find(a => a.LoginName == this.Model.LoginName));

            this.HideLoginPassword();

            this.SelectedRole = this.Roles.Find(a => a.Id == this.Model.RoleInSystem);

            this.SelectedStaff = this.Staff.Find(a => a.Id == this.Model.StaffId);
        }

        private void HideLoginPassword()
        {
            var protectedString = this.resourceHandler.GetValue("LoginDialogPasswordProtection");
            this.Model.Password = protectedString;
            this.Model.ConfirmPassword = protectedString;
        }

        private SaveChangesResponse SaveChanges()
        {
            if (this.Mode == WorkModeType.Add)
            {
                this.AddItem();
            }
            else if (this.Mode == WorkModeType.Edit)
            {
                this.EditItem();
            }

            return this.unitOfWork.Save();
        }

        private void AddItem()
        {
            this.unitOfWork.LoginRepository.Add(this.Model);
            this.Status = LoadingStatus.Added;
        }

        private void EditItem()
        {
            if (this.Model.Password == this.resourceHandler.GetValue("LoginDialogPasswordProtection"))
            {
                this.Model.Password = string.Empty;
                this.Model.ConfirmPassword = string.Empty;
            }

            this.unitOfWork.LoginRepository.Update(this.Model);
            this.Status = LoadingStatus.Updated;
        }

        private void UpdateIdOfAddedItem(SaveChangesResponse response)
        {
            if (this.Status == LoadingStatus.Added)
            {
                this.Model.Id = response.TryGetValue(DatabaseEntity.Logins.ToString());
            }
        }

        private void HandleFailure()
        {
            this.Status = LoadingStatus.Failed;
            this.messageBoxProvider.CannotBeSavedDueToError();
        }

        private bool Validate()
        {
            var result = new List<LoginModel>();

            if (this.loginList != null && this.Model != null)
            {
                result.AddRange(this.loginList.FindAll(a => a.LoginName == this.Model.LoginName));
            }

            return !result.Any();
        }
    }
}
