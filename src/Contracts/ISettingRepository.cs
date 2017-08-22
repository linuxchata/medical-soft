using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents setting repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface ISettingRepository<TCommon, in TKey> : IGet<TCommon, TKey>, IUpdate<TCommon> where TCommon : class
    {
    }
}
