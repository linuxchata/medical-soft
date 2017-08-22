using System;

namespace Utilities
{
    /// <summary>
    /// Represent generic WeakReference.
    /// </summary>
    /// <typeparam name="T">Class that represents WeakReference.</typeparam>
    public sealed class WeakReference<T>
        where T : class
    {
        private readonly WeakReference weakReference;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReference{T}"/> class.
        /// </summary>
        /// <param name="target">An object to track.</param>
        public WeakReference(T target)
        {
            this.weakReference = new WeakReference(target);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReference{T}"/> class.
        /// </summary>
        /// <param name="target">An object to track.</param>
        /// <param name="trackResurrection">Indicates when to stop tracking the object. If true, the object
        /// is tracked after finalization; if false, the object is only tracked until finalization.</param>
        public WeakReference(T target, bool trackResurrection)
        {
            this.weakReference = new WeakReference(target, trackResurrection);
        }

        /// <summary>
        /// Gets the object (the target) referenced by the current
        /// WeakReference object.
        /// </summary>
        public T Target
        {
            get
            {
                return this.weakReference.Target as T;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object referenced by the current
        /// WeakReference object has been garbage collected.
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return this.weakReference.IsAlive;
            }
        }
    }
}