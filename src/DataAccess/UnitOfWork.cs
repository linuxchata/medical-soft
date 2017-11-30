using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using Common.Communication;
using Contracts;
using Logger;
using Microsoft.Practices.Unity;
using Models;
using Models.Enumeration;

namespace DataAccess
{
    /// <summary>
    /// Represents unity of work.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ObjectContext entities;

        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary> 
        /// <param name="container">Unity container.</param>
        public UnitOfWork(IUnityContainer container)
        {
            Log.Debug("Going to init UnitOfWork.");

            var watch = new Stopwatch();
            watch.Start();

            var hierarchicalLifetimeManager = new HierarchicalLifetimeManager();
            var injectionConstructor = new InjectionConstructor();
            container.RegisterType<ObjectContext, dentistEntities>(hierarchicalLifetimeManager, injectionConstructor);
            this.entities = container.Resolve<ObjectContext>();

            this.AppointmentRepository = container.Resolve<IAppointmentRepository<AppointmentModel, Guid>>();
            this.BackupRepository = container.Resolve<IBackupRepository<BackupLogsModel, int>>();
            this.EducationRepository = container.Resolve<IEducationRepository<EducationModel, int>>();
            this.LoginRepository = container.Resolve<ILoginRepository<LoginModel, int>>();
            this.NotificationGroupRepository = container.Resolve<INotificationGroupRepository<NotificationGroupModel, int>>();
            this.NotificationListRepository = container.Resolve<INotificationListRepository<NotificationListModel, int>>();
            this.NotificationListStatusRepository = container.Resolve<INotificationListStatusRepository<NotificationListStatusModel>>();
            this.NotificationTemplateRepository = container.Resolve<INotificationTemplateRepository<NotificationTemplateModel, ItemModel, int>>();
            this.PatientRepository = container.Resolve<IPatientRepository<PatientModel, ItemModel, int>>();
            this.PositionRepository = container.Resolve<IPositionRepository<PositionModel, int>>();
            this.RoleRepository = container.Resolve<IRoleRepository<RoleModel, int>>();
            this.SettingRepository = container.Resolve<ISettingRepository<SettingModel, AvailableSettings>>();
            this.GenderRepository = container.Resolve<IGenderRepository<GenderModel>>();
            this.StaffRepository = container.Resolve<IStaffRepository<StaffModel, ItemModel, int>>();
            this.SystemUpdatesRepository = container.Resolve<ISystemUpdatesRepository<SystemUpdatesModel>>();
            this.ReminderRepository = container.Resolve<IReminderRepository<ReminderModel, int>>();
            this.ReminderFilterRepository = container.Resolve<IReminderFilterRepository<ReminderFilterModel>>();
            this.UiCultureRepository = container.Resolve<IUiCultureRepository<CultureModel>>();
            this.ReminderAlertRepository = container.Resolve<IReminderAlertRepository<ReminderAlertModel>>();

            watch.Stop();

            Log.Debug("UnitOfWork was initialized. Took {0}", Log.Args(watch.Elapsed));
        }

        /// <summary>
        /// Gets appointment repository.
        /// </summary>
        public IAppointmentRepository<AppointmentModel, Guid> AppointmentRepository { get; private set; }

        /// <summary>
        /// Gets backup repository.
        /// </summary>
        public IBackupRepository<BackupLogsModel, int> BackupRepository { get; private set; }

        /// <summary>
        /// Gets education repository.
        /// </summary>
        public IEducationRepository<EducationModel, int> EducationRepository { get; set; }

        /// <summary>
        /// Gets login repository.
        /// </summary>
        public ILoginRepository<LoginModel, int> LoginRepository { get; private set; }

        /// <summary>
        /// Gets notification group repository.
        /// </summary>
        public INotificationGroupRepository<NotificationGroupModel, int> NotificationGroupRepository { get; private set; }

        /// <summary>
        /// Gets notification list repository.
        /// </summary>
        public INotificationListRepository<NotificationListModel, int> NotificationListRepository { get; private set; }

        /// <summary>
        /// Gets notification list status repository.
        /// </summary>
        public INotificationListStatusRepository<NotificationListStatusModel> NotificationListStatusRepository { get; private set; }

        /// <summary>
        /// Gets notification template repository.
        /// </summary>
        public INotificationTemplateRepository<NotificationTemplateModel, ItemModel, int> NotificationTemplateRepository { get; private set; }

        /// <summary>
        /// Gets patient repository.
        /// </summary>
        public IPatientRepository<PatientModel, ItemModel, int> PatientRepository { get; private set; }

        /// <summary>
        /// Gets position repository.
        /// </summary>
        public IPositionRepository<PositionModel, int> PositionRepository { get; private set; }

        /// <summary>
        /// Gets role repository.
        /// </summary>
        public IRoleRepository<RoleModel, int> RoleRepository { get; private set; }

        /// <summary>
        /// Gets setting repository.
        /// </summary>
        public ISettingRepository<SettingModel, AvailableSettings> SettingRepository { get; private set; }

        /// <summary>
        /// Gets sex repository.
        /// </summary>
        public IGenderRepository<GenderModel> GenderRepository { get; private set; }

        /// <summary>
        /// Gets staff repository.
        /// </summary>
        public IStaffRepository<StaffModel, ItemModel, int> StaffRepository { get; private set; }

        /// <summary>
        /// Gets system updates repository.
        /// </summary>
        public ISystemUpdatesRepository<SystemUpdatesModel> SystemUpdatesRepository { get; private set; }

        /// <summary>
        /// Gets reminder repository.
        /// </summary>
        public IReminderRepository<ReminderModel, int> ReminderRepository { get; private set; }

        /// <summary>
        /// Gets reminder Filter repository.
        /// </summary>
        public IReminderFilterRepository<ReminderFilterModel> ReminderFilterRepository { get; private set; }

        /// <summary>
        /// Gets reminder Alert repository.
        /// </summary>
        public IReminderAlertRepository<ReminderAlertModel> ReminderAlertRepository { get; private set; }

        /// <summary>
        /// Gets culture repository.
        /// </summary>
        public IUiCultureRepository<CultureModel> UiCultureRepository { get; private set; }

        /// <summary>
        /// Save changes.
        /// </summary>
        /// <returns>Returns exception if any.</returns>
        public SaveChangesResponse Save()
        {
            var saveChangesResponse = new SaveChangesResponse();

            try
            {
                var watch = new Stopwatch();
                watch.Start();

                this.ReopenConnectionIfClosed();

                var addedEntities = this.entities.ObjectStateManager.GetObjectStateEntries(EntityState.Added).ToList();

                this.entities.SaveChanges();

                if (addedEntities.Any())
                {
                    this.HandleAddedEntities(addedEntities, saveChangesResponse);
                }

                watch.Stop();

                Log.Debug("Changes have been saved. Took {0}.", Log.Args(watch.Elapsed));

                return saveChangesResponse;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                saveChangesResponse.SetException(ex);

                return saveChangesResponse;
            }
        }

        /// <summary>
        /// Dispose object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose object.
        /// </summary>
        /// <param name="disposing">Does object has to be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.entities.Dispose();
                }

                this.isDisposed = true;
            }
        }

        private void ReopenConnectionIfClosed()
        {
            if (this.entities.Connection.State == ConnectionState.Closed)
            {
                Log.Warn("Database connection was closed. Try to reopen it.");
                this.entities.Connection.Open();
            }
        }

        private void HandleAddedEntities(IEnumerable<ObjectStateEntry> addedEntities, SaveChangesResponse response)
        {
            foreach (var entity in addedEntities)
            {
                var entityKey = entity.EntityKey;
                if (!entityKey.EntityKeyValues.Any())
                {
                    Log.Warn("No EntityKeyValues were found in object context.");
                    continue;
                }

                DatabaseEntity key;
                var iskeyParsed = Enum.TryParse(entityKey.EntitySetName, out key);

                int value;
                var isValueParsed = int.TryParse(entityKey.EntityKeyValues[0].Value.ToString(), out value);

                if (iskeyParsed && isValueParsed)
                {
                    Log.Debug("Add value {0} with the key {1} to the save changes response.", Log.Args(value, key));
                    response.TryAddResult(key.ToString(), value);
                }
            }
        }
    }
}
