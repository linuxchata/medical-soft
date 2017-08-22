using System;
using System.Collections.Generic;
using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents notification group repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface INotificationGroupRepository<TCommon, in TKey> : IRepository<TCommon, TKey> where TCommon : class
    {
        /// <summary>
        /// Get active (not completed) records except deleted record.
        /// </summary> 
        /// <returns>List of active non-deleted records.</returns>
        IEnumerable<TCommon> GetActiveExceptDeleted();

        /// <summary>
        /// Get record by unique identifier.
        /// </summary>
        /// <param name="id">Unique identifier of the record.</param>
        /// <returns>T class.</returns>
        TCommon GetByGuidId(Guid id);
    }
}
