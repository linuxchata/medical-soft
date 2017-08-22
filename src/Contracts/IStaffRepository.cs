using System.Collections.Generic;
using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents staff repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the staff model.</typeparam>
    /// <typeparam name="TCommon2">Common class that represents the item model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface IStaffRepository<TCommon, TCommon2, in TKey> : IRepository<TCommon, TKey>
        where TCommon : class
        where TCommon2 : class
    {
        /// <summary>
        /// Get only taking staff for drop down list.
        /// </summary>
        /// <returns>List of all records.</returns>
        List<TCommon2> GetAllIsTakingForList();
    }
}
