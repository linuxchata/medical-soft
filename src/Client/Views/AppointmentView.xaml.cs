using System;
using System.Windows;
using Client.ViewModel;
using Scheduler.Logic;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for AppointmentView
    /// </summary>
    public partial class AppointmentView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentView"/> class.
        /// </summary>
        public AppointmentView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Add an appointment event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnAddAppointment(object sender, RoutedEventArgs e)
        {
            // Get all necessary information for initialization add appointment window
            var currentSelectedDate = ((Scheduler.Logic.Calendar)sender).CurrentDay;
            var selectedStartHour = Convert.ToInt32(((TimeSlot)e.OriginalSource).Hours);
            var selectedStartMinute = Convert.ToInt32(((TimeSlot)e.OriginalSource).Minutes);

            var vm = this.DataContext as AppointmentViewModel;

            if (vm != null)
            {
                // Call add appointment event handler from view model
                vm.OnAdd(currentSelectedDate, selectedStartHour, selectedStartMinute);

                // Update appointments
                ((Scheduler.Logic.Calendar)sender).FilterAppointments();
            }
        }

        /// <summary>
        /// Edit an appointment event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnEditAppointment(object sender, RoutedEventArgs e)
        {
            // Get guid of the selected appointment
            var appointmentGuid = ((AppointmentItem)e.OriginalSource).AppointmentGuid;

            var vm = this.DataContext as AppointmentViewModel;

            if (vm != null)
            {
                vm.OnEdit(appointmentGuid);

                // Update appointments
                ((Scheduler.Logic.Calendar)sender).FilterAppointments();
            }
        }

        /// <summary>
        /// Delete the appointment event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnDeleteAppointment(object sender, RoutedEventArgs e)
        {
            // Get guid of the selected appointment
            var appointmentGuid = ((DeleteButton)e.OriginalSource).AppointmentDeleteGuid;

            var vm = this.DataContext as AppointmentViewModel;

            if (vm != null)
            {
                vm.OnDelete(appointmentGuid);

                // Update appointments
                ((Scheduler.Logic.Calendar)sender).FilterAppointments();
            }
        }
    }
}
