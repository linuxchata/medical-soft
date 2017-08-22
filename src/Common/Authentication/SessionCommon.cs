using System;

namespace Common.Authentication
{
    /// <summary>
    /// Represents session information.
    /// </summary>
    internal class SessionCommon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionCommon"/> class.
        /// </summary>
        /// <param name="name">Name of the logged-in user.</param>
        /// <param name="loginId">Id of the logged-in user.</param>
        /// <param name="isLoggined">Is user logged-in.</param>
        /// <param name="loggined">Logged-in time.</param>
        public SessionCommon(string name, int loginId, bool isLoggined, DateTime loggined)
        {
            this.LoginName = name;
            this.LoginId = loginId;
            this.IsLoggined = isLoggined;
            this.Loggined = loggined;
        }

        /// <summary>
        /// Gets login name.
        /// </summary>
        public string LoginName { get; private set; }

        /// <summary>
        /// Gets login id.
        /// </summary>
        public int LoginId { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the item is user logged-in.
        /// </summary>
        public bool IsLoggined { get; private set; }

        /// <summary>
        /// Gets time of the logged-in.
        /// </summary>
        public DateTime Loggined { get; private set; }
    }
}
