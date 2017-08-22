using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents culture repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    public interface IUiCultureRepository<out TCommon> : IGetAll<TCommon>, IGetAllExceptDeleted<TCommon> where TCommon : class
    {
    }
}
