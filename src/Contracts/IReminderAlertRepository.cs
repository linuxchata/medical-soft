using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents reminder alert repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    public interface IReminderAlertRepository<out TCommon> : IGetAll<TCommon>, IGetAllExceptDeleted<TCommon> where TCommon : class
    {
    }
}
