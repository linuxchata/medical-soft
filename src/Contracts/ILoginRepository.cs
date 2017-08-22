using System.Collections.Generic;
using System.Security;
using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents login repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface ILoginRepository<TCommon, in TKey> : IRepository<TCommon, TKey> where TCommon : class
    {
        /// <summary>
        /// Is login information valid.
        /// </summary>
        /// <param name="login">Login name.</param>
        /// <param name="password">The password.</param>
        /// <param name="loginId">Login id.</param>
        /// <returns>Returns is login information valid.</returns>
        bool IsLoginValid(string login, SecureString password, ref int loginId);

        /// <summary>
        /// Deactivate and hide logins by staff Id
        /// </summary>
        /// <param name="staffId">Identification of the staff.</param>
        void DeactivateAndHideLoginsByStaffId(int staffId);

        /// <summary>
        /// Get all records include not mapped (not mapped records were created during the installation).
        /// </summary>
        /// <returns>List of all records.</returns>
        IEnumerable<TCommon> GetAllIncludeNotMapped();

        /// <summary>
        /// Get all records except deleted record.
        /// </summary>
        /// <returns>List of all records.</returns>
        IEnumerable<TCommon> GetAllSuperAdministratorsExceptDeleted();
    }
}
