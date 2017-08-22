using System.Windows;
using System.Windows.Controls;

namespace Scheduler.Logic
{
    /// <summary>
    /// Represents ScrollViewer with CustomVerticalOffset property for setting vertical and horizontal position of the ScrollViewer.
    /// </summary>
    public class Scroll : ScrollViewer
    {
        /// <summary>
        /// Vertical position of the scroll viewer.
        /// </summary>
        public static readonly DependencyProperty CustomVerticalOffsetProperty =
            DependencyProperty.Register(
            "CustomVerticalOffset",
            typeof(double),
            typeof(Scroll),
            new FrameworkPropertyMetadata(0.0, OnVerticalOffsetChanged));

        /// <summary>
        /// Horizontal position of the scroll viewer.
        /// </summary>
        public static readonly DependencyProperty CustomHorisontalOffsetProperty =
            DependencyProperty.Register(
            "CustomHorisontalOffset",
            typeof(double),
            typeof(Scroll),
            new FrameworkPropertyMetadata(0.0, OnHorisontalOffsetChanged));

        /// <summary>
        /// Initializes static members of the Scroll class.
        /// </summary>
        static Scroll()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Scroll), new FrameworkPropertyMetadata(typeof(Scroll)));
        }

        /// <summary>
        /// Gets or sets vertical position of the scroll viewer.
        /// </summary>
        public double CustomVerticalOffset
        {
            get
            {
                return (double)this.GetValue(CustomVerticalOffsetProperty);
            }

            set
            {
                this.SetValue(CustomVerticalOffsetProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets horizontal position of the scroll viewer.
        /// </summary>
        public double CustomHorisontalOffset
        {
            get
            {
                return (double)this.GetValue(CustomHorisontalOffsetProperty);
            }

            set
            {
                this.SetValue(CustomHorisontalOffsetProperty, value);
            }
        }

        /// <summary>
        /// Vertical position of the scroll viewer changed handler.
        /// </summary>
        /// <param name="d">DependencyObject as ScrollViewer.</param>
        /// <param name="e">New and old value of the property.</param>
        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = ((ScrollViewer)d).FindName("scrollViewer") as ScrollViewer;

            if (viewer != null)
            {
                viewer.ScrollToVerticalOffset((double)e.NewValue);
            }
        }

        /// <summary>
        /// Horizontal position of the scroll viewer changed handler.
        /// </summary>
        /// <param name="d">DependencyObject as ScrollViewer.</param>
        /// <param name="e">New and old value of the property.</param>
        private static void OnHorisontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = ((ScrollViewer)d).FindName("scrollViewer") as ScrollViewer;

            if (viewer != null)
            {
                viewer.ScrollToHorizontalOffset((double)e.NewValue);
            }
        }
    }
}
