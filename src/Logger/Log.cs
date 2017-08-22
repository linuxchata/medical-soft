using System;
using System.IO;
using System.Runtime.CompilerServices;
using log4net;
using log4net.Config;

namespace Logger
{
    /// <summary>
    /// Represents wrapper for the log4net. Use this class to write log information in text file.
    /// </summary>
    public static class Log
    {
        private static readonly ILog Loging = LogManager.GetLogger(typeof(Log));

        private static readonly string Separator = " | ";

        /// <summary>
        /// Initializes static members of the Log class.
        /// </summary>
        static Log()
        {
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// Logs a formatted debug message.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>s
        /// <param name="caller">Caller object.</param>
        public static void Debug(string format, object[] args, [CallerFilePath] string caller = "")
        {
            Loging.DebugFormat(GetFileNameWithoutExtension(caller) + Separator + string.Format(format, args));
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">A string message.</param>
        /// <param name="caller">The caller object.</param>
        public static void Debug(string message, [CallerFilePath] string caller = "")
        {
            Loging.DebugFormat(GetFileNameWithoutExtension(caller) + Separator + message);
        }

        /// <summary>
        /// Logs a information message.
        /// </summary>
        /// <param name="message">A string message.</param>
        /// <param name="caller">The caller object.</param>
        public static void Info(string message, [CallerMemberName] string caller = "")
        {
            Loging.InfoFormat(GetFileNameWithoutExtension(caller) + Separator + message);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">A string message.</param>
        /// <param name="caller">The caller object.</param>
        public static void Warn(string message, [CallerMemberName] string caller = "")
        {
            Loging.WarnFormat(GetFileNameWithoutExtension(caller) + Separator + message);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">A string message.</param>
        /// <param name="caller">The caller object.</param>
        public static void Error(string message, [CallerMemberName] string caller = "")
        {
            Loging.ErrorFormat(GetFileNameWithoutExtension(caller) + Separator + message);
        }

        /// <summary>
        /// Logs an exception message.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void Exception(Exception exception)
        {
            var innerException = exception.InnerException != null ? exception.InnerException.ToString() : "-";
            Loging.ErrorFormat("Message: {0} Inner exception: {1}.{2}{3}", exception.Message, innerException, Environment.NewLine, exception.StackTrace);
        }

        /// <summary>
        /// Get object array that contains zero or more objects to format.
        /// </summary>
        /// <param name="args">Method parameter that takes a variable number of arguments.</param>
        /// <returns>Returns object array that contains zero or more objects to format.</returns>
        public static object[] Args(params object[] args)
        {
            return args;
        }

        private static string GetFileNameWithoutExtension(string caller)
        {
            return Path.GetFileNameWithoutExtension(caller);
        }
    }
}