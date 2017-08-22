using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Contracts;

namespace Scheduler.Logic
{
    /// <summary>
    /// Represents calendar.
    /// </summary>
    public class Calendar : Control
    {
        /// <summary>
        /// Appointments property for binding calendar by list of the appointments
        /// </summary>
        public static readonly DependencyProperty AppointmentsProperty =
            DependencyProperty.Register(
            "Appointments",
            typeof(IEnumerable<IModel>),
            typeof(Calendar),
            new FrameworkPropertyMetadata(null, OnAppointmentsChanged));

        /// <summary>
        /// Property represents selected doctor.
        /// </summary>
        public static readonly DependencyProperty SelectedDoctorProperty =
            DependencyProperty.Register(
            "SelectedDoctor",
            typeof(string),
            typeof(Calendar),
            new FrameworkPropertyMetadata(null, OnSelectedDoctorChanged));

        /// <summary>
        /// Current day.
        /// </summary>
        public static readonly DependencyProperty CurrentDayProperty =
            DependencyProperty.Register(
            "CurrentDay",
            typeof(DateTime),
            typeof(Calendar),
            new FrameworkPropertyMetadata(DateTime.Now, OnCurrentDayChanged));

        /// <summary>
        /// The culture.
        /// </summary>
        public static readonly DependencyProperty CultureProperty =
            DependencyProperty.Register(
            "Culture",
            typeof(string),
            typeof(Calendar),
            new FrameworkPropertyMetadata(string.Empty, OnCultureChanged));

        /// <summary>
        /// Add appointment event.
        /// </summary>
        public static readonly RoutedEvent AddAppointmentEvent = TimeSlot.AddAppointmentEvent.AddOwner(typeof(SchedulerDay));

        /// <summary>
        /// Edit appointment event.
        /// </summary>
        public static readonly RoutedEvent EditAppointmentEvent = AppointmentItem.EditAppointmentEvent.AddOwner(typeof(AppointmentItem));

        /// <summary>
        /// Close appointment event.
        /// </summary>
        public static readonly RoutedEvent DeleteAppointmentEvent = DeleteButton.DeleteAppointmentEvent.AddOwner(typeof(AppointmentItem));

        /// <summary>
        /// Next day commands.
        /// </summary>
        public static readonly RoutedCommand NextDay = new RoutedCommand("NextDay", typeof(Calendar));

        /// <summary>
        /// Previous day commands.
        /// </summary>
        public static readonly RoutedCommand PreviousDay = new RoutedCommand("PreviousDay", typeof(Calendar));

        /// <summary>
        /// Initializes static members of the Calendar class.
        /// </summary>
        static Calendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(typeof(Calendar)));

            CommandManager.RegisterClassCommandBinding(typeof(Calendar), new CommandBinding(NextDay, OnExecutedNextDay, OnCanExecuteNextDay));
            CommandManager.RegisterClassCommandBinding(typeof(Calendar), new CommandBinding(PreviousDay, OnExecutedPreviousDay, OnCanExecutePreviousDay));
        }

        /// <summary>
        /// Add appointment event.
        /// </summary>
        public event RoutedEventHandler AddAppointment
        {
            add
            {
                this.AddHandler(Calendar.AddAppointmentEvent, value);
            }

            remove
            {
                this.RemoveHandler(Calendar.AddAppointmentEvent, value);
            }
        }

        /// <summary>
        /// Edit appointment event.
        /// </summary>
        public event RoutedEventHandler EditAppointment
        {
            add
            {
                this.AddHandler(Calendar.EditAppointmentEvent, value);
            }

            remove
            {
                this.RemoveHandler(Calendar.EditAppointmentEvent, value);
            }
        }

        /// <summary>
        /// Delete appointment event.
        /// </summary>
        public event RoutedEventHandler DeleteAppointment
        {
            add
            {
                this.AddHandler(Calendar.DeleteAppointmentEvent, value);
            }

            remove
            {
                this.RemoveHandler(Calendar.DeleteAppointmentEvent, value);
            }
        }

        /// <summary>
        /// Gets or sets appointments property for binding calendar by list of the appointments.
        /// </summary>
        public IEnumerable<IModel> Appointments
        {
            get
            {
                return (IEnumerable<IModel>)this.GetValue(Calendar.AppointmentsProperty);
            }

            set
            {
                this.SetValue(Calendar.AppointmentsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets property represents selected doctor.
        /// </summary>
        public string SelectedDoctor
        {
            get
            {
                return (string)this.GetValue(Calendar.SelectedDoctorProperty);
            }

            set
            {
                this.SetValue(Calendar.SelectedDoctorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets property represents culture.
        /// </summary>
        public string Culture
        {
            get
            {
                return (string)this.GetValue(Calendar.CultureProperty);
            }

            set
            {
                this.SetValue(Calendar.CultureProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets current Day.
        /// </summary>
        public DateTime CurrentDay
        {
            get
            {
                return (DateTime)this.GetValue(Calendar.CurrentDayProperty);
            }

            set
            {
                this.SetValue(Calendar.CurrentDayProperty, value);
            }
        }

        /// <summary>
        /// Set states and properties of elements when they are loaded.
        /// </summary>
        public override void OnApplyTemplate()
        {
            this.FilterAppointments();

            this.UpdateSelectedItem();
        }

        /// <summary>
        /// Filter and refresh appointments for current day.
        /// </summary>
        public void FilterAppointments()
        {
            var bydate = this.CurrentDay;
            var day = this.GetTemplateChild("Day") as SchedulerDay;
            var dayHeader = this.GetTemplateChild("DayHeader") as TextBlock;

            if (day != null)
            {
                var items = this.Appointments.ByDate(bydate);
                day.ItemsSource = items;
            }

            if (dayHeader != null)
            {
                dayHeader.Text = string.Format("{0}, {1} ", bydate.DayOfWeek, bydate.ToShortDateString());
            }
        }

        /// <summary>
        /// AppointmentsChanged event handler.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected virtual void OnAppointmentsChanged(DependencyPropertyChangedEventArgs e)
        {
            this.FilterAppointments();
        }

        /// <summary>
        /// AppointmentsChanged event handler.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected virtual void OnSelectedDoctorChanged(DependencyPropertyChangedEventArgs e)
        {
            this.UpdateSelectedItem();
        }

        /// <summary>
        /// CurrentDayChanged event handler.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected virtual void OnCurrentDayChanged(DependencyPropertyChangedEventArgs e)
        {
            this.FilterAppointments();
        }

        /// <summary>
        /// ExecutedNextDay event handler.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected virtual void OnExecutedNextDay(ExecutedRoutedEventArgs e)
        {
            this.CurrentDay += TimeSpan.FromDays(1);
            e.Handled = true;
        }

        /// <summary>
        /// CanExecuteNextDay event handler.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected virtual void OnCanExecuteNextDay(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = false;
        }

        /// <summary>
        /// ExecutedPreviousDay event handler.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected virtual void OnExecutedPreviousDay(ExecutedRoutedEventArgs e)
        {
            this.CurrentDay -= TimeSpan.FromDays(1);
            e.Handled = true;
        }

        /// <summary>
        /// CanExecutePreviousDay event handler.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected virtual void OnCanExecutePreviousDay(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = false;
        }

        /// <summary>
        /// CultureChanged event handler.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected virtual void OnCultureChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e != null)
            {
                var value = e.NewValue.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    var cultureInfo = new CultureInfo(value);
                    Properties.Resources.Culture = cultureInfo;
                }
            }
        }

        /// <summary>
        /// AppointmentsChanged event handler.
        /// </summary>
        /// <param name="d">Dependent object.</param>
        /// <param name="e">Event argument.</param>
        private static void OnAppointmentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Calendar)d).OnAppointmentsChanged(e);
        }

        /// <summary>
        /// SelectedDoctorChanged event handler.
        /// </summary>
        /// <param name="d">Dependent object.</param>
        /// <param name="e">Event argument.</param>
        private static void OnSelectedDoctorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Calendar)d).OnSelectedDoctorChanged(e);
        }

        /// <summary>
        /// CurrentDayChanged event handler.
        /// </summary>
        /// <param name="d">Dependency object.</param>
        /// <param name="e">Event argument.</param>
        private static void OnCurrentDayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Calendar)d).OnCurrentDayChanged(e);
        }

        /// <summary>
        /// CanExecuteNextDay event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Event argument.</param>
        private static void OnCanExecuteNextDay(object sender, CanExecuteRoutedEventArgs e)
        {
            ((Calendar)sender).OnCanExecuteNextDay(e);
        }

        /// <summary>
        /// ExecutedNextDay event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Event argument.</param>
        private static void OnExecutedNextDay(object sender, ExecutedRoutedEventArgs e)
        {
            ((Calendar)sender).OnExecutedNextDay(e);
        }

        /// <summary>
        /// ExecutedPreviousDay event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Event argument.</param>
        private static void OnExecutedPreviousDay(object sender, ExecutedRoutedEventArgs e)
        {
            ((Calendar)sender).OnExecutedPreviousDay(e);
        }

        /// <summary>
        /// CanExecutePreviousDay event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Event argument.</param>
        private static void OnCanExecutePreviousDay(object sender, CanExecuteRoutedEventArgs e)
        {
            ((Calendar)sender).OnCanExecutePreviousDay(e);
        }

        /// <summary>
        /// CultureChanged event handler.
        /// </summary>
        /// <param name="d">Dependency object.</param>
        /// <param name="e">Event argument.</param>
        private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Calendar)d).OnCultureChanged(e);
        }

        /// <summary>
        /// Update selected item.
        /// </summary>
        private void UpdateSelectedItem()
        {
            var header = this.GetTemplateChild("Header") as TextBlock;

            if (header != null)
            {
                header.Text = this.SelectedDoctor;
            }
        }
    }
}
