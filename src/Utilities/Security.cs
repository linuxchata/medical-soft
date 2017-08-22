using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Represents security logic.
    /// </summary>
    public class Security
    {
        /// <summary>
        /// Get md5 hash.
        /// </summary>
        /// <param name="md5Hash">The hash.</param>
        /// <param name="input">Input string.</param>
        /// <returns>Coded string.</returns>
        public string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string
            var sbuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string
            foreach (var t in data)
            {
                sbuilder.Append(t.ToString("x2"));
            }

            // Return the hexadecimal string
            return sbuilder.ToString();
        }

        /// <summary>
        /// Verify a hash against a string.
        /// </summary>
        /// <param name="md5Hash">The hash.</param>
        /// <param name="input">Input string.</param>
        /// <param name="hash">Hash string.</param>
        /// <returns>String equality.</returns>
        public bool VerifyMd5Hash(MD5 md5Hash, SecureString input, SecureString hash)
        {
            // Hash the input
            var hashOfInput = this.GetMd5Hash(md5Hash, EncodeString(input).ToString());

            // Create a StringComparer an compare the hashes
            var comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, EncodeString(hash).ToString()))
            {
                return true;
            }

            return false;
        }

        private static unsafe StringBuilder EncodeString(SecureString ss)
        {
            char* pc = null;

            var sb = new StringBuilder(30);

            try
            {
                pc = (char*)Marshal.SecureStringToCoTaskMemUnicode(ss);

                for (var i = 0; pc[i] != 0; i++)
                {
                    sb.Append(pc[i]);
                }
            }
            finally
            {
                if (pc != null)
                {
                    Marshal.ZeroFreeCoTaskMemUnicode((IntPtr)pc);
                }
            }

            return sb;
        }
    }
}