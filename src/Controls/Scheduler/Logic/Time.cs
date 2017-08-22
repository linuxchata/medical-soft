using System.Windows;
using System.Windows.Controls;

namespace Scheduler.Logic
{
    /// <summary>
    /// Represents time (hour and minute)
    /// </summary>
    public class Time : Control
    {
        /// <summary>
        /// The hours.
        /// </summary>
        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register(
            "Hours",
            typeof(string),
            typeof(Time),
            new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// The minutes.
        /// </summary>
        public static readonly DependencyProperty MinutesProperty = 
            DependencyProperty.Register(
            "Minutes",
            typeof(string),
            typeof(Time),
            new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// Initializes static members of the Time class.
        /// </summary>
        static Time()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Time), new FrameworkPropertyMetadata(typeof(Time)));
        }

        /// <summary>
        /// Gets or sets the hours.
        /// </summary>
        public string Hours
        {
            get
            {
                return (string)this.GetValue(HoursProperty);
            }
            
            set
            {
                this.SetValue(HoursProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the minutes.
        /// </summary>
        public string Minutes
        {
            get
            {
                return (string)this.GetValue(MinutesProperty);
            }

            set
            {
                this.SetValue(MinutesProperty, value);
            }
        }
    }
}
