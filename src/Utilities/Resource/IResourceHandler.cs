namespace Utilities.Resource
{
    /// <summary>
    /// Represents resource handler interface.
    /// </summary>
    public interface IResourceHandler
    {
        /// <summary>
        /// Get value from resource file
        /// </summary>
        /// <param name="key">Name of the resource key</param>
        /// <returns>Value of the resource key</returns>
        string GetValue(string key);
    }
}