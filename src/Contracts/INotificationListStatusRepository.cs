using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents notification list status repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    public interface INotificationListStatusRepository<out TCommon> : IGetAll<TCommon>, IGetAllExceptDeleted<TCommon> where TCommon : class
    {
    }
}
