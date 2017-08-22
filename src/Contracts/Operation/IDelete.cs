namespace Contracts.Operation
{
    /// <summary>
    /// Represents delete interface for working with database.
    /// </summary>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface IDelete<in TKey>
    {
        /// <summary>
        /// Delete record.
        /// </summary>
        /// <param name="id">The identification.</param>
        void Delete(TKey id);
    }
}
