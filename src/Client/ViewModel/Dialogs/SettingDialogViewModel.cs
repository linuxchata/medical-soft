using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Cache.Interface;
using Client.Contracts.Dialogs;
using Client.Providers;
using Common.Enumeration;
using Common.ViewModel;
using DataAccess;
using Models;
using Models.Enumeration;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model for setting view.
    /// </summary>
    public sealed class SettingDialogViewModel : ViewModelDialogBase2<SettingCollectionModel>, ISettingDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMessageBoxProvider messageBoxProvider;

        private readonly IApplicationSettings applicationSettings;

        private readonly ICultureCache cultureCache;

        private List<CultureModel> cultures;

        private List<int> durations;

        private CultureModel selectedCulture;

        private List<string> hours;

        private List<string> minutes;

        private List<string> videoDevices;

        private string selectedBackupHour;

        private string selectedBackupMinute;

        private int selectedReminderCheckDelay;

        private string selectedVideoDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="applicationSettings">Application settings.</param>
        /// <param name="cultureCache">Culture cache.</param>
        public SettingDialogViewModel(
            IUnitOfWork unitOfWork,
            IMessageBoxProvider messageBoxProvider,
            IApplicationSettings applicationSettings,
            ICultureCache cultureCache)
        {
            this.unitOfWork = unitOfWork;
            this.messageBoxProvider = messageBoxProvider;
            this.applicationSettings = applicationSettings;
            this.cultureCache = cultureCache;

            Task.Factory.StartNewWithDefaultCulture(() => this.Load(null));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="applicationSettings">Application settings.</param>
        /// <param name="cultureCache">Culture cache.</param>
        /// <param name="model">The model.</param>
        public SettingDialogViewModel(
            IUnitOfWork unitOfWork,
            IMessageBoxProvider messageBoxProvider,
            IApplicationSettings applicationSettings,
            ICultureCache cultureCache,
            SettingCollectionModel model = null)
        {
            this.unitOfWork = unitOfWork;
            this.messageBoxProvider = messageBoxProvider;
            this.applicationSettings = applicationSettings;
            this.cultureCache = cultureCache;

            Task.Factory.StartNewWithDefaultCulture(() => this.Load(model));
        }

        /// <summary>
        /// Gets or sets list of the cultures.
        /// </summary>
        public List<CultureModel> Cultures
        {
            get
            {
                return this.cultures;
            }

            set
            {
                this.cultures = value;
                this.OnPropertyChanged(() => this.Cultures);
            }
        }

        /// <summary>
        /// Gets or sets list of the hours.
        /// </summary>
        public List<string> Hours
        {
            get
            {
                return this.hours;
            }

            set
            {
                this.hours = value;
                this.OnPropertyChanged(() => this.Hours);
            }
        }

        /// <summary>
        /// Gets or sets list of the minutes.
        /// </summary>
        public List<string> Minutes
        {
            get
            {
                return this.minutes;
            }

            set
            {
                this.minutes = value;
                this.OnPropertyChanged(() => this.Minutes);
            }
        }

        /// <summary>
        /// Gets list of the duration.
        /// </summary>
        public List<int> Durations
        {
            get
            {
                if (this.durations == null)
                {
                    this.GetDurations();
                }

                return this.durations;
            }
        }

        /// <summary>
        /// Gets or sets list of the video devices.
        /// </summary>
        public List<string> VideoDevices
        {
            get
            {
                return this.videoDevices;
            }

            set
            {
                this.videoDevices = value;
                this.OnPropertyChanged(() => this.VideoDevices);
            }
        }

        /// <summary>
        /// Gets or sets selected backup hours.
        /// </summary>
        public string SelectedBackupHour
        {
            get
            {
                return this.selectedBackupHour;
            }

            set
            {
                this.selectedBackupHour = value;
                this.Model.BackupHour.NvValue = this.selectedBackupHour;
                this.OnPropertyChanged(() => this.SelectedBackupHour);
            }
        }

        /// <summary>
        /// Gets or sets selected backup minutes.
        /// </summary>
        public string SelectedBackupMinute
        {
            get
            {
                return this.selectedBackupMinute;
            }

            set
            {
                this.selectedBackupMinute = value;
                this.Model.BackupMinute.NvValue = this.selectedBackupMinute;
                this.OnPropertyChanged(() => this.SelectedBackupMinute);
            }
        }

        /// <summary>
        /// Gets or sets currently selected culture.
        /// </summary>
        public CultureModel SelectedCulture
        {
            get
            {
                return this.selectedCulture;
            }

            set
            {
                this.selectedCulture = value;
                this.Model.Language.NvValue = this.selectedCulture.Name;
                this.OnPropertyChanged(() => this.SelectedCulture);
            }
        }

        /// <summary>
        /// Gets or sets currently duration between showing the same notification on UI.
        /// </summary>
        public int SelectedReminderCheckDelay
        {
            get
            {
                return this.selectedReminderCheckDelay;
            }

            set
            {
                this.selectedReminderCheckDelay = value;
                this.Model.ReminderCheckDelay.IntValue = this.selectedReminderCheckDelay;
                this.OnPropertyChanged(() => this.SelectedReminderCheckDelay);
            }
        }

        /// <summary>
        /// Gets or sets currently selected video device.
        /// </summary>
        public string SelectedVideoDevice
        {
            get
            {
                return this.selectedVideoDevice;
            }

            set
            {
                this.selectedVideoDevice = value;
                this.Model.VideoDevice = this.selectedVideoDevice;
                this.OnPropertyChanged(() => this.SelectedVideoDevice);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the settings information is valid.
        /// </summary>
        protected override bool CanHandle
        {
            get
            {
                return this.Model != null &&
                    this.Model.Language != null && this.Model.Language.IsValid &&
                    this.Model.BackupLocation != null && this.Model.BackupLocation.IsValid &&
                    this.Model.BackupDatabaseName != null && this.Model.BackupDatabaseName.IsValid &&
                    this.Model.BackupFileName != null && this.Model.BackupFileName.IsValid;
            }
        }

        /// <summary>
        /// Load information about settings.
        /// </summary>
        /// <param name="settingCollectionModel">Settings model.</param>
        protected override void Load(SettingCollectionModel settingCollectionModel)
        {
            this.Status = LoadingStatus.Loading;

            this.LoadModels();

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Save login logic.
        /// </summary>
        protected override void Handle()
        {
            if (this.Model.IsValid)
            {
                this.Status = LoadingStatus.Loading;

                this.SaveChanges();

                this.CloseDialog();
            }
            else
            {
                this.HandleInvalidModel();
            }
        }

        private void GetDurations()
        {
            this.durations = new List<int>();

            const int Init = 15;
            const int Count = 4;

            for (var i = Init; i <= Init * Count; i += Init)
            {
                this.durations.Add(i);
            }
        }

        private void LoadModels()
        {
            this.Hours = TimeHelper.Hours;
            this.Minutes = TimeHelper.Minutes;

            this.Model = new SettingCollectionModel();

            var settings = this.unitOfWork.SettingRepository.GetAll().ToList();

            this.SetLanguageSettings(settings);

            this.SetBackupSettings(settings);

            this.SetReminderSettings(settings);

            this.SetSmtpSettings(settings);

            this.SetVideoSettings();
        }

        private void SetLanguageSettings(List<SettingModel> settings)
        {
            this.Cultures = this.cultureCache.Get();
            this.Model.Language = settings.FirstOrDefault(a => a.NvKey == AvailableSettings.Language.ToString());
            this.SelectedCulture = this.Cultures.FirstOrDefault(a => a.Name == this.Model.Language.NvValue);
        }

        private void SetBackupSettings(List<SettingModel> settings)
        {
            this.Model.BackupDatabaseName = settings.FirstOrDefault(a => a.NvKey == AvailableSettings.BackupDatabaseName.ToString());
            this.Model.BackupLocation = settings.FirstOrDefault(a => a.NvKey == AvailableSettings.BackupLocation.ToString());
            this.Model.BackupFileName = settings.FirstOrDefault(a => a.NvKey == AvailableSettings.BackupFileName.ToString());
            this.Model.BackupHour = settings.FirstOrDefault(a => a.NvKey == AvailableSettings.BackupHour.ToString());
            this.Model.BackupMinute = settings.FirstOrDefault(a => a.NvKey == AvailableSettings.BackupMinute.ToString());
            this.SelectedBackupHour = this.Hours.FirstOrDefault(a => string.Equals(a, this.Model.BackupHour.NvValue));
            this.SelectedBackupMinute = this.Minutes.FirstOrDefault(a => string.Equals(a, this.Model.BackupMinute.NvValue));
        }

        private void SetReminderSettings(List<SettingModel> settings)
        {
            this.Model.ReminderCheckDelay = settings.FirstOrDefault(a => a.NvKey == AvailableSettings.ReminderCheckDelay.ToString());
            this.SelectedReminderCheckDelay = this.Durations.FirstOrDefault(a => a == this.Model.ReminderCheckDelay.IntValue);
        }

        private void SetSmtpSettings(List<SettingModel> settings)
        {
            this.Model.SmtpEnableSsl = settings.FirstOrDefault(a => a.NvKey == AvailableSettings.SmtpEnableSsl.ToString());
            this.Model.SmtpFromAddress = settings.FirstOrDefault(a => a.NvKey == AvailableSettings.SmtpFromAddress.ToString());
            this.Model.SmtpHost = settings.FirstOrDefault(a => a.NvKey == AvailableSettings.SmtpHost.ToString());
            this.Model.SmtpPassword = settings.FirstOrDefault(a => a.NvKey == AvailableSettings.SmtpPassword.ToString());
            this.Model.SmtpPort = settings.FirstOrDefault(a => a.NvKey == AvailableSettings.SmtpPort.ToString());
            this.Model.SmtpUserName = settings.FirstOrDefault(a => a.NvKey == AvailableSettings.SmtpUserName.ToString());
        }

        private void SetVideoSettings()
        {
            this.VideoDevices = Utilities.VideoHandlers.AvailableDevices.FindVideoDevies();
            this.SelectedVideoDevice = this.applicationSettings.VideoDevice;
        }

        private void SaveChanges()
        {
            this.unitOfWork.SettingRepository.Update(this.Model.Language);
            this.unitOfWork.SettingRepository.Update(this.Model.BackupDatabaseName);
            this.unitOfWork.SettingRepository.Update(this.Model.BackupLocation);
            this.unitOfWork.SettingRepository.Update(this.Model.BackupFileName);
            this.unitOfWork.SettingRepository.Update(this.Model.BackupHour);
            this.unitOfWork.SettingRepository.Update(this.Model.BackupMinute);
            this.unitOfWork.SettingRepository.Update(this.Model.ReminderCheckDelay);
            this.unitOfWork.SettingRepository.Update(this.Model.SmtpEnableSsl);
            this.unitOfWork.SettingRepository.Update(this.Model.SmtpFromAddress);
            this.unitOfWork.SettingRepository.Update(this.Model.SmtpHost);
            this.unitOfWork.SettingRepository.Update(this.Model.SmtpPassword);
            this.unitOfWork.SettingRepository.Update(this.Model.SmtpPort);
            this.unitOfWork.SettingRepository.Update(this.Model.SmtpUserName);

            this.unitOfWork.Save();

            ApplicationSettingsManager.TryWriteValue("VideoDevice", this.SelectedVideoDevice);
        }

        private void HandleInvalidModel()
        {
            this.messageBoxProvider.SettingsCannotBeSaved();
        }
    }
}
