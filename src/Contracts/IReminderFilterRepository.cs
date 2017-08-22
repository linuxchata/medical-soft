using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents appointment repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    public interface IReminderFilterRepository<out TCommon> : IGetAll<TCommon>, IGetAllExceptDeleted<TCommon> where TCommon : class
    {
    }
}
