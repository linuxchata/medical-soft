using System;
using System.Collections.Generic;

namespace Common.Communication
{
    /// <summary>
    /// Represents save changes response.
    /// </summary>
    public class SaveChangesResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveChangesResponse"/> class.
        /// </summary>
        public SaveChangesResponse()
        {
            this.Result = new Dictionary<string, int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveChangesResponse"/> class.
        /// </summary>
        /// <param name="exception">Exception details if any.</param>
        public SaveChangesResponse(Exception exception)
            : this()
        {
            this.Exception = exception;
        }

        /// <summary>
        /// Gets result.
        /// </summary>
        public Dictionary<string, int> Result { get; private set; }

        /// <summary>
        /// Gets a value indicating whether operation was successful.
        /// </summary>
        public bool IsSuccessful
        {
            get
            {
                return this.Exception == null;
            }
        }

        /// <summary>
        /// Gets exception details if any.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Try add result.
        /// </summary>
        /// <param name="entityName">Database entity.</param>
        /// <param name="id">Id of the record.</param>
        public void TryAddResult(string entityName, int id)
        {
            if (entityName.IsNullOrEmpty())
            {
                throw new ArgumentNullException("entityName", "Entity name cannot be null.");
            }

            if (!this.Result.ContainsKey(entityName))
            {
                this.Result.Add(entityName, id);
            }
            else
            {
                throw new InvalidOperationException("Entity with the same name (key) already added.");
            }
        }

        /// <summary>
        /// Try get value.
        /// </summary>
        /// <param name="entityName">Database entity.</param>
        /// <returns>Returns id of the record.</returns>
        public int TryGetValue(string entityName)
        {
            if (entityName.IsNullOrEmpty())
            {
                throw new ArgumentNullException("entityName", "Entity name cannot be null.");
            }

            int result;
            this.Result.TryGetValue(entityName, out result);

            return result;
        }

        /// <summary>
        /// Set exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void SetException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception", "Exception cannot be null.");
            }

            this.Exception = exception;
        }
    }
}
