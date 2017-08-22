using System.Collections.Generic;
using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents notification template repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the notification template model.</typeparam>
    /// <typeparam name="TCommon2">Common class that represents the item model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface INotificationTemplateRepository<TCommon, TCommon2, in TKey> : IRepository<TCommon, TKey>
        where TCommon : class
        where TCommon2 : class
    {
        /// <summary>
        /// Get all records except deleted for drop down list.
        /// </summary>
        /// <returns>List of all records except deleted.</returns>
        List<TCommon2> GetAllForList();
    }
}
