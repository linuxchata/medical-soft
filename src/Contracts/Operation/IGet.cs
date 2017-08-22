namespace Contracts.Operation
{
    /// <summary>
    /// Represents get interface for working with database.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface IGet<out T, in TKey> : IGetAll<T>, IGetAllExceptDeleted<T>, IGetById<T, TKey> where T : class
    {
    }
}
