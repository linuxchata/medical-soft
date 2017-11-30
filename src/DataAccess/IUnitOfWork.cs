using System;
using Common.Communication;
using Contracts;
using Models;
using Models.Enumeration;

namespace DataAccess
{
    /// <summary>
    /// Represents unit of work interface.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets appointment repository.
        /// </summary>
        IAppointmentRepository<AppointmentModel, Guid> AppointmentRepository { get; }

        /// <summary>
        /// Gets backup repository.
        /// </summary>
        IBackupRepository<BackupLogsModel, int> BackupRepository { get; }

        /// <summary>
        /// Gets education repository.
        /// </summary>
        IEducationRepository<EducationModel, int> EducationRepository { get; }

        /// <summary>
        /// Gets login repository.
        /// </summary>
        ILoginRepository<LoginModel, int> LoginRepository { get; }

        /// <summary>
        /// Gets notification group repository.
        /// </summary>
        INotificationGroupRepository<NotificationGroupModel, int> NotificationGroupRepository { get; }

        /// <summary>
        /// Gets notification list repository.
        /// </summary>
        INotificationListRepository<NotificationListModel, int> NotificationListRepository { get; }

        /// <summary>
        /// Gets notification list status repository.
        /// </summary>
        INotificationListStatusRepository<NotificationListStatusModel> NotificationListStatusRepository { get; }

        /// <summary>
        /// Gets notification template repository.
        /// </summary>
        INotificationTemplateRepository<NotificationTemplateModel, ItemModel, int> NotificationTemplateRepository { get; }

        /// <summary>
        /// Gets patient repository.
        /// </summary>
        IPatientRepository<PatientModel, ItemModel, int> PatientRepository { get; }

        /// <summary>
        /// Gets position repository.
        /// </summary>
        IPositionRepository<PositionModel, int> PositionRepository { get; }

        /// <summary>
        /// Gets reminder Alert repository.
        /// </summary>
        IReminderAlertRepository<ReminderAlertModel> ReminderAlertRepository { get; }

        /// <summary>
        /// Gets reminder Filter repository.
        /// </summary>
        IReminderFilterRepository<ReminderFilterModel> ReminderFilterRepository { get; }

        /// <summary>
        /// Gets reminder repository.
        /// </summary>
        IReminderRepository<ReminderModel, int> ReminderRepository { get; }

        /// <summary>
        /// Gets role repository.
        /// </summary>
        IRoleRepository<RoleModel, int> RoleRepository { get; }

        /// <summary>
        /// Gets setting repository.
        /// </summary>
        ISettingRepository<SettingModel, AvailableSettings> SettingRepository { get; }

        /// <summary>
        /// Gets gender repository.
        /// </summary>
        IGenderRepository<GenderModel> GenderRepository { get; }

        /// <summary>
        /// Gets staff repository.
        /// </summary>
        IStaffRepository<StaffModel, ItemModel, int> StaffRepository { get; }

        /// <summary>
        /// Gets system updates repository.
        /// </summary>
        ISystemUpdatesRepository<SystemUpdatesModel> SystemUpdatesRepository { get; }

        /// <summary>
        /// Gets culture repository.
        /// </summary>
        IUiCultureRepository<CultureModel> UiCultureRepository { get; }

        /// <summary>
        /// Save changes.
        /// </summary>
        /// <returns>Returns exception if any.</returns>
        SaveChangesResponse Save();
    }
}
