using System.Collections.Generic;
using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents reminder repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface IReminderRepository<TCommon, in TKey> : IRepository<TCommon, TKey> where TCommon : class
    {
        /// <summary>
        /// Get today's reminders for notification.
        /// </summary>
        /// <returns>Returns list of today's reminders for notification.</returns>
        IEnumerable<TCommon> GetActiveReminders();
    }
}
