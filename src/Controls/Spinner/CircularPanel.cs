using System;
using System.Windows;
using System.Windows.Controls;

namespace Spinner
{
    /// <summary>
    /// Represents circular panel.
    /// </summary>
    public class CircularPanel : Panel
    {
        /// <summary>
        /// Measure size in a circle.
        /// </summary>
        /// <param name="availableSize">Available size.</param>
        /// <returns>Return measure size.</returns>
        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
        {
            foreach (UIElement child in this.Children)
            {
                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            return base.MeasureOverride(availableSize);
        }

        /// <summary>
        /// Arrange size in a circle.
        /// </summary>
        /// <param name="finalSize">Final size.</param>
        /// <returns>Return arranged size.</returns>
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize)
        {
            if (this.Children.Count > 0)
            {
                var size = new System.Windows.Size(finalSize.Width - 6, finalSize.Height - 6);

                // Center & radius of panel.
                var center = new Point(size.Width / 2, size.Height / 2);
                var radius = Math.Min(size.Width, size.Height) / 2.0;
                radius *= 0.95;

                // Radians between children.
                var angleIncrRadians = 2.0 * Math.PI / this.Children.Count;

                var angleInRadians = 0.0;

                foreach (UIElement child in this.Children)
                {
                    var childPosition = new Point(
                        (radius * Math.Cos(angleInRadians)) + center.X,
                        (radius * Math.Sin(angleInRadians)) + center.Y);

                    child.Arrange(new Rect(childPosition, child.DesiredSize));

                    angleInRadians += angleIncrRadians;
                }
            }

            return finalSize;
        }
    }
}
