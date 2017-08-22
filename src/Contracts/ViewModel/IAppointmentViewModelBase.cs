using System;
using System.Collections.Generic;
using Contracts.ViewModel.Base;

namespace Contracts.ViewModel
{
    /// <summary>
    /// Represents view model interface for appointment.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    /// <typeparam name="TItem">Common class that represents the item model.</typeparam>
    public interface IAppointmentViewModelBase<T, TItem> : ISubscribable
    {
        /// <summary>
        /// Gets or sets model for staff items.
        /// </summary>
        List<TItem> StaffItemsModel { get; set; }

        /// <summary>
        /// Gets or sets selected staff items.
        /// </summary>
        TItem SelectedStaffItem { get; set; }

        /// <summary>
        /// Gets or sets model for appointment.
        /// </summary>
        List<T> Model { get; set; }

        /// <summary>
        /// Add an appointment event handler.
        /// </summary>
        /// <param name="currentSelectedDate">Currently selected date.</param>
        /// <param name="selectedStartHour">Selected start hour.</param>
        /// <param name="selectedStartMinute">Selected start minute.</param>
        void OnAdd(DateTime currentSelectedDate, int selectedStartHour, int selectedStartMinute);

        /// <summary>
        /// Edit the appointment event handler.
        /// </summary>
        /// <param name="appointmentGuid">Unique number of the selected appointment.</param>
        void OnEdit(Guid appointmentGuid);

        /// <summary>
        /// Delete the appointment event handler.
        /// </summary>
        /// <param name="appointmentGuid">Unique number of the selected appointment.</param>
        void OnDelete(Guid appointmentGuid);
    }
}
