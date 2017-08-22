namespace Contracts.Operation
{
    /// <summary>
    /// Represents add interface for working with database.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public interface IAdd<in T> where T : class
    {
        /// <summary>
        /// Add a new record.
        /// </summary>
        /// <param name="tclass">Common class.</param>
        void Add(T tclass);
    }
}
