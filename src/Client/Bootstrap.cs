using System;
using Client.Builders.MessageBuilder;
using Client.Builders.MessageBuilder.ConcreteBuilders;
using Client.Cache;
using Client.Cache.Interface;
using Client.Contracts;
using Client.Contracts.Dialogs;
using Client.Providers;
using Client.ViewModel;
using Client.ViewModel.Dialogs;
using Client.ViewModel.UserControls;
using Common.Authentication;
using Common.Builder;
using Common.Enumeration;
using Contracts;
using Contracts.ViewModel;
using Contracts.ViewModel.Dialogs;
using Contracts.ViewModel.UserControls;
using DataAccess;
using Microsoft.Practices.Unity;
using Models;
using Models.Enumeration;
using Services.Backup;
using Services.Email;
using Services.Reminder;
using Services.Settings;
using Utilities.EventAggregator;
using Utilities.Resource;

namespace Client
{
    /// <summary>
    /// Represents initialization of the unity container.
    /// </summary>
    public class Bootstrap : IDisposable
    {
        public readonly IUnityContainer Container;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrap"/> class.
        /// </summary>
        public Bootstrap()
        {
            this.Container = new UnityContainer();
        }

        /// <summary>
        /// Register types.
        /// </summary>
        public void Register()
        {
            this.Container.RegisterType<IUnitOfWork, UnitOfWork>();

            this.Container.RegisterInstance<IAuthenticationSession>(AuthenticationSession.Instance);

            IEventAggregator eventAggregator = new EventAggregator();
            this.Container.RegisterInstance(eventAggregator);

            this.Container.RegisterType<IApplicationSettings, ApplicationSettings>();

            this.Container.RegisterType<IAppointmentRepository<AppointmentModel, Guid>, AppointmentRepository>();
            this.Container.RegisterType<IBackupRepository<BackupLogsModel, int>, BackupRepository>();
            this.Container.RegisterType<IEducationRepository<EducationModel, int>, EducationRepository>();
            this.Container.RegisterType<ILoginRepository<LoginModel, int>, LoginRepository>();
            this.Container.RegisterType<INotificationGroupRepository<NotificationGroupModel, int>, NotificationGroupRepository>();
            this.Container.RegisterType<INotificationListRepository<NotificationListModel, int>, NotificationListRepository>();
            this.Container.RegisterType<INotificationListStatusRepository<NotificationListStatusModel>, NotificationListStatusRepository>();
            this.Container.RegisterType<INotificationTemplateRepository<NotificationTemplateModel, ItemModel, int>, NotificationTemplateRepository>();
            this.Container.RegisterType<IPatientRepository<PatientModel, ItemModel, int>, PatientRepository>();
            this.Container.RegisterType<IPositionRepository<PositionModel, int>, PositionRepository>();
            this.Container.RegisterType<IRoleRepository<RoleModel, int>, RoleRepository>();
            this.Container.RegisterType<ISettingRepository<SettingModel, AvailableSettings>, SettingRepository>();
            this.Container.RegisterType<IGenderRepository<GenderModel>, GenderRepository>();
            this.Container.RegisterType<IStaffRepository<StaffModel, ItemModel, int>, StaffRepository>();
            this.Container.RegisterType<ISystemUpdatesRepository<SystemUpdatesModel>, SystemUpdatesRepository>();
            this.Container.RegisterType<IReminderRepository<ReminderModel, int>, ReminderRepository>();
            this.Container.RegisterType<IReminderFilterRepository<ReminderFilterModel>, ReminderFilterRepository>();
            this.Container.RegisterType<IReminderAlertRepository<ReminderAlertModel>, ReminderAlertRepository>();
            this.Container.RegisterType<IUiCultureRepository<CultureModel>, UiCultureRepository>();

            this.Container.RegisterType<IResourceHandler, ResourceHandler<Properties.Resources>>();

            this.Container.RegisterType<IMessageViewModelDirector, MessageViewModelDirector>();
            this.Container.RegisterType<IConcreteMessageViewModelBuilder, InformationMessageViewModelBuilder>(MessageType.Information.ToString());
            this.Container.RegisterType<IConcreteMessageViewModelBuilder, QuestionMessageViewModelBuilder>(MessageType.Question.ToString());
            this.Container.RegisterType<IConcreteMessageViewModelBuilder, WarningMessageViewModelBuilder>(MessageType.Warning.ToString());
            this.Container.RegisterType<IConcreteMessageViewModelBuilder, ErrorMessageViewModelBuilder>(MessageType.Error.ToString());
            this.Container.RegisterType<IMessageBoxProvider, MessageBoxProvider>();

            this.Container.RegisterType<IStartupServicesProvider, StartupServicesProvider>();

            this.Container.RegisterType<IGenderCache, GenderCache>();
            this.Container.RegisterType<IPositionCache, PositionCache>();
            this.Container.RegisterType<IEducationCache, EducationCache>();
            this.Container.RegisterType<IReminderAlertCache, ReminderAlertCache>();
            this.Container.RegisterType<IRoleCache, RoleCache>();
            this.Container.RegisterType<ICultureCache, CultureCache>();

            this.Container.RegisterType<IViewBuilder, ViewBuilder>();
            this.Container.RegisterType<IViewModelBuilder, ViewModelBuilder>();

            this.Container.RegisterType<ISettingsService, SettingsService>();

            this.Container.RegisterType<IBackupService, BackupService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IReminderService, ReminderService>(new ContainerControlledLifetimeManager());

            this.Container.RegisterType<IEmailDeliveryService, EmailDeliveryService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IEmailSender, EmailSender>();
            this.Container.RegisterType<IEmailSettings, EmailSettings>();
            this.Container.RegisterType<IBackuper, Backuper>();

            this.Container.RegisterType<Views.Dialogs.LoginDialog>();
            this.Container.RegisterType<ILoginingDialogViewModel, LoginingDialogViewModel>();

            this.Container.RegisterType<MainWindow>();
            this.Container.RegisterType<MainViewModel>();

            this.Container.RegisterType<INavigationsPanelViewModel, NavigationsPanelViewModel>();

            this.Container.RegisterType<IPatientViewModel, PatientViewModel>();
            this.Container.RegisterType<IPatientDialogViewModel, PatientDialogViewModel>();
            this.Container.RegisterType<IPreviewDialogViewModel, PreviewDialogViewModel>();

            this.Container.RegisterType<IStaffViewModel, StaffViewModel>();
            this.Container.RegisterType<IStaffDialogViewModel, StaffDialogViewModel>();

            this.Container.RegisterType<IReminderViewModel, ReminderViewModel>();
            this.Container.RegisterType<IReminderDialogViewModel, ReminderDialogViewModel>();
            this.Container.RegisterType<IReminderPopupDialogViewModel, ReminderPopupDialogViewModel>();

            this.Container.RegisterType<INotificationTemplateViewModel, NotificationTemplateViewModel>();
            this.Container.RegisterType<INotificationTemplateDialogViewModel, NotificationTemplateDialogViewModel>();

            this.Container.RegisterType<INotificationGroupViewModel, NotificationGroupViewModel>();
            this.Container.RegisterType<INotificationGroupDialogViewModel, NotificationGroupDialogViewModel>();
            this.Container.RegisterType<INotificationListDialogViewModel, NotificationListDialogViewModel>();
            this.Container.RegisterType<INotificationListManageDialogViewModel, NotificationListManageDialogViewModel>();
            this.Container.RegisterType<INotificationInValidateDialogEmailViewModel, NotificationInValidateDialogEmailViewModel>();

            this.Container.RegisterType<IAppointmentViewModel, AppointmentViewModel>();
            this.Container.RegisterType<IAppointmentDialogViewModel, AppointmentDialogViewModel>();

            this.Container.RegisterType<ILoginViewModel, LoginViewModel>();
            this.Container.RegisterType<ILoginDialogViewModel, LoginDialogViewModel>();

            this.Container.RegisterType<IPositionViewModel, PositionViewModel>();
            this.Container.RegisterType<IPositionDialogViewModel, PositionDialogViewModel>();

            this.Container.RegisterType<IEducationViewModel, EducationViewModel>();
            this.Container.RegisterType<IEducationDialogViewModel, EducationDialogViewModel>();

            this.Container.RegisterType<IBackupViewModel, BackupViewModel>();

            this.Container.RegisterType<IAboutViewModel, AboutViewModel>();
            this.Container.RegisterType<IAboutDialogViewModel, AboutDialogViewModel>();

            this.Container.RegisterType<ISettingViewModel, SettingViewModel>();
            this.Container.RegisterType<ISettingDialogViewModel, SettingDialogViewModel>();
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        /// <param name="disposing">Define whether managed objects have to be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.Container.Dispose();
            }

            this.disposed = true;
        }
    }
}
