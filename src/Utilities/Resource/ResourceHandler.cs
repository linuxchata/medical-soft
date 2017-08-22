using System.Resources;
using System.Threading;

namespace Utilities.Resource
{
    /// <summary>
    /// Represents resource handler.
    /// </summary>
    /// <typeparam name="T">The resource type.</typeparam>
    public class ResourceHandler<T> : IResourceHandler where T : class
    {
        private ResourceManager resourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceHandler{T}"/> class.
        /// </summary>
        public ResourceHandler()
        {
            this.Init();
        }

        /// <summary>
        /// Get value from resource file
        /// </summary>
        /// <param name="key">Name of the resource key</param>
        /// <returns>Value of the resource key</returns>
        public string GetValue(string key)
        {
            if (this.resourceManager == null)
            {
                this.Init();
            }

            return this.resourceManager.GetString(key, Thread.CurrentThread.CurrentCulture);
        }

        private void Init()
        {
            this.resourceManager = new ResourceManager(typeof(T));
        }
    }
}