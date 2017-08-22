using System.Threading;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Represents TaskFactory extension.
    /// </summary>
    public static class TaskFactoryExtension
    {
        /// <summary>
        /// Start a new task with default culture.
        /// </summary>
        /// <param name="taskFactory">Task factory.</param>
        /// <param name="action">The action.</param>
        /// <returns>Returns a new task.</returns>
        public static Task StartNewWithDefaultCulture(this TaskFactory taskFactory, Action action)
        {
            if (taskFactory == null)
            {
                throw new ArgumentNullException("taskFactory");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var currentCulture = Thread.CurrentThread.CurrentCulture;
            var currentUiCulture = Thread.CurrentThread.CurrentUICulture;

            return taskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
                Thread.CurrentThread.CurrentUICulture = currentUiCulture;

                action();
            });
        }

        /// <summary>
        /// Start a new task with default culture.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="taskFactory">Task factory.</param>
        /// <param name="function">The function.</param>
        /// <returns>Returns a new task.</returns>
        public static Task<TResult> StartNewWithDefaultCulture<TResult>(this TaskFactory taskFactory, Func<TResult> function)
        {
            if (taskFactory == null)
            {
                throw new ArgumentNullException("taskFactory");
            }

            if (function == null)
            {
                throw new ArgumentNullException("action");
            }

            var currentCulture = Thread.CurrentThread.CurrentCulture;
            var currentUiCulture = Thread.CurrentThread.CurrentUICulture;

            return taskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
                Thread.CurrentThread.CurrentUICulture = currentUiCulture;

                return function();
            });
        }
    }
}