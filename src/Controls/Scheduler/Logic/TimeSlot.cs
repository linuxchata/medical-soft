using System.Windows;
using System.Windows.Controls.Primitives;

namespace Scheduler.Logic
{
    /// <summary>
    /// Represents atomic slot of time (15 minutes)
    /// </summary>
    public class TimeSlot : ButtonBase
    {
        /// <summary>
        /// Add appointment event.
        /// </summary>
        public static readonly RoutedEvent AddAppointmentEvent =
            EventManager.RegisterRoutedEvent(
            "AddAppointment",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(TimeSlot));

        /// <summary>
        /// The hours.
        /// </summary>
        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register(
            "Hours",
            typeof(string),
            typeof(TimeSlot),
            new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// The minutes.
        /// </summary>
        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register(
            "Minutes",
            typeof(string),
            typeof(TimeSlot),
            new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// Initializes static members of the TimeSlot class.
        /// </summary>
        static TimeSlot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeSlot), new FrameworkPropertyMetadata(typeof(TimeSlot)));
        }

        /// <summary>
        /// Add appointment event.
        /// </summary>
        public event RoutedEventHandler AddAppointment
        {
            add
            {
                this.AddHandler(AddAppointmentEvent, value);
            }

            remove
            {
                this.RemoveHandler(AddAppointmentEvent, value);
            }
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

        /// <summary>
        /// Override OnClick event.
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();

            this.RaiseAddAppointmentEvent();
        }

        /// <summary>
        /// Raise AddAppointmentEvent.
        /// </summary>
        private void RaiseAddAppointmentEvent()
        {
            this.OnAddAppointmentEvent(new RoutedEventArgs(AddAppointmentEvent, this));
        }

        /// <summary>
        /// AddAppointment event handler.
        /// </summary>
        /// <param name="routedEventArgs">Routed event argument.</param>
        private void OnAddAppointmentEvent(RoutedEventArgs routedEventArgs)
        {
            this.RaiseEvent(routedEventArgs);
        }
    }
}
