using Contracts.Operation;

namespace Contracts
{
    /// <summary>
    /// Represents education repository.
    /// </summary>
    /// <typeparam name="TCommon">Common class that represents the model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface IEducationRepository<TCommon, in TKey> : IRepository<TCommon, TKey> where TCommon : class
    {
    }
}
