namespace System
{
    /// <summary>
    /// Represents extensions methods for System.DateTime.
    /// </summary>
    public static class DateTimeExtentions
    {
        /// <summary>
        /// Extensions method to ignore milliseconds in System.DateTime.
        /// </summary>
        /// <param name="dt">System.DateTime object.</param>
        /// <returns>Returns System.DateTime object without milliseconds.</returns>
        public static DateTime TrimMilliseconds(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0);
        }

        /// <summary>
        /// Extensions method to get date of the first day of week.
        /// </summary>
        /// <param name="dt">System.DateTime object.</param>
        /// <returns>Returns date of the first day of week.</returns>
        public static DateTime GetFirstDayOfWeek(this DateTime dt)
        {
            var dayOfWeek = (WeekDay)Enum.Parse(typeof(WeekDay), dt.DayOfWeek.ToString());

            var delta = WeekDay.Monday - dayOfWeek;
            return dt.AddDays(delta);
        }

        /// <summary>
        /// Extensions method to get date of the last day week.
        /// </summary>
        /// <param name="dt">System.DateTime object.</param>
        /// <returns>Returns date of the last day of week.</returns>
        public static DateTime GetLastDayOfWeek(this DateTime dt)
        {
            var dayOfWeek = (WeekDay)Enum.Parse(typeof(WeekDay), dt.DayOfWeek.ToString());

            var delta = WeekDay.Sunday - dayOfWeek;
            return dt.AddDays(delta);
        }
    }
}