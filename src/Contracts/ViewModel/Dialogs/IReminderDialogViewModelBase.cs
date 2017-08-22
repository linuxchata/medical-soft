using System.Collections.Generic;
using Contracts.ViewModel.Base;

namespace Contracts.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model interface for reminder dialog.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    /// <typeparam name="T2">Common class that represents the item model.</typeparam>
    /// <typeparam name="T3">Common class that represents the reminder model model.</typeparam>
    public interface IReminderDialogViewModelBase<T, T2, T3> : IViewModelDialogBase2<T>
    {
        /// <summary>
        /// Gets list of all doctors.
        /// </summary>
        List<T2> Doctors { get; }

        /// <summary>
        /// Gets or sets currently selected doctor.
        /// </summary>
        T2 SelectedDoctor { get; set; }

        /// <summary>
        /// Gets list of all patients.
        /// </summary>
        List<T2> Patients { get; }

        /// <summary>
        /// Gets or sets currently selected patient.
        /// </summary>
        T2 SelectedPatient { get; set; }

        /// <summary>
        /// Gets list of the all reminder alerts.
        /// </summary>
        List<T3> ReminderAlerts { get; }

        /// <summary>
        /// Gets or sets currently selected reminder's alert.
        /// </summary>
        T3 SelectedReminderAlert { get; set; }

        /// <summary>
        /// Gets or sets selected hours.
        /// </summary>
        string SelectedHour { get; set; }

        /// <summary>
        /// Gets or sets selected minutes.
        /// </summary>
        string SelectedMinute { get; set; }
    }
}
