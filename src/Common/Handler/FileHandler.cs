using System;
using System.IO;
using Logger;

namespace Common.Handler
{
    /// <summary>
    /// Represent helper to handle files.
    /// </summary>
    public static class FileHandler
    {
        /// <summary>
        /// Try load file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="sizeLimit">Limit in size of the file (in KB).</param>
        /// <returns>Returns sequence of the bytes of the file.</returns>
        public static byte[] TryLoadFile(string fileName, int sizeLimit)
        {
            if (fileName.IsNullOrEmpty())
            {
                throw new ArgumentNullException("fileName", "File name cannot be null or empty.");
            }

            if (sizeLimit <= 0)
            {
                throw new ArgumentNullException("sizeLimit", "Size limit must have positive value.");
            }

            try
            {
                var fileSize = new System.IO.FileInfo(fileName).Length;

                if (fileSize <= sizeLimit)
                {
                    return File.ReadAllBytes(fileName);
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }

            return null;
        }
    }
}
