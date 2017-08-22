using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// Represents string helper.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Returns a value indicating whether the specified string occurs within this string. Ignore case.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="toCheck">The string to seek.</param>
        /// <returns>True if the value parameter occurs within this string; otherwise, false.</returns>
        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            if (!source.IsNullOrEmpty())
            {
                return source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether the specified string starts within this string. Ignore case.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="toCheck">The string to seek.</param>
        /// <returns>True if the value parameter starts within this string; otherwise, false.</returns>
        public static bool StartsWithIgnoreCase(this string source, string toCheck)
        {
            if (!source.IsNullOrEmpty())
            {
                return source.StartsWith(toCheck, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the specified string is null or an System.String.Empty string.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <returns>Returns true if the value parameter is null or an empty string (""); otherwise, false.</returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// Check whether string is phone number.
        /// </summary>
        /// <param name="source">Possible phone number.</param>
        /// <returns>Returns true whether string is phone number; otherwise, false.</returns>
        public static bool IsPhoneNumber(this string source)
        {
            return Regex.Match(source, @"^[\d|\+|\(]+[\)|\d|\s|-]*[\d]$").Success;
        }
    }
}