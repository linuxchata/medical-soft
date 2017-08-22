using System.Collections.Generic;

namespace Contracts.Operation
{
    /// <summary>
    /// Represents get all interface for working with database.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public interface IGetAll<out T> where T : class
    {
        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>List of all records.</returns>
        IEnumerable<T> GetAll();
    }
}
