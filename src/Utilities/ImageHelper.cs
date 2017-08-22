using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Utilities
{
    /// <summary>
    /// Represents image helper.
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Check whether provided file path corresponding to an image.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>Returns true if provided file path corresponding to an image.</returns>
        public static bool IsImage(string filePath)
        {
            var isImage = false;

            if (!filePath.IsNullOrEmpty() && File.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    stream.Seek(0, SeekOrigin.Begin);

                    var jpg = new List<string> { "FF", "D8" };
                    var bmp = new List<string> { "42", "4D" };
                    var gif = new List<string> { "47", "49", "46" };
                    var png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
                    var imageTypes = new List<List<string>> { jpg, bmp, gif, png };

                    var bytesIterated = new List<string>();

                    for (var i = 0; i < 8; i++)
                    {
                        var bit = stream.ReadByte().ToString("X2");
                        bytesIterated.Add(bit);

                        isImage = imageTypes.Any(m => !m.Except(bytesIterated).Any());

                        if (isImage)
                        {
                            break;
                        }
                    }
                }
            }

            return isImage;
        }
    }
}