using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Commands;
using Common.Enumeration;
using Common.ViewModel;
using Contracts.ViewModel;
using DataAccess;
using Models;
using Models.Enumeration;
using Services.Backup;

namespace Client.ViewModel
{
    /// <summary>
    /// Represents view model for backup.
    /// </summary>
    public sealed class BackupViewModel : ViewModelMainBase<BackupLogsModel>, IBackupViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IBackuper backuper;

        private readonly IApplicationSettings applicationSettings;

        private ICommand runCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="backuper">Object to perform backup of the database.</param>
        /// <param name="applicationSettings">Application settings.</param>
        public BackupViewModel(IUnitOfWork unitOfWork, IBackuper backuper, IApplicationSettings applicationSettings)
        {
            this.unitOfWork = unitOfWork;
            this.backuper = backuper;
            this.applicationSettings = applicationSettings;

            Task.Factory.StartNewWithDefaultCulture(this.UpdateData);
        }

        /// <summary>
        /// Gets run command.
        /// </summary>
        public ICommand RunCommand
        {
            get
            {
                return this.runCommand ?? (this.runCommand = new CommonCommand(
                    param => this.RunBackup(),
                    param => true));
            }
        }

        /// <summary>
        /// Gets a value indicating whether application is run under client only mode.
        /// </summary>
        public bool IsClientMode
        {
            get
            {
                return this.applicationSettings.RunningMode == RunningMode.ClientOnly;
            }
        }

        /// <summary>
        /// Refresh backup.
        /// </summary>
        public override void UpdateData()
        {
            this.Status = LoadingStatus.Loading;

            var backups = this.unitOfWork.BackupRepository.GetAll();
            this.Model = new ObservableCollection<BackupLogsModel>(backups);

            this.OnPropertyChanged(() => this.Count);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Add/Edit patient dialog.
        /// </summary>
        /// <param name="mode">The mode (Add/Edit).</param>
        protected override void AddEditDialog(WorkModeType mode)
        {
        }

        /// <summary>
        /// Delete backup.
        /// </summary>
        protected override void Delete()
        {
        }

        private void RunBackup()
        {
            var task = new Task(() =>
            {
                this.Status = LoadingStatus.Loading;

                this.backuper.PerformBackup(BackuperType.Manual);

                this.UpdateData();

                this.Status = LoadingStatus.Loaded;
            });
            task.Start();
        }
    }
}
