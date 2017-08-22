using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents position repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface IPositionRepository<TCommon, in TKey> : IRepository<TCommon, TKey> where TCommon : class
    {
    }
}
