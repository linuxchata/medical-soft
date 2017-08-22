namespace Contracts.Operation
{
    /// <summary>
    /// Represents get by id interface for working with database.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface IGetById<out T, in TKey> where T : class
    {
        /// <summary>
        /// Get record by id.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <returns>T class.</returns>
        T GetById(TKey id);
    }
}
