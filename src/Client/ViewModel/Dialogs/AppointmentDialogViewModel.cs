using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Contracts.Dialogs;
using Client.Providers;
using Common.Enumeration;
using Contracts;
using DataAccess;
using Models;
using Utilities.Resource;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model for the appointment view.
    /// </summary>
    public sealed class AppointmentDialogViewModel : Scheduler.ViewModel, IAppointmentDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IResourceHandler resourceHandler;

        private readonly IMessageBoxProvider messageBoxProvider;

        private List<AppointmentModel> appointments;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="resourceHandler">Resource handler.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        public AppointmentDialogViewModel(
            IUnitOfWork unitOfWork,
            IResourceHandler resourceHandler,
            IMessageBoxProvider messageBoxProvider)
        {
            this.unitOfWork = unitOfWork;
            this.resourceHandler = resourceHandler;
            this.messageBoxProvider = messageBoxProvider;
        }

        /// <summary>
        /// Populate view model for add a new appointment.
        /// </summary>
        /// <param name="listOfAppointments">List of all appointments.</param>
        /// <param name="selectedDate">Currently selected date.</param>
        /// <param name="startHour">Selected start hour.</param>
        /// <param name="startMinute">Selected start minute.</param>
        /// <param name="item1">Selected item 1 (doctor).</param>
        public void InitializeForAdd(List<AppointmentModel> listOfAppointments, DateTime selectedDate, int startHour, int startMinute, ItemModel item1)
        {
            this.Mode = WorkModeType.Add;
            this.appointments = listOfAppointments;

            Task.Factory.StartNewWithDefaultCulture(() => this.OnAddInitialize(selectedDate, startHour, startMinute, item1));
        }

        /// <summary>
        /// Populate view model for edit an existed appointment.
        /// </summary>
        /// <param name="listOfAppointments">List of all appointments.</param>
        /// <param name="appointment">The appointment.</param>
        public void InitializeForEdit(List<AppointmentModel> listOfAppointments, AppointmentModel appointment)
        {
            this.Mode = WorkModeType.Edit;
            this.appointments = listOfAppointments;

            var item1 = new ItemModel
            {
                Id = appointment.Item1Id,
                Name = appointment.Item1
            };

            var item2 = new ItemModel
            {
                Id = appointment.Item2Id,
                Name = appointment.Item2
            };

            Task.Factory.StartNewWithDefaultCulture(() => this.OnEditInitialize(appointment, item1, item2));
        }

        /// <summary>
        /// Populate of the selective items.
        /// </summary>
        /// <param name="selectedItem1">Selected item 1 (doctor).</param>
        /// <param name="selectedItem2">Selected item 2 (patient).</param>
        protected override void PopulateSelectiveItems(ItemModel selectedItem1, ItemModel selectedItem2 = null)
        {
            this.Status = LoadingStatus.Loading;

            this.PopulateSelectedStaff(selectedItem1);

            this.PopulateSelectedClient(selectedItem2);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Save an appointment.
        /// </summary>
        protected override void Handle()
        {
            this.Status = LoadingStatus.Loading;

            base.Handle();

            var appointmentModel = AppointmentModel.FromModel(this.Model);

            string validationErrorMessage;
            var isValid = this.ValidateAppointment(appointmentModel, out validationErrorMessage);
            if (!isValid)
            {
                this.Status = LoadingStatus.Canceled;

                this.ShowValidationErrorToUser(validationErrorMessage);
            }
            else
            {
                this.SaveChanges(appointmentModel);

                this.CloseDialog();
            }
        }

        private bool ValidateAppointment(IModel appointment, out string result)
        {
            result = string.Empty;

            if (appointment.StartTime == appointment.EndTime)
            {
                result = this.GetStartEndTimeMustBeDifferentMessage();
                return false;
            }

            if (appointment.StartTime > appointment.EndTime)
            {
                result = this.GetStartTimeMustBeLessThanEndTimeMessage();
                return false;
            }

            var itemStart = from c in this.appointments
                            where c.Id != this.Model.Id && c.Item1Id == SelectedItem1.Id
                            where c.StartTime > appointment.StartTime & c.StartTime < appointment.EndTime
                            select c;

            var itemEnd = from c in this.appointments
                          where c.Id != this.Model.Id && c.Item1Id == SelectedItem1.Id
                          where c.EndTime > appointment.StartTime & c.StartTime < appointment.EndTime
                          select c;

            if (itemStart.Any() || itemEnd.Any())
            {
                result = this.GetConflictingAppointmentsExistMessage();
                return false;
            }

            return true;
        }

        private void PopulateSelectedStaff(ItemModel selectedItem1)
        {
            this.Item1 = this.unitOfWork.StaffRepository.GetAllIsTakingForList();
            if (this.Item1.Any())
            {
                this.SelectedItem1 = selectedItem1 != null
                    ? this.Item1.Find(a => a.Id == selectedItem1.Id)
                    : this.Item1.First();
            }
        }

        private void PopulateSelectedClient(ItemModel selectedItem2)
        {
            this.Item2 = this.unitOfWork.PatientRepository.GetAllForList();
            if (this.Item2.Any())
            {
                this.SelectedItem2 = selectedItem2 != null
                    ? this.Item2.Find(a => a.Id == selectedItem2.Id)
                    : this.Item2.First();
            }
        }

        private void ShowValidationErrorToUser(string result)
        {
            var header = this.resourceHandler.GetValue("AppointmentAddItemTitle");
            this.messageBoxProvider.Show(header, result, MessageType.Warning);
        }

        private void SaveChanges(AppointmentModel appointmentModel)
        {
            switch (this.Mode)
            {
                case WorkModeType.Add:
                    this.unitOfWork.AppointmentRepository.Add(appointmentModel);
                    break;
                case WorkModeType.Edit:
                    this.unitOfWork.AppointmentRepository.Update(appointmentModel);
                    break;
            }

            this.unitOfWork.Save();
        }

        private string GetStartEndTimeMustBeDifferentMessage()
        {
            return this.resourceHandler.GetValue("AppointmentStartEndTimeMustBeDifferent")
                   + Environment.NewLine
                   + this.resourceHandler.GetValue("AppointmentChangeTime");
        }

        private string GetStartTimeMustBeLessThanEndTimeMessage()
        {
            return this.resourceHandler.GetValue("AppointmentStartTimeMustBeLessEndTime")
                   + Environment.NewLine
                   + this.resourceHandler.GetValue("AppointmentChangeTime");
        }

        private string GetConflictingAppointmentsExistMessage()
        {
            return this.resourceHandler.GetValue("AppointmentWithSameTimeExists")
                   + Environment.NewLine
                   + this.resourceHandler.GetValue("AppointmentChangeTime");
        }
    }
}
