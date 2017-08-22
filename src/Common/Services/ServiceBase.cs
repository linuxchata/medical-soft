using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Logger;

namespace Common.Services
{
    /// <summary>
    /// Service base class.
    /// </summary>
    public abstract class ServiceBase : IServiceBase
    {
        /// <summary>
        /// Main task.
        /// </summary>
        protected readonly Task Task;

        private readonly CultureInfo currentCulture;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBase"/> class.
        /// </summary>
        protected ServiceBase()
        {
            this.currentCulture = Thread.CurrentThread.CurrentCulture;

            this.Task = new Task(this.Handle);
            Log.Debug("{0} thread has been created.", Log.Args(this.GetType().Name));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cancellation requested.
        /// </summary>
        public bool IsCancellationRequested { get; protected set; }

        /// <summary>
        /// Gets the callback.
        /// </summary>
        protected Action<ManualResetEvent> CallBack { get; private set; }

        /// <summary>
        /// Start service.
        /// </summary>
        public virtual void Start()
        {
            try
            {
                this.Task.Start();

                Log.Debug("{0} task has been started.", Log.Args(this.GetType().Name));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Start service.
        /// </summary>
        /// <param name="callBack">The callback.</param>
        public virtual void Start(Action<ManualResetEvent> callBack)
        {
            this.CallBack = callBack;

            this.Start();
        }

        /// <summary>
        /// Cancel service.
        /// </summary>
        public virtual void Cancel()
        {
            this.IsCancellationRequested = true;

            Log.Debug("{0} thread has been canceled.", Log.Args(this.GetType().Name));
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Represents service logic.
        /// </summary>
        protected abstract void Do();

        /// <summary>
        /// Dispose the object.
        /// </summary>
        /// <param name="disposing">Define whether managed objects have to be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.Task.Dispose();
            }

            this.disposed = true;
        }

        private void Handle()
        {
            Thread.CurrentThread.CurrentCulture = this.currentCulture;

            this.Do();
        }
    }
}
