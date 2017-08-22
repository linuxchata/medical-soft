using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Spinner
{
    /// <summary>
    /// Represents spinner.
    /// </summary>
    public class Spinner : Control
    {
        /// <summary>
        /// Background color.
        /// </summary>
        public static readonly DependencyProperty SpinnerBackgroundProperty =
            DependencyProperty.Register("SpinnerBackground", typeof(Brush), typeof(Spinner), new PropertyMetadata(null));

        /// <summary>
        /// Initializes static members of the Spinner class.
        /// </summary>
        static Spinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Spinner), new FrameworkPropertyMetadata(typeof(Spinner)));
        }

        /// <summary>
        /// Gets or sets background color.
        /// </summary>
        public Brush SpinnerBackground
        {
            get
            {
                return (Brush)GetValue(Spinner.SpinnerBackgroundProperty);
            }

            set
            {
                this.SetValue(Spinner.SpinnerBackgroundProperty, value);
            }
        }

        /// <summary>
        /// On apply template event handler.
        /// </summary>
        public override void OnApplyTemplate()
        {
            const string Ellipse = "ellipse";
            const int Duration = 50;
            const int NumberOfElements = 8;
            const double Speed = 0.15;

            base.OnApplyTemplate();

            var listBox = GetTemplateChild("listBox") as ListBox;
            var storyboard = GetTemplateChild("storyboard") as Storyboard;

            if (listBox == null)
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("ListBox is null.");
            }

            if (storyboard == null)
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("Storyboard is null.");
            }

            // Get all animated elements.
            var ellipses = new List<Ellipse>();
            for (var i = 1; i <= NumberOfElements; i++)
            {
                var ellipse = GetTemplateChild(Ellipse + i) as Ellipse;

                if (ellipse == null)
                {
                    // ReSharper disable once NotResolvedInText
                    throw new ArgumentNullException("Ellipse is null.");
                }

                ellipse.Fill = new SolidColorBrush(Colors.Gray);

                ellipses.Add(ellipse);
            }

            // Create a name scope.
            NameScope.SetNameScope(this, new NameScope());

            double time = 0;
            for (var i = 0; i < 8; i++)
            {
                time += Speed;
                var previous = i == 0 ? NumberOfElements - 1 : i - 1;
                var previous2 = (previous - 1) == -1 ? NumberOfElements - 1 : previous - 1;
                var next = i + 1 == NumberOfElements ? 0 : i + 1;
                var next2 = (next + 1) == NumberOfElements ? 0 : (next + 1);

                var animation1 = new DoubleAnimation
                {
                    To = 3,
                    Duration = new Duration(TimeSpan.FromMilliseconds(Duration)),
                    BeginTime = TimeSpan.FromSeconds(time)
                };
                var animation2 = animation1.Clone();
                Storyboard.SetTargetName(animation2, ellipses[previous2].Name);
                Storyboard.SetTargetProperty(animation2, new PropertyPath(FrameworkElement.WidthProperty));
                Storyboard.SetTargetName(animation1, ellipses[previous2].Name);
                Storyboard.SetTargetProperty(animation1, new PropertyPath(FrameworkElement.HeightProperty));

                var animation3 = new DoubleAnimation
                {
                    To = 4,
                    Duration = new Duration(TimeSpan.FromMilliseconds(Duration)),
                    BeginTime = TimeSpan.FromSeconds(time)
                };
                var animation4 = animation3.Clone();
                Storyboard.SetTargetName(animation3, ellipses[previous].Name);
                Storyboard.SetTargetProperty(animation3, new PropertyPath(FrameworkElement.WidthProperty));
                Storyboard.SetTargetName(animation4, ellipses[previous].Name);
                Storyboard.SetTargetProperty(animation4, new PropertyPath(FrameworkElement.HeightProperty));

                var animation5 = new DoubleAnimation
                {
                    To = 5,
                    Duration = new Duration(TimeSpan.FromMilliseconds(Duration)),
                    BeginTime = TimeSpan.FromSeconds(time)
                };
                var animation6 = animation5.Clone();
                Storyboard.SetTargetName(animation5, ellipses[i].Name);
                Storyboard.SetTargetProperty(animation5, new PropertyPath(FrameworkElement.WidthProperty));
                Storyboard.SetTargetName(animation6, ellipses[i].Name);
                Storyboard.SetTargetProperty(animation6, new PropertyPath(FrameworkElement.HeightProperty));

                var animation7 = new DoubleAnimation
                {
                    To = 1,
                    Duration = new Duration(TimeSpan.FromMilliseconds(Duration)),
                    BeginTime = TimeSpan.FromSeconds(time)
                };
                var animation8 = animation7.Clone();
                Storyboard.SetTargetName(animation7, ellipses[next].Name);
                Storyboard.SetTargetProperty(animation7, new PropertyPath(FrameworkElement.WidthProperty));
                Storyboard.SetTargetName(animation8, ellipses[next].Name);
                Storyboard.SetTargetProperty(animation8, new PropertyPath(FrameworkElement.HeightProperty));

                var animation9 = new DoubleAnimation
                {
                    To = 2,
                    Duration = new Duration(TimeSpan.FromMilliseconds(Duration)),
                    BeginTime = TimeSpan.FromSeconds(time)
                };
                var animation10 = animation9.Clone();
                Storyboard.SetTargetName(animation9, ellipses[next2].Name);
                Storyboard.SetTargetProperty(animation9, new PropertyPath(FrameworkElement.WidthProperty));
                Storyboard.SetTargetName(animation10, ellipses[next2].Name);
                Storyboard.SetTargetProperty(animation10, new PropertyPath(FrameworkElement.HeightProperty));

                storyboard.Children.Add(animation1);
                storyboard.Children.Add(animation2);
                storyboard.Children.Add(animation3);
                storyboard.Children.Add(animation4);
                storyboard.Children.Add(animation5);
                storyboard.Children.Add(animation6);
                storyboard.Children.Add(animation7);
                storyboard.Children.Add(animation8);
                storyboard.Children.Add(animation9);
                storyboard.Children.Add(animation10);
            }

            storyboard.Begin(listBox);
        }
    }
}
