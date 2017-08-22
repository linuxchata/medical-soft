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
    /// Represents view model class for reminder view.
    /// </summary>
    public sealed class ReminderDialogViewModel : ViewModelDialogBase2<ReminderModel>, IReminderDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IResourceHandler resourceHandler;

        private readonly IMessageBoxProvider messageBoxProvider;

        private readonly IReminderAlertCache reminderAlertCache;

        private List<ItemModel> doctors;

        private ItemModel selectedDoctor;

        private List<ItemModel> patients;

        private ItemModel selectedPatient;

        private List<ReminderAlertModel> reminderAlerts;

        private ReminderAlertModel selectedReminderAlert;

        private List<string> hours;

        private List<string> minutes;

        private string selectedHour;

        private string selectedMinute;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="resourceHandler">Resource handler.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="reminderAlertCache">Reminder alert cache.</param>
        /// <param name="mode">Mode (Add/Edit).</param>
        /// <param name="model">Reminder model.</param>
        public ReminderDialogViewModel(
            IUnitOfWork unitOfWork,
            IResourceHandler resourceHandler,
            IMessageBoxProvider messageBoxProvider,
            IReminderAlertCache reminderAlertCache,
            WorkModeType mode,
            ReminderModel model = null)
            : base(mode)
        {
            this.unitOfWork = unitOfWork;
            this.resourceHandler = resourceHandler;
            this.messageBoxProvider = messageBoxProvider;
            this.reminderAlertCache = reminderAlertCache;

            Task.Factory.StartNewWithDefaultCulture(() => this.Load(model));
        }

        /// <summary>
        /// Gets or sets list of all doctors.
        /// </summary>
        public List<ItemModel> Doctors
        {
            get
            {
                return this.doctors;
            }

            set
            {
                this.doctors = value;
                this.OnPropertyChanged(() => this.Doctors);
            }
        }

        /// <summary>
        /// Gets or sets currently selected doctor.
        /// </summary>
        public ItemModel SelectedDoctor
        {
            get
            {
                return this.selectedDoctor;
            }

            set
            {
                this.selectedDoctor = value;
                this.Model.DoctorId = this.selectedDoctor != null ? this.selectedDoctor.Id : -1;
                this.Model.Doctor = this.selectedDoctor != null ? this.selectedDoctor.Name : string.Empty;
                this.OnPropertyChanged(() => this.SelectedDoctor);
            }
        }

        /// <summary>
        /// Gets or sets list of all patients.
        /// </summary>
        public List<ItemModel> Patients
        {
            get
            {
                return this.patients;
            }

            set
            {
                this.patients = value;
                this.OnPropertyChanged(() => this.Patients);
            }
        }

        /// <summary>
        /// Gets or sets currently selected patient.
        /// </summary>
        public ItemModel SelectedPatient
        {
            get
            {
                return this.selectedPatient;
            }

            set
            {
                this.selectedPatient = value;
                this.Model.PatientId = this.selectedPatient != null ? this.selectedPatient.Id : (int?)null;
                this.Model.Patient = this.selectedPatient != null ? this.selectedPatient.Name : string.Empty;
                this.OnPropertyChanged(() => this.SelectedPatient);
            }
        }

        /// <summary>
        /// Gets or sets list of the all reminder alerts.
        /// </summary>
        public List<ReminderAlertModel> ReminderAlerts
        {
            get
            {
                return this.reminderAlerts;
            }

            set
            {
                this.reminderAlerts = value;
                this.OnPropertyChanged(() => this.ReminderAlerts);
            }
        }

        /// <summary>
        /// Gets or sets currently selected reminder's alert.
        /// </summary>
        public ReminderAlertModel SelectedReminderAlert
        {
            get
            {
                return this.selectedReminderAlert;
            }

            set
            {
                this.selectedReminderAlert = value;
                this.Model.AlertId = this.selectedReminderAlert.Id;
                this.OnPropertyChanged(() => this.SelectedReminderAlert);
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
        /// Gets or sets selected hour.
        /// </summary>
        public string SelectedHour
        {
            get
            {
                return this.selectedHour;
            }

            set
            {
                this.selectedHour = value;
                this.OnPropertyChanged(() => this.SelectedHour);
            }
        }

        /// <summary>
        /// Gets or sets selected minute.
        /// </summary>
        public string SelectedMinute
        {
            get
            {
                return this.selectedMinute;
            }

            set
            {
                this.selectedMinute = value;
                this.OnPropertyChanged(() => this.SelectedMinute);
            }
        }

        /// <summary>
        /// Load information about reminders.
        /// </summary>
        /// <param name="reminderModel">Reminder model.</param>
        protected override void Load(ReminderModel reminderModel)
        {
            this.Status = LoadingStatus.Loading;

            this.LoadModels(reminderModel);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Save reminder.
        /// </summary>
        protected override void Handle()
        {
            this.Status = LoadingStatus.Loading;

            this.UpdateReminderDateTime();

            this.DeleteDummyDoctorNameIfDoctorWasNotSelected();

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

        private void LoadModels(ReminderModel reminderModel)
        {
            this.Hours = TimeHelper.Hours;
            this.Minutes = TimeHelper.Minutes;

            this.ReminderAlerts = this.reminderAlertCache.Get();

            this.Patients = this.unitOfWork.PatientRepository.GetAllForList();

            this.LoadDoctors();

            if (this.Mode == WorkModeType.Add)
            {
                this.LoadModelsForAddItem();
            }
            else if (this.Mode == WorkModeType.Edit)
            {
                this.LoadModelsForEditItem(reminderModel);
            }
        }

        private void LoadDoctors()
        {
            var dummyDoctor = new ItemModel
            {
                Id = -1,
                Name = this.resourceHandler.GetValue("ReminderSelectDoctor")
            };

            this.Doctors = new List<ItemModel>
            {
                dummyDoctor
            };

            var takingDoctors = this.unitOfWork.StaffRepository.GetAllIsTakingForList();

            this.Doctors.AddRange(takingDoctors);
        }

        private void LoadModelsForAddItem()
        {
            this.Model = new ReminderModel();

            if (this.Doctors.Any())
            {
                this.SelectedDoctor = this.Doctors.First();
            }

            if (this.Patients.Any())
            {
                this.SelectedPatient = this.Patients.First();
            }

            if (this.ReminderAlerts.Any())
            {
                this.SelectedReminderAlert = this.ReminderAlerts.First();
            }

            this.Model.Date = DateTime.Now;
            this.SelectedHour = TimeHelper.Hours.FirstOrDefault(a => a.Equals("08", StringComparison.InvariantCultureIgnoreCase));
            this.SelectedMinute = TimeHelper.Minutes.FirstOrDefault();
        }

        private void LoadModelsForEditItem(ReminderModel reminderModel)
        {
            this.Model = reminderModel.Map();

            this.SelectedDoctor = this.Model.DoctorId.HasValue
                ? this.Doctors.Find(a => a.Id == this.Model.DoctorId)
                : this.Doctors.FirstOrDefault();

            this.SelectedPatient = this.Patients.Find(a => a.Id == this.Model.PatientId);
            this.SelectedReminderAlert = this.ReminderAlerts.Find(a => a.Id == this.Model.AlertId);
            this.SelectedHour = this.Model.Date.ToString("HH");
            this.SelectedMinute = this.Model.Date.ToString("mm");
        }

        private void UpdateReminderDateTime()
        {
            var currentDate = this.Model.Date;
            var hour = Convert.ToInt32(this.SelectedHour);
            var minute = Convert.ToInt32(this.SelectedMinute);

            this.Model.Date = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, hour, minute, 00);
        }

        private void DeleteDummyDoctorNameIfDoctorWasNotSelected()
        {
            if (this.Model.DoctorId == -1)
            {
                this.Model.Doctor = string.Empty;
            }
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
            this.unitOfWork.ReminderRepository.Add(this.Model);
            this.Status = LoadingStatus.Added;
        }

        private void EditItem()
        {
            this.unitOfWork.ReminderRepository.Update(this.Model);
            this.Status = LoadingStatus.Updated;
        }

        private void UpdateIdOfAddedItem(SaveChangesResponse response)
        {
            if (this.Status == LoadingStatus.Added)
            {
                this.Model.Id = response.TryGetValue(DatabaseEntity.Reminders.ToString());
            }
        }

        private void HandleFailure()
        {
            this.Status = LoadingStatus.Failed;
            this.messageBoxProvider.CannotBeSavedDueToError();
        }
    }
}
