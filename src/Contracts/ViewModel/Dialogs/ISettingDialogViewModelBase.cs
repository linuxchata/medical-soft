using System.Collections.Generic;
using Contracts.ViewModel.Base;

namespace Contracts.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model interface for about box dialog.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    /// <typeparam name="T2">Common class that represents the culture model.</typeparam>
    public interface ISettingDialogViewModelBase<T, T2> : IViewModelDialogBase2<T>
    {
        /// <summary>
        /// Gets or sets selected backup hours.
        /// </summary>
        string SelectedBackupHour { get; set; }

        /// <summary>
        /// Gets or sets selected backup minutes.
        /// </summary>
        string SelectedBackupMinute { get; set; }

        /// <summary>
        /// Gets list of the cultures.
        /// </summary>
        List<T2> Cultures { get; }

        /// <summary>
        /// Gets list of the available hours.
        /// </summary>
        List<string> Hours { get; }

        /// <summary>
        /// Gets list of the available minutes in hour.
        /// </summary>
        List<string> Minutes { get; }

        /// <summary>
        /// Gets list of the duration.
        /// </summary>
        List<int> Durations { get; }

        /// <summary>
        /// Gets or sets currently selected culture.
        /// </summary>
        T2 SelectedCulture { get; set; }

        /// <summary>
        /// Gets or sets currently duration between showing the same notification on UI.
        /// </summary>
        int SelectedReminderCheckDelay { get; set; }
    }
}
