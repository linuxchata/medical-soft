using System.Diagnostics;
using System.Text;
using Logger;

namespace Client
{
    /// <summary>
    /// Represents tracer to catch binding errors.
    /// </summary>
    public class BindingErrorTraceListener : DefaultTraceListener
    {
        private static BindingErrorTraceListener listener;

        private readonly StringBuilder message = new StringBuilder();

        private BindingErrorTraceListener()
        {
        }

        /// <summary>
        /// Set trace.
        /// </summary>
        public static void SetTrace()
        {
            SetTrace(SourceLevels.Error, TraceOptions.None);
        }

        /// <summary>
        /// Set trace.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="options">The options.</param>
        public static void SetTrace(SourceLevels level, TraceOptions options)
        {
            if (listener == null)
            {
                listener = new BindingErrorTraceListener();
                PresentationTraceSources.DataBindingSource.Listeners.Add(listener);
            }

            listener.TraceOutputOptions = options;
            PresentationTraceSources.DataBindingSource.Switch.Level = level;
        }

        /// <summary>
        /// Close trace.
        /// </summary>
        public static void CloseTrace()
        {
            if (listener == null)
            {
                return;
            }

            listener.Flush();
            listener.Close();
            PresentationTraceSources.DataBindingSource.Listeners.Remove(listener);
            listener = null;
        }

        /// <summary>
        /// Write message.
        /// </summary>
        /// <param name="messageText">Message text.</param>
        public override void Write(string messageText)
        {
            this.message.Append(messageText);
        }

        /// <summary>
        /// Write message.
        /// </summary>
        /// <param name="messageText">Message text.</param>
        public override void WriteLine(string messageText)
        {
            this.message.Append(messageText);

            var final = this.message.ToString();
            this.message.Length = 0;

            Log.Error(final);
        }
    }
}
