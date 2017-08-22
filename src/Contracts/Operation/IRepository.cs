namespace Contracts.Operation
{
    /// <summary>
    /// Represents repository interface for working with database.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface IRepository<T, in TKey> : IGet<T, TKey>, IAdd<T>, IUpdate<T>, IDelete<TKey>, IHide<TKey> where T : class
    {
    }
}
