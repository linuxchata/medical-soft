using System.Windows;
using System.Windows.Controls;

namespace Scheduler.Logic
{
    /// <summary>
    /// Represents day.
    /// </summary>
    public class SchedulerDay : ItemsControl
    {
        /// <summary>
        /// Initializes static members of the SchedulerDay class.
        /// </summary>
        static SchedulerDay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SchedulerDay), new FrameworkPropertyMetadata(typeof(SchedulerDay)));
        }

        /// <summary>
        /// Get appointment item.
        /// </summary>
        /// <returns>Returns appointment item.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new AppointmentItem();
        }

        /// <summary>
        /// Check is item of type AppointmentItem.
        /// </summary>
        /// <param name="item">The type to check.</param>
        /// <returns>Returns true if item is of type AppointmentItem; otherwise, false.</returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is AppointmentItem;
        }
    }
}
