using System;
using System.Collections.Generic;
using Common.ViewModel;
using Models;

namespace Scheduler
{
    /// <summary>
    /// Represents base view model for appointments.
    /// </summary>
    public abstract class ViewModel : ViewModelDialogBase2<AppointmentModel>
    {
        private DateTime currentDate;

        private List<string> hours;

        private List<string> minutes;

        private string selectedStartHour;

        private string selectedEndHour;

        private string selectedStartMinute;

        private string selectedEndMinute;

        private List<ItemModel> item1;

        private List<ItemModel> item2;

        private ItemModel selectedItem1;

        private ItemModel selectedItem2;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        protected ViewModel()
        {
            this.Hours = TimeHelper.Hours;
            this.Minutes = TimeHelper.Minutes;
        }

        /// <summary>
        /// Gets or sets list of the hours.
        /// </summary>
        public List<string> Hours
        {
            get
            {
                return this.hours;
            }

            set
            {
                this.hours = value;
                this.OnPropertyChanged(() => this.Hours);
            }
        }

        /// <summary>
        /// Gets or sets list of the minutes.
        /// </summary>
        public List<string> Minutes
        {
            get
            {
                return this.minutes;
            }

            set
            {
                this.minutes = value;
                this.OnPropertyChanged(() => this.Minutes);
            }
        }

        /// <summary>
        /// Gets or sets current selected date.
        /// </summary>
        public DateTime CurrentDate
        {
            get
            {
                return this.currentDate;
            }

            set
            {
                this.currentDate = value;
                this.OnPropertyChanged(() => this.CurrentDate);
            }
        }

        /// <summary>
        /// Gets or sets selected start hour.
        /// </summary>
        public string SelectedStartHour
        {
            get
            {
                return this.selectedStartHour;
            }

            set
            {
                this.selectedStartHour = value;
                this.OnPropertyChanged(() => this.SelectedStartHour);
            }
        }

        /// <summary>
        /// Gets or sets selected end hour.
        /// </summary>
        public string SelectedEndHour
        {
            get
            {
                return this.selectedEndHour;
            }

            set
            {
                this.selectedEndHour = value;
                this.OnPropertyChanged(() => this.SelectedEndHour);
            }
        }

        /// <summary>
        /// Gets or sets selected start minute.
        /// </summary>
        public string SelectedStartMinute
        {
            get
            {
                return this.selectedStartMinute;
            }

            set
            {
                this.selectedStartMinute = value;
                this.OnPropertyChanged(() => this.SelectedStartMinute);
            }
        }

        /// <summary>
        /// Gets or sets selected end minute.
        /// </summary>
        public string SelectedEndMinute
        {
            get
            {
                return this.selectedEndMinute;
            }

            set
            {
                this.selectedEndMinute = value;
                this.OnPropertyChanged(() => this.SelectedEndMinute);
            }
        }

        /// <summary>
        /// Gets or sets items 1.
        /// </summary>
        public List<ItemModel> Item1
        {
            get
            {
                return this.item1;
            }

            set
            {
                this.item1 = value;
                this.OnPropertyChanged(() => this.Item1);
            }
        }

        /// <summary>
        /// Gets or sets items 2.
        /// </summary>
        public List<ItemModel> Item2
        {
            get
            {
                return this.item2;
            }

            set
            {
                this.item2 = value;
                this.OnPropertyChanged(() => this.Item2);
            }
        }

        /// <summary>
        /// Gets or sets selected item 1.
        /// </summary>
        public ItemModel SelectedItem1
        {
            get
            {
                return this.selectedItem1;
            }

            set
            {
                this.selectedItem1 = value;
                this.Model.Item1Id = this.SelectedItem1.Id;

                this.OnPropertyChanged(() => this.SelectedItem1);
            }
        }

        /// <summary>
        /// Gets or sets selected item 2.
        /// </summary>
        public ItemModel SelectedItem2
        {
            get
            {
                return this.selectedItem2;
            }

            set
            {
                this.selectedItem2 = value;

                if (this.SelectedItem2 != null)
                {
                    this.Model.Item2Id = this.SelectedItem2.Id;
                }

                this.OnPropertyChanged(() => this.SelectedItem2);
            }
        }

        /// <summary>
        /// Initializes ViewModel class.
        /// </summary>
        /// <param name="selectedDate">Currently selected date.</param>
        /// <param name="itemStartHour">Selected start hour.</param>
        /// <param name="itemStartMinute">Selected start minute.</param>
        /// <param name="itemModel1">Selected item 1 (doctor).</param>
        protected virtual void OnAddInitialize(DateTime selectedDate, int itemStartHour, int itemStartMinute, ItemModel itemModel1)
        {
            this.CurrentDate = selectedDate;

            var startAndEndDates = this.GetStartAndEndDates(selectedDate, itemStartHour, itemStartMinute);
            var startTime = startAndEndDates.Item1;
            var endTime = startAndEndDates.Item2;

            this.PopulateDateRange(startTime, endTime);

            this.Model = new AppointmentModel
            {
                StartTime = startTime,
                EndTime = endTime
            };

            this.PopulateSelectiveItems(itemModel1);
        }

        /// <summary>
        /// Initializes ViewModel class.
        /// </summary>
        /// <param name="appointment">The appointment.</param> 
        /// <param name="itemModel1">Selected item 1 (doctor).</param>
        /// <param name="itemModel2">Selected item 2 (patient).</param>
        protected virtual void OnEditInitialize(AppointmentModel appointment, ItemModel itemModel1, ItemModel itemModel2)
        {
            this.CurrentDate = appointment.StartTime;

            this.PopulateDateRange(appointment.StartTime, appointment.EndTime);

            this.Model = appointment;

            this.PopulateSelectiveItems(itemModel1, itemModel2);
        }

        /// <summary>
        /// Populate of the selective items.
        /// </summary>
        /// <param name="selectedItem1">Selected item 1 (doctor).</param>
        /// <param name="selectedItem2">Selected item 2 (patient).</param>
        protected abstract void PopulateSelectiveItems(ItemModel selectedItem1, ItemModel selectedItem2 = null);

        /// <summary>
        /// Save an appointment.
        /// </summary>
        protected override void Handle()
        {
            var startHour = Convert.ToInt32(this.SelectedStartHour);
            var startMinute = Convert.ToInt32(this.SelectedStartMinute);
            var endHour = Convert.ToInt32(this.SelectedEndHour);
            var endMinute = Convert.ToInt32(this.SelectedEndMinute);

            this.Model.StartTime = new DateTime(this.CurrentDate.Year, this.CurrentDate.Month, this.CurrentDate.Day, startHour, startMinute, 00);
            this.Model.EndTime = new DateTime(this.CurrentDate.Year, this.CurrentDate.Month, this.CurrentDate.Day, endHour, endMinute, 00);
        }

        private Tuple<DateTime, DateTime> GetStartAndEndDates(DateTime selectedDate, int itemStartHour, int itemStartMinute)
        {
            var year = selectedDate.Year;
            var month = selectedDate.Month;
            var day = selectedDate.Day;

            int nextHour;
            int nextMinutes;

            if (itemStartHour == DateTime.MaxValue.Hour)
            {
                nextHour = itemStartHour;
                nextMinutes = 45;
            }
            else
            {
                nextHour = itemStartHour + 1;
                nextMinutes = 0;
            }

            var startTime = new DateTime(year, month, day, itemStartHour, itemStartMinute, 0);
            var endTime = new DateTime(year, month, day, nextHour, nextMinutes, 0);

            return new Tuple<DateTime, DateTime>(startTime, endTime);
        }

        private void PopulateDateRange(DateTime startTime, DateTime endTime)
        {
            this.SelectedStartHour = startTime.ToString("HH");
            this.SelectedStartMinute = startTime.ToString("mm");
            this.SelectedEndHour = endTime.ToString("HH");
            this.SelectedEndMinute = endTime.ToString("mm");
        }
    }
}
