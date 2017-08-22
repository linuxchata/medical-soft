using System;
using System.Windows;
using System.Windows.Controls;

namespace Scheduler.Logic
{
    /// <summary>
    /// Represents tome slot.
    /// </summary>
    public class TimeSlotPanel : Panel
    {
        /// <summary>
        /// Start time.
        /// </summary>
        public static readonly DependencyProperty StartTimeProperty =
            DependencyProperty.RegisterAttached(
            "StartTime",
            typeof(DateTime),
            typeof(TimeSlotPanel),
            new FrameworkPropertyMetadata(DateTime.Now));

        /// <summary>
        /// End start time.
        /// </summary>
        public static readonly DependencyProperty EndTimeProperty =
            DependencyProperty.RegisterAttached(
            "EndTime",
            typeof(DateTime),
            typeof(TimeSlotPanel),
            new FrameworkPropertyMetadata(DateTime.Now));

        /// <summary>
        /// Initializes static members of the <see cref="TimeSlotPanel"/> class.
        /// </summary>
        static TimeSlotPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeSlotPanel), new FrameworkPropertyMetadata(typeof(TimeSlotPanel)));
        }

        /// <summary>
        /// Get start time.
        /// </summary>
        /// <param name="d">Dependency object.</param>
        /// <returns>Returns start time.</returns>
        public static DateTime GetStartTime(DependencyObject d)
        {
            return Convert.ToDateTime(d.GetValue(StartTimeProperty));
        }

        /// <summary>
        /// End start time.
        /// </summary>
        /// <param name="d">Dependency object.</param>
        /// <param name="value">Date time.</param>
        public static void SetStartTime(DependencyObject d, DateTime value)
        {
            d.SetValue(StartTimeProperty, value);
        }

        /// <summary>
        /// Get end time.
        /// </summary>
        /// <param name="d">Dependency object.</param>
        /// <returns>Returns end time.</returns>
        public static DateTime GetEndTime(DependencyObject d)
        {
            return Convert.ToDateTime(d.GetValue(EndTimeProperty));
        }

        /// <summary>
        /// Set end time.
        /// </summary>
        /// <param name="d">Dependency object.</param>
        /// <param name="value">Date time.</param>
        public static void SetEndTime(DependencyObject d, DateTime value)
        {
            d.SetValue(EndTimeProperty, value);
        }

        /// <summary>
        /// Calculate size based on duration.
        /// </summary>
        /// <param name="availableSize">Available size.</param>
        /// <returns>Returns size.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size(double.PositiveInfinity, double.PositiveInfinity);

            foreach (UIElement element in this.Children)
            {
                element.Measure(size);
            }

            return new Size();
        }

        /// <summary>
        /// Calculate size of the appointment item.
        /// </summary>
        /// <param name="finalSize">Final size.</param>
        /// <returns>Returns size.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement element in this.Children)
            {
                var startTime = element.GetValue(StartTimeProperty) as DateTime?;
                var endTime = element.GetValue(EndTimeProperty) as DateTime?;

                if (!startTime.HasValue || !endTime.HasValue)
                {
                    continue;
                }

                double startMinutes = (startTime.Value.Hour * 60) + startTime.Value.Minute;
                double endMinutes = (endTime.Value.Hour * 60) + endTime.Value.Minute;
                var startOffset = (finalSize.Height / (24 * 60)) * startMinutes;
                var endOffset = (finalSize.Height / (24 * 60)) * endMinutes;

                var y = startOffset + 1;

                var width = finalSize.Width;
                var height = endOffset - startOffset - 2;

                element.Arrange(new Rect(0, y, width > 0 ? width : 0, height > 0 ? height : 0));
            }

            return finalSize;
        }
    }
}
