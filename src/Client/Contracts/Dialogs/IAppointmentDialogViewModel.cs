using System;
using System.Collections.Generic;
using Contracts.ViewModel.Dialogs;
using Models;

namespace Client.Contracts.Dialogs
{
    /// <summary>
    /// Represents view model interface for appointment dialog.
    /// </summary>
    public interface IAppointmentDialogViewModel : IAppointmentDialogViewModelBase
    {
        /// <summary>
        /// Populate view model for add a new appointment.
        /// </summary>
        /// <param name="listOfAppointments">List of all appointments.</param>
        /// <param name="selectedDate">Currently selected date.</param>
        /// <param name="startHour">Selected start hour.</param>
        /// <param name="startMinute">Selected start minute.</param>
        /// <param name="item1">Selected item 1 (doctor).</param>
        void InitializeForAdd(List<AppointmentModel> listOfAppointments, DateTime selectedDate, int startHour, int startMinute, ItemModel item1);

        /// <summary>
        /// Populate view model for edit an existed appointment.
        /// </summary>
        /// <param name="listOfAppointments">List of all appointments.</param>
        /// <param name="appointment">The appointment.</param>
        void InitializeForEdit(List<AppointmentModel> listOfAppointments, AppointmentModel appointment);
    }
}