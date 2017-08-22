using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents system updates repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    public interface ISystemUpdatesRepository<out TCommon> : IGetAll<TCommon> where TCommon : class
    {
        /// <summary>
        /// Get current database version. 
        /// </summary>
        /// <returns>T class.</returns>
        TCommon GetDatabaseVersion();
    }
}
