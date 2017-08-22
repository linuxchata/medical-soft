using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Contracts.Dialogs;
using Common.Commands;
using Common.Enumeration;
using Common.Events;
using Common.ViewModel;
using DataAccess;
using Models;
using Utilities.EventAggregator;
using Utilities.Resource;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model for reminder popup view.
    /// </summary>
    public sealed class ReminderPopupDialogViewModel : ViewModelDialogBase<ReminderModel>, IReminderPopupDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IEventAggregator eventAggregator;

        private readonly IResourceHandler resourceHandler;

        private ManualResetEvent manualResetEvent;

        private List<ReminderModel> reminders;

        private bool isNextReminderExists;

        private bool isPreviousReminderExists;

        private ICommand nextReminderCommand;

        private ICommand previousReminderCommand;

        private ICommand completeReminderCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderPopupDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="eventAggregator">Event aggregator.</param>
        /// <param name="resourceHandler">Resource handler.</param>
        public ReminderPopupDialogViewModel(
            IUnitOfWork unitOfWork,
            IEventAggregator eventAggregator,
            IResourceHandler resourceHandler)
        {
            this.unitOfWork = unitOfWork;
            this.eventAggregator = eventAggregator;
            this.resourceHandler = resourceHandler;

            this.Reminders = new List<ReminderModel>();
            this.Model = new ReminderModel();
        }

        /// <summary>
        /// Gets or sets reminder's model.
        /// </summary>
        public List<ReminderModel> Reminders
        {
            get
            {
                return this.reminders;
            }

            set
            {
                this.reminders = value;

                var currentIndex = this.Reminders.IndexOf(this.Model);
                this.IsNextReminderExists = this.Reminders.Count > currentIndex + 1;
                this.IsPreviousReminderExists = (currentIndex - 1) >= 0;
                this.OnPropertyChanged(() => this.Reminders);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the next reminder visible.
        /// </summary>
        public bool IsNextReminderExists
        {
            get
            {
                return this.isNextReminderExists;
            }

            set
            {
                this.isNextReminderExists = value;
                this.OnPropertyChanged(() => this.IsNextReminderExists);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the previous reminder visible.
        /// </summary>
        public bool IsPreviousReminderExists
        {
            get
            {
                return this.isPreviousReminderExists;
            }

            set
            {
                this.isPreviousReminderExists = value;
                this.OnPropertyChanged(() => this.IsPreviousReminderExists);
            }
        }

        /// <summary>
        /// Gets next reminder command.
        /// </summary>
        public ICommand NextReminderCommand
        {
            get
            {
                return this.nextReminderCommand ?? (this.nextReminderCommand = new CommonCommand(
                    param => this.GetNextReminder(),
                    param => this.IsNextReminderExists));
            }
        }

        /// <summary>
        /// Gets previous reminder command.
        /// </summary>
        public ICommand PreviousReminderCommand
        {
            get
            {
                return this.previousReminderCommand ?? (this.previousReminderCommand = new CommonCommand(
                    param => this.GetPreviousReminder(),
                    param => this.IsPreviousReminderExists));
            }
        }

        /// <summary>
        /// Gets complete reminder command.
        /// </summary>
        public ICommand CompleteReminderCommand
        {
            get
            {
                return this.completeReminderCommand ?? (this.completeReminderCommand = new CommonCommand(
                    param => Task.Factory.StartNewWithDefaultCulture(this.CompleteReminder),
                    param => true));
            }
        }

        /// <summary>
        /// Check for active notifications.
        /// </summary>
        /// <returns>Returns is active notifications exist.</returns>
        public bool CheckForActiveNotifications()
        {
            var listOfReminders = this.unitOfWork.ReminderRepository.GetActiveReminders().ToList();
            if (listOfReminders.Any())
            {
                foreach (var reminder in listOfReminders)
                {
                    reminder.Doctor = this.GetReminderDoctor(reminder);
                    reminder.Patient = this.GetReminderPatient(reminder);
                    reminder.PatientPhoneNumbers = this.GetReminderPatientPhoneNumbers(reminder);
                }

                this.Model = listOfReminders.First();
                this.Reminders = listOfReminders;

                return true;
            }

            this.Model = new ReminderModel();
            this.Reminders = new List<ReminderModel>();

            return false;
        }

        /// <summary>
        /// Set ManualResetEvent object.
        /// </summary>
        /// <param name="resetEvent">ManualResetEvent object.</param>
        public void SetManualResetEvent(ManualResetEvent resetEvent)
        {
            this.manualResetEvent = resetEvent;
        }

        /// <summary>
        /// Close window.
        /// </summary>
        protected override void OnRequestClose()
        {
            if (this.manualResetEvent != null)
            {
                this.manualResetEvent.Set();
            }

            base.OnRequestClose();
        }

        private void GetNextReminder()
        {
            var currentIndex = this.Reminders.IndexOf(this.Model);

            if (this.IsNextReminderExists)
            {
                this.Model = this.Reminders[++currentIndex];
            }

            this.IsNextReminderExists = this.Reminders.Count > currentIndex + 1;
            this.IsPreviousReminderExists = (currentIndex - 1) >= 0;
        }

        private void GetPreviousReminder()
        {
            var currentIndex = this.Reminders.IndexOf(this.Model);

            if (this.IsPreviousReminderExists)
            {
                this.Model = this.Reminders[--currentIndex];
            }

            this.IsNextReminderExists = this.Reminders.Count > currentIndex + 1;
            this.IsPreviousReminderExists = (currentIndex - 1) >= 0;
        }

        private void CompleteReminder()
        {
            this.Status = LoadingStatus.Loading;

            this.unitOfWork.ReminderRepository.Update(this.Model);
            this.unitOfWork.Save();

            this.eventAggregator.Publish<ReminderChangedEvent>();

            this.Status = LoadingStatus.Loaded;
        }

        private string GetReminderDoctor(ReminderModel reminder)
        {
            if (reminder.DoctorId == null)
            {
                reminder.Doctor = this.resourceHandler.GetValue("ReminderDoctorWasNotSelected");
            }

            return this.resourceHandler.GetValue("NotificationDoctor") + reminder.Doctor;
        }

        private string GetReminderPatient(ReminderModel reminder)
        {
            return this.resourceHandler.GetValue("NotificationPatient") + reminder.Patient;
        }

        private string GetReminderPatientPhoneNumbers(ReminderModel reminder)
        {
            var phoneNumberWork = reminder.PatientPhoneNumberWork;
            var phoneNumberCell = this.GetCellPhoneNumber(phoneNumberWork, reminder);
            var phoneNumberHome = this.GetHomePhoneNumber(phoneNumberCell, reminder);
            return string.Format("{0}{1}{2}", phoneNumberWork, phoneNumberCell, phoneNumberHome);
        }

        private string GetCellPhoneNumber(string phoneNumberWork, ReminderModel reminder)
        {
            return !string.IsNullOrWhiteSpace(phoneNumberWork) && !string.IsNullOrWhiteSpace(reminder.PatientPhoneNumberCell)
                ? ", " + reminder.PatientPhoneNumberCell
                : reminder.PatientPhoneNumberCell;
        }

        private string GetHomePhoneNumber(string phoneNumberCell, ReminderModel reminder)
        {
            return (!string.IsNullOrWhiteSpace(phoneNumberCell)
                || !string.IsNullOrWhiteSpace(reminder.PatientPhoneNumberWork))
                && !string.IsNullOrWhiteSpace(reminder.PatientPhoneNumberHome)
                ? ", " + reminder.PatientPhoneNumberHome
                : reminder.PatientPhoneNumberHome;
        }
    }
}
