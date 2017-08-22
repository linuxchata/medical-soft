namespace Contracts.Operation
{
    /// <summary>
    /// Represents update interface for working with database.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public interface IUpdate<in T> where T : class
    {
        /// <summary>
        /// Update record.
        /// </summary>
        /// <param name="tclass">Common class.</param>
        void Update(T tclass);
    }
}
