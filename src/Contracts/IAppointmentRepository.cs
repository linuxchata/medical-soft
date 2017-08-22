using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents appointment repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface IAppointmentRepository<TCommon, in TKey> : IGet<TCommon, TKey>, IAdd<TCommon>, IUpdate<TCommon>, IDelete<TKey> where TCommon : class
    {
    }
}
