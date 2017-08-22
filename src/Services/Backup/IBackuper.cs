using Models.Enumeration;

namespace Services.Backup
{
    /// <summary>
    /// Represents interface to perform backup of the database.
    /// </summary>
    public interface IBackuper
    {
        /// <summary>
        /// Perform backup of the database.
        /// </summary>
        /// <param name="backupType">Type of backup.</param>
        void PerformBackup(BackuperType backupType);
    }
}
