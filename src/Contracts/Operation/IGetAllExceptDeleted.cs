using System.Collections.Generic;

namespace Contracts.Operation
{
    /// <summary>
    /// Represents get all except deleted interface for working with database.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public interface IGetAllExceptDeleted<out T> where T : class
    {
        /// <summary>
        /// Get all records except deleted record.
        /// </summary> 
        /// <returns>List of all non-deleted records.</returns>
        IEnumerable<T> GetAllExceptDeleted();
    }
}
