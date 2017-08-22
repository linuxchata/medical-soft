using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents gender repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    public interface IGenderRepository<out TCommon> : IGetAll<TCommon>, IGetAllExceptDeleted<TCommon>
        where TCommon : class
    {
    }
}
