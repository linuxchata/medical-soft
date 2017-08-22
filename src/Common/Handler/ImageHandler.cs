using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using Logger;

namespace Common.Handler
{
    /// <summary>
    /// Represent helper to handle images.
    /// </summary>
    public static class ImageHandler
    {
        /// <summary>
        /// Create bitmap image.
        /// </summary>
        /// <param name="array">Byte array.</param>
        /// <returns>Returns bitmap image.</returns>
        public static BitmapImage CreateBitmapImage(byte[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array", "Array of bytes cannot be null.");
            }

            using (var stream = new System.IO.MemoryStream(array))
            {
                return CreateBitmapImage(stream);
            }
        }

        /// <summary>
        /// Create bitmap image.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>Returns bitmap image.</returns>
        public static BitmapImage CreateBitmapImage(string fileName)
        {
            if (fileName.IsNullOrEmpty())
            {
                throw new ArgumentNullException("fileName", "File name cannot be null.");
            }

            var image = CreateBitmapImage();
            image.UriSource = new Uri(fileName, UriKind.Absolute);
            CloseBitmapImage(image);

            return image;
        }

        /// <summary>
        /// Resize image to set size with keeping size ratio.
        /// </summary>
        /// <param name="image">An image to resize.</param>
        /// <param name="imageSize">An image size to resize in pixels.</param>
        /// <returns>Return array of the bites of the resized image.</returns>
        public static byte[] ResizeImage(Image image, int imageSize)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image", "Image cannot be null.");
            }

            if (imageSize <= 0)
            {
                throw new ArgumentNullException("imageSize", "Image size must have positive value.");
            }

            var size = image.PhysicalDimension;

            if (size.IsEmpty && size.Height > 0 && size.Width > 0)
            {
                throw new ArgumentException("size", "Size cannot be empty.");
            }

            var height = (float)imageSize;
            var width = (float)imageSize;

            if (size.Height < height || size.Width < width)
            {
                // Don't not change image size if the original size of the image
                // is smaller than provided height and width to resize.
                height = size.Height;
                width = size.Width;
            }
            else
            {
                // Define height and width of the image.
                if (size.Width > size.Height)
                {
                    var ratio = size.Width / width;
                    height = size.Height / ratio;
                }
                else
                {
                    var ratio = size.Height / height;
                    width = size.Width / ratio;
                }
            }

            try
            {
                var bitmapImage = DrawImage(image, (int)width, (int)height);
                return ToByteArray(bitmapImage, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Draw image.
        /// </summary>
        /// <param name="image">An image to redraw.</param>
        /// <param name="width">Width of an image.</param>
        /// <param name="height">Height of an image.</param>
        /// <returns>Returns bitmap of the redrawn image.</returns>
        private static Bitmap DrawImage(Image image, int width, int height)
        {
            var destRectangle = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary>
        /// Convert an image to array of the bytes.
        /// </summary>
        /// <param name="image">An image to convert.</param>
        /// <param name="format">An image format.</param>
        /// <returns>Returns byte array that represent specified image.</returns>
        private static byte[] ToByteArray(this Image image, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Create bitmap image.
        /// </summary>
        /// <param name="stream">Memory stream.</param>
        /// <returns>Returns bitmap image.</returns>
        private static BitmapImage CreateBitmapImage(System.IO.MemoryStream stream)
        {
            var image = CreateBitmapImage();
            image.StreamSource = stream;
            CloseBitmapImage(image);

            return image;
        }

        /// <summary>
        /// Create bitmap image.
        /// </summary>
        /// <returns>Returns bitmap image.</returns>
        private static BitmapImage CreateBitmapImage()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;

            return image;
        }

        /// <summary>
        /// Close bitmap image.
        /// </summary>
        /// <param name="bitmapImage">Bitmap image to close.</param>
        private static void CloseBitmapImage(BitmapImage bitmapImage)
        {
            bitmapImage.EndInit();
            bitmapImage.Freeze();
        }
    }
}
