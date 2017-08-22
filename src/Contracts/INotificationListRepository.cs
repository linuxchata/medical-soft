using System.Collections.Generic;
using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents notification list repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface INotificationListRepository<TCommon, in TKey> : IGet<TCommon, TKey>, IAdd<TCommon>, IUpdate<TCommon>, IDelete<TKey> where TCommon : class
    {
        /// <summary>
        /// Get all records except deleted record by notification group id.
        /// Ordered by patient name.
        /// </summary>
        /// <param name="groupId">Id of the group.</param>
        /// <returns>List of all non-deleted records.</returns>
        IEnumerable<TCommon> GetAllExceptDeletedByGroupId(TKey groupId);
    }
}
