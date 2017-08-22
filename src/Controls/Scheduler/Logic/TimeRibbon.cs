using System.Windows;
using System.Windows.Controls;

namespace Scheduler.Logic
{
    /// <summary>
    /// Represents all day's hours (0..23)
    /// </summary>
    public class TimeRibbon : Control
    {
        /// <summary>
        /// Initializes static members of the TimeRibbon class.
        /// </summary>
        static TimeRibbon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeRibbon), new FrameworkPropertyMetadata(typeof(TimeRibbon)));
        }
    }
}
