using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Scheduler.Logic
{
    /// <summary>
    /// Represents delete appointment button.
    /// </summary>
    public class DeleteButton : ButtonBase
    {
        /// <summary>
        /// Appointment unique identifier.
        /// </summary>
        public static readonly DependencyProperty AppointmentDeleteGuidProperty =
            DependencyProperty.Register(
            "AppointmentDeleteGuid",
            typeof(Guid),
            typeof(DeleteButton),
            new FrameworkPropertyMetadata(Guid.Empty));

        /// <summary>
        /// Delete appointment command.
        /// </summary>
        public static readonly RoutedEvent DeleteAppointmentEvent =
            EventManager.RegisterRoutedEvent(
            "DeleteAppointment",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(DeleteButton));

        /// <summary>
        /// Initializes static members of the DeleteButton class.
        /// </summary>
        static DeleteButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DeleteButton), new FrameworkPropertyMetadata(typeof(DeleteButton)));
        }

        /// <summary>
        /// Delete appointment command.
        /// </summary>
        public event RoutedEventHandler CloseAppointment
        {
            add
            {
                this.AddHandler(DeleteAppointmentEvent, value);
            }

            remove
            {
                this.RemoveHandler(DeleteAppointmentEvent, value);
            }
        }

        /// <summary>
        /// Gets or sets appointment unique identifier.
        /// </summary>
        public Guid AppointmentDeleteGuid
        {
            get
            {
                return (Guid)this.GetValue(AppointmentDeleteGuidProperty);
            }

            set
            {
                this.SetValue(AppointmentDeleteGuidProperty, value);
            }
        }

        /// <summary>
        /// Override OnClick event.
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();

            this.RaiseDeleteAppointmentEvent();
        }

        /// <summary>
        /// Raise DeleteAppointmentEvent.
        /// </summary>
        private void RaiseDeleteAppointmentEvent()
        {
            this.OnDeleteAppointmentEvent(new RoutedEventArgs(DeleteAppointmentEvent, this));
        }

        /// <summary>
        /// Delete appointment event handler.
        /// </summary>
        /// <param name="routedEventArgs">Routed event argument.</param>
        private void OnDeleteAppointmentEvent(RoutedEventArgs routedEventArgs)
        {
            this.RaiseEvent(routedEventArgs);
        }
    }
}
