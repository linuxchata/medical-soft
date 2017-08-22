using System;

namespace Common.Authentication
{
    /// <summary>
    /// Represents authentication information interface.
    /// </summary>
    public interface IAuthenticationSession
    {
        /// <summary>
        /// Create a new authentication session.
        /// </summary>
        /// <param name="loginName">Name of the logged-in user.</param>
        /// <param name="loginId">Id of the logged-in user.</param>
        /// <param name="isLoggined">Is user logged-in.</param>
        /// <param name="logginedTime">Logged-in time.</param>
        void CreateSession(string loginName, int loginId, bool isLoggined, DateTime logginedTime);

        /// <summary>
        /// Get name of the logged-in user.
        /// </summary>
        /// <returns>User's name.</returns>
        string GetUser();

        /// <summary>
        /// Get id of the logged-in user.
        /// </summary>
        /// <returns>User's id.</returns>
        int GetUserId();
    }
}