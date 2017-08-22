using System;
using System.Threading;

namespace Common.Services
{
    /// <summary>
    /// Service base class.
    /// </summary>
    public interface IServiceBase : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the cancellation requested.
        /// </summary>
        bool IsCancellationRequested { get; }

        /// <summary>
        /// Start service.
        /// </summary>
        void Start();

        /// <summary>
        /// Start service.
        /// </summary>
        /// <param name="callBack">The callback.</param>
        void Start(Action<ManualResetEvent> callBack);

        /// <summary>
        /// Cancel service.
        /// </summary>
        void Cancel();
    }
}
