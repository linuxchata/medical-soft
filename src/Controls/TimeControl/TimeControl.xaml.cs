using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Common.Commands;

namespace TimeControl
{
    /// <summary>
    /// Interaction logic for TimeControl
    /// </summary>
    public partial class TimeControl
    {
        /// <summary>
        /// List of the hours.
        /// </summary>
        public static readonly DependencyProperty HoursProperty = DependencyProperty.Register(
            "Hours",
            typeof(List<string>),
            typeof(TimeControl));

        /// <summary>
        /// List of the minutes.
        /// </summary>
        public static readonly DependencyProperty MinutesProperty = DependencyProperty.Register(
            "Minutes",
            typeof(List<string>),
            typeof(TimeControl));

        /// <summary>
        /// Selected hour.
        /// </summary>
        public static readonly DependencyProperty SelectedHourProperty = DependencyProperty.Register(
            "SelectedHour",
            typeof(string),
            typeof(TimeControl));

        /// <summary>
        /// Selected minute.
        /// </summary>
        public static readonly DependencyProperty SelectedMinuteProperty = DependencyProperty.Register(
            "SelectedMinute",
            typeof(string),
            typeof(TimeControl));

        /// <summary>
        /// Up command.
        /// </summary>
        private ICommand increaseCommand;

        /// <summary>
        /// Down command.
        /// </summary>
        private ICommand decreaseCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeControl"/> class.
        /// </summary>
        public TimeControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets selected hour.
        /// </summary>
        public string SelectedHour
        {
            get
            {
                return (string)this.GetValue(SelectedHourProperty);
            }

            set
            {
                this.SetValue(SelectedHourProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets selected minute.
        /// </summary>
        public string SelectedMinute
        {
            get
            {
                return (string)this.GetValue(SelectedMinuteProperty);
            }

            set
            {
                this.SetValue(SelectedMinuteProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets hours.
        /// </summary>
        public List<string> Hours
        {
            get
            {
                return (List<string>)this.GetValue(HoursProperty);
            }

            set
            {
                this.SetValue(HoursProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets minutes.
        /// </summary>
        public List<string> Minutes
        {
            get
            {
                return (List<string>)this.GetValue(MinutesProperty);
            }

            set
            {
                this.SetValue(MinutesProperty, value);
            }
        }

        /// <summary>
        /// Gets increase command.
        /// </summary>
        public ICommand IncreaseCommand
        {
            get
            {
                return this.increaseCommand ?? (this.increaseCommand = new CommonCommand(
                    param =>
                    {
                        var result = Up(this.Minutes, this.SelectedMinute);
                        this.SelectedMinute = result.Item1;
                        if (result.Item2)
                        {
                            this.SelectedHour = Up(this.Hours, this.SelectedHour).Item1;
                        }
                    },
                    param => true));
            }
        }

        /// <summary>
        /// Gets decrease command.
        /// </summary>
        public ICommand DecreaseCommand
        {
            get
            {
                return this.decreaseCommand ?? (this.decreaseCommand = new CommonCommand(
                    param =>
                    {
                        var result = Down(this.Minutes, this.SelectedMinute);
                        this.SelectedMinute = result.Item1;
                        if (result.Item2)
                        {
                            this.SelectedHour = Down(this.Hours, this.SelectedHour).Item1;
                        }
                    },
                    param => true));
            }
        }

        /// <summary>
        /// Increase time.
        /// </summary>
        /// <param name="array">Array with time items.</param>
        /// <param name="selectedItem">Selected item.</param>
        /// <returns>Returns a new selected item.</returns>
        private static Tuple<string, bool> Up(IList<string> array, string selectedItem)
        {
            var newSelectedItem = selectedItem;
            var @continue = false;
            for (var i = 0; i < array.Count; i++)
            {
                if (selectedItem == array[i])
                {
                    if (i != array.Count - 1)
                    {
                        newSelectedItem = array[i + 1];
                    }
                    else
                    {
                        newSelectedItem = array[0];
                        @continue = true;
                    }

                    break;
                }
            }

            return new Tuple<string, bool>(newSelectedItem, @continue);
        }

        /// <summary>
        /// Decrease time.
        /// </summary>
        /// <param name="array">Array with time items.</param>
        /// <param name="selectedItem">Selected item.</param>
        /// <returns>Returns a new selected item.</returns>
        private static Tuple<string, bool> Down(IList<string> array, string selectedItem)
        {
            var newSelectedItem = selectedItem;
            var @continue = false;
            for (var i = array.Count - 1; i >= 0; i--)
            {
                if (selectedItem == array[i])
                {
                    if (i != 0)
                    {
                        newSelectedItem = array[i - 1];
                    }
                    else
                    {
                        newSelectedItem = array[array.Count - 1];
                        @continue = true;
                    }

                    break;
                }
            }

            return new Tuple<string, bool>(newSelectedItem, @continue);
        }
    }
}
