using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents Backup repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface IBackupRepository<TCommon, in TKey> : IGetAll<TCommon>, IGetAllExceptDeleted<TCommon>, IAdd<TCommon>, IDelete<TKey> where TCommon : class
    {
        /// <summary>
        /// Perform backup of the database.
        /// </summary>
        /// <param name="nameOfDb">Name of the database.</param>
        /// <param name="locationOfTheBackup">Location of the backup.</param>
        /// <returns>Returns resulted string.</returns>
        string PerformBackup(string nameOfDb, string locationOfTheBackup);
    }
}
