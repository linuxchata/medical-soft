using System;

namespace Common.Authentication
{
    /// <summary>
    /// Represents authentication information.
    /// </summary>
    public sealed class AuthenticationSession : IAuthenticationSession
    {
        private static readonly object SyncRoot = new object();

        private static volatile AuthenticationSession instance;

        private SessionCommon sessionCommon;

        private AuthenticationSession()
        {
            this.sessionCommon = new SessionCommon(string.Empty, 0, false, DateTime.Now);
        }

        /// <summary>
        /// Gets instance of the authentication service.
        /// </summary>
        public static AuthenticationSession Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRoot)
                    {
                        instance = new AuthenticationSession();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Create a new authentication session.
        /// </summary>
        /// <param name="loginName">Name of the logged-in user.</param>
        /// <param name="loginId">Id of the logged-in user.</param>
        /// <param name="isLoggined">Is user logged-in.</param>
        /// <param name="logginedTime">Logged-in time.</param>
        public void CreateSession(string loginName, int loginId, bool isLoggined, DateTime logginedTime)
        {
            this.sessionCommon = new SessionCommon(loginName, loginId, isLoggined, logginedTime);
        }

        /// <summary>
        /// Get name of the logged-in user.
        /// </summary>
        /// <returns>User's name.</returns>
        public string GetUser()
        {
            if (this.sessionCommon.IsLoggined)
            {
                return this.sessionCommon.LoginName;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get id of the logged-in user.
        /// </summary>
        /// <returns>User's id.</returns>
        public int GetUserId()
        {
            if (this.sessionCommon.IsLoggined)
            {
                return this.sessionCommon.LoginId;
            }

            return 0;
        }
    }
}
