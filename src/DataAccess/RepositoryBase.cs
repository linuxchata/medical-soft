using Common.Authentication;

namespace DataAccess
{
    /// <summary>
    /// Represents base repository.
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>
        /// Entity framework context.
        /// </summary>
        protected readonly dentistEntities Entities;

        /// <summary>
        /// Authentication session.
        /// </summary>
        protected readonly IAuthenticationSession AuthenticationSession;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected RepositoryBase(dentistEntities entity)
        {
            this.Entities = entity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="authenticationSession">Authentication session.</param>
        protected RepositoryBase(dentistEntities entity, IAuthenticationSession authenticationSession)
        {
            this.Entities = entity;
            this.AuthenticationSession = authenticationSession;
        }
    }
}
