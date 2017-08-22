using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;

namespace Scheduler.Logic
{
    /// <summary>
    /// Represents filter.
    /// </summary>
    public static class Filter
    {
        /// <summary>
        /// Extension method for Appointments.
        /// </summary>
        /// <param name="appointments">The appointments.</param>
        /// <param name="date">The date.</param>
        /// <returns>Returns list of the appointments for current date.</returns>
        public static IEnumerable<IModel> ByDate(this IEnumerable<IModel> appointments, DateTime date)
        {
            IEnumerable<IModel> app = null;

            if (appointments != null)
            {
                app = from a in appointments
                      where a.StartTime.ToShortDateString() == date.ToShortDateString()
                      select a;
            }

            return app;
        }
    }
}
