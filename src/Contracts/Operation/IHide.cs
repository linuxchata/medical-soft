namespace Contracts.Operation
{
    /// <summary>
    /// Represents hide interface for working with database.
    /// </summary>
    /// <typeparam name="TKey">Key that identified entry in the database (primary key of the table).</typeparam>
    public interface IHide<in TKey>
    {
        /// <summary>
        /// Try hide record.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <returns>Returns true if record was marked as deleted; otherwise, false.</returns>
        bool TryHide(TKey id);
    }
}
