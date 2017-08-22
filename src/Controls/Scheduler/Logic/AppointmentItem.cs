using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Scheduler.Logic
{
    /// <summary>
    /// Represents the appointment item.
    /// </summary>
    public class AppointmentItem : ButtonBase
    {
        /// <summary>
        /// Appointment unique identifier.
        /// </summary>
        public static readonly DependencyProperty AppointmentGuidProperty =
            DependencyProperty.Register(
            "AppointmentGuid",
            typeof(Guid),
            typeof(AppointmentItem),
            new FrameworkPropertyMetadata(Guid.Empty));

        /// <summary>
        /// Edit appointment command.
        /// </summary>
        public static readonly RoutedEvent EditAppointmentEvent =
            EventManager.RegisterRoutedEvent(
            "EditAppointment",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(AppointmentItem));

        /// <summary>
        /// Appointment start time.
        /// </summary>
        public static readonly DependencyProperty StartTimeProperty = TimeSlotPanel.StartTimeProperty.AddOwner(typeof(AppointmentItem));

        /// <summary>
        /// Appointment end time.
        /// </summary>
        public static readonly DependencyProperty EndTimeProperty = TimeSlotPanel.EndTimeProperty.AddOwner(typeof(AppointmentItem));

        /// <summary>
        /// Initializes static members of the AppointmentItem class.
        /// </summary>
        static AppointmentItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppointmentItem), new FrameworkPropertyMetadata(typeof(AppointmentItem)));
        }

        /// <summary>
        /// Edit appointment command.
        /// </summary>
        public event RoutedEventHandler EditAppointment
        {
            add
            {
                this.AddHandler(EditAppointmentEvent, value);
            }

            remove
            {
                this.RemoveHandler(EditAppointmentEvent, value);
            }
        }

        /// <summary>
        /// Gets or sets appointment unique identifier.
        /// </summary>
        public Guid AppointmentGuid
        {
            get
            {
                return (Guid)this.GetValue(AppointmentGuidProperty);
            }

            set
            {
                this.SetValue(AppointmentGuidProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the start time.
        /// </summary>
        public bool StartTime
        {
            get
            {
                return (bool)this.GetValue(StartTimeProperty);
            }

            set
            {
                this.SetValue(StartTimeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the end time.
        /// </summary>
        public bool EndTime
        {
            get
            {
                return (bool)this.GetValue(EndTimeProperty);
            }

            set
            {
                this.SetValue(EndTimeProperty, value);
            }
        }

        /// <summary>
        /// Override OnClick event.
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();

            this.RaiseEditAppointmentEvent();
        }

        /// <summary>
        /// Raise EditAppointmentEvent.
        /// </summary>
        private void RaiseEditAppointmentEvent()
        {
            this.OnEditAppointmentEvent(new RoutedEventArgs(EditAppointmentEvent, this));
        }

        /// <summary>
        /// Edit appointmentEvent event handler.
        /// </summary>
        /// <param name="routedEventArgs">Routed event argument.</param>
        private void OnEditAppointmentEvent(RoutedEventArgs routedEventArgs)
        {
            this.RaiseEvent(routedEventArgs);
        }
    }
}
