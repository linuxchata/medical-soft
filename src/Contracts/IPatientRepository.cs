using System.Collections.Generic;
using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents patient repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the patient model.</typeparam>
    /// <typeparam name="TCommon2">Common class that represents the item model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface IPatientRepository<TCommon, TCommon2, in TKey> : IRepository<TCommon, TKey>
        where TCommon : class
        where TCommon2 : class
    {
        /// <summary>
        /// Get patients for drop down list.
        /// </summary>
        /// <returns>Returns list of all patients for drop down list.</returns>
        List<TCommon2> GetAllForList();

        /// <summary>
        /// Search patients for drop down list.
        /// </summary>
        /// <param name="searchPattern">The search pattern.</param>
        /// <returns>Returns IEnumerable of patients for drop down list.</returns>
        List<TCommon2> SearchForList(string searchPattern);
    }
}
