using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Client.Contracts.Dialogs;
using Client.Views.Dialogs;
using Common.Builder;
using Common.Enumeration;
using Logger;
using Models.Enumeration;
using Services.Backup;
using Services.Email;
using Services.Reminder;
using Services.Settings;

namespace Client.Providers
{
    /// <summary>
    /// Represents class to start startup services.
    /// </summary>
    public class StartupServicesProvider : IStartupServicesProvider
    {
        private readonly IApplicationSettings applicationSettings;

        private readonly ISettingsService settingsService;

        private readonly IViewModelBuilder viewModelBuilder;

        private readonly IEmailDeliveryService emailDeliveryService;

        private readonly IReminderService reminderService;

        private readonly IBackupService backupService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupServicesProvider"/> class.
        /// </summary>
        /// <param name="applicationSettings">Application settings.</param>
        /// <param name="settingsService">Setting service.</param>
        /// <param name="viewModelBuilder">View model builder.</param>
        /// <param name="emailDeliveryService">Email delivery service.</param>
        /// <param name="reminderService">Reminder service.</param>
        /// <param name="backupService">Backup service.</param>
        public StartupServicesProvider(
            IApplicationSettings applicationSettings,
            ISettingsService settingsService,
            IViewModelBuilder viewModelBuilder,
            IEmailDeliveryService emailDeliveryService,
            IReminderService reminderService,
            IBackupService backupService)
        {
            this.applicationSettings = applicationSettings;
            this.settingsService = settingsService;
            this.viewModelBuilder = viewModelBuilder;
            this.emailDeliveryService = emailDeliveryService;
            this.reminderService = reminderService;
            this.backupService = backupService;
        }

        /// <summary>
        /// Start startup services.
        /// </summary>
        public void StartServices()
        {
            this.StartNotifications();
            this.StartEmailNotifications();
            this.StartBackup();
        }

        private void StartNotifications()
        {
            var isNotificationOn = this.settingsService.GetBit(AvailableSettings.ReminderIsOn);
            if (isNotificationOn)
            {
                Log.Debug("Notifications is on.");

                var viewModel = this.viewModelBuilder.Build<IReminderPopupDialogViewModel>();
                var notificationsView = new ReminderPopupDialogView();
                viewModel.RequestClose += (s, e) =>
                {
                    notificationsView.Visibility = Visibility.Collapsed;
                };

                notificationsView.DataContext = viewModel;

                Log.Debug("Notification view and view model have been initialize successfully.");

                var callBack = new Action<ManualResetEvent>(manualResetEvent =>
                    notificationsView.Dispatcher.BeginInvoke(
                        DispatcherPriority.ApplicationIdle,
                        new Action(() =>
                        {
                            viewModel.SetManualResetEvent(manualResetEvent);

                            if (viewModel.CheckForActiveNotifications())
                            {
                                Log.Debug("Active notifications have been found.");
                                notificationsView.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                Log.Debug("Active notifications haven't been found.");
                                manualResetEvent.Set();
                            }
                        })));

                Log.Debug("Starting notifications.");

                this.reminderService.Start(callBack);
            }
            else
            {
                Log.Debug("Notifications is off.");
            }
        }

        private void StartEmailNotifications()
        {
            if (this.IsClientOnlyRunningMode())
            {
                Log.Info("Machine is run under client only mode. Email notification won't be executed");
                return;
            }

            var isEmailNotificationOn = this.settingsService.GetBit(AvailableSettings.EmailNotificationIsOn);
            if (isEmailNotificationOn)
            {
                Log.Debug("Starting e-mail notifications.");
                this.emailDeliveryService.Start();
            }
            else
            {
                Log.Debug("E-mail notifications is off.");
            }
        }

        private void StartBackup()
        {
            if (this.IsClientOnlyRunningMode())
            {
                Log.Info("Machine is run under client only mode. Scheduled backup won't be executed");
                return;
            }

            this.backupService.Start();
        }

        private bool IsClientOnlyRunningMode()
        {
            return this.applicationSettings.RunningMode == RunningMode.ClientOnly;
        }
    }
}