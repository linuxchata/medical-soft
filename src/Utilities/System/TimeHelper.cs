using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// Represents static class for hours and minutes.
    /// </summary>
    public static class TimeHelper
    {
        private static List<string> hours;

        private static List<string> minutes;

        /// <summary>
        /// Gets list of the hours
        /// </summary>
        public static List<string> Hours
        {
            get
            {
                if (hours == null)
                {
                    hours = new List<string>();

                    var date = new DateTime(0001, 1, 1, 00, 00, 0);

                    for (var i = 0; i <= DateTime.MaxValue.Hour; i++)
                    {
                        hours.Add(date.ToString("HH"));
                        date = date.AddHours(1);
                    }
                }

                return hours;
            }
        }

        /// <summary>
        /// Gets list of the minutes
        /// </summary>
        public static List<string> Minutes
        {
            get { return minutes ?? (minutes = new List<string> { "00", "15", "30", "45" }); }
        }
    }
}