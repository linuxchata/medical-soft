using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace Utilities
{
    /// <summary>
    /// Represents html helper.
    /// </summary>
    public class HtmlHelper
    {
        /// <summary>
        /// Parse html to prepare image tags for further sending html via e-mail.
        /// </summary>
        /// <param name="html">Html to be parsed.</param>
        /// <returns>Returns list of the image tags with content and modified html.</returns>
        public static Tuple<IList<HtmlItem>, HtmlItem> ParseHtml(string html)
        {
            if (html.IsNullOrEmpty())
            {
                throw new ArgumentNullException("html", "Html cannot be null.");
            }

            // Handle html to find and modify image tags.
            var splittedHtml = SplitHtml(html);

            // Create result html with modified image tags.
            var handledHtml = CreateHtmlItemForText(string.Join(string.Empty, splittedHtml.Select(a => a.Html)));

            // Filter image tags items that contain content of the images.
            var imageTags = splittedHtml
                .Where(a => a.Type.Equals(MediaTypeNames.Image.Jpeg, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            return new Tuple<IList<HtmlItem>, HtmlItem>(imageTags, handledHtml);
        }

        private static IList<HtmlItem> SplitHtml(string html)
        {
            var result = new List<HtmlItem>();

            var imageStartIndex = html.IndexOf("<img", StringComparison.InvariantCultureIgnoreCase);

            // <img may exist.
            if (imageStartIndex > -1)
            {
                var imageStartSubstring = html.Substring(imageStartIndex);
                var imageEndIndex = imageStartSubstring.IndexOf('>');

                if (imageEndIndex > -1)
                {
                    // Possible image tag.
                    var imageSubstring = imageStartSubstring.Substring(0, imageEndIndex + 1);

                    // Add image tag to the collection.
                    if (IsImageTag(imageSubstring))
                    {
                        // Add string before <img> tag to the collection.
                        // In case img tag at the beginning of the string, there is nothing to add.
                        if (imageStartIndex > 0)
                        {
                            var beforeImgString = html.Substring(0, imageStartIndex);
                            result.Add(CreateHtmlItemForText(beforeImgString));
                        }

                        result.Add(CreateHtmlItemForImage(imageSubstring));

                        // Handle string after img tag.
                        var afterImgString = imageStartSubstring.Substring(imageEndIndex + 1);
                        result.AddRange(SplitHtml(afterImgString));
                    }
                    else
                    {
                        // No image was found.
                        result.Add(CreateHtmlItemForText(html));
                    }
                }
            }
            else
            {
                // No image was found.
                result.Add(CreateHtmlItemForText(html));
            }

            return result;
        }

        private static HtmlItem CreateHtmlItemForImage(string imageTag)
        {
            var contentId = Guid.NewGuid().ToString();
            var modifiedContent = ModifyImageSourceContent(imageTag, contentId);
            var imageSourceContent = GetImageSourceContent(imageTag);
            return new HtmlItem(modifiedContent, MediaTypeNames.Image.Jpeg, imageSourceContent, contentId);
        }

        private static HtmlItem CreateHtmlItemForText(string text)
        {
            return new HtmlItem(text, MediaTypeNames.Text.Html);
        }

        private static bool IsImageTag(string inputString)
        {
            var regexString = "<img.+?src=[\"'](.+?)[\"'].*?>";
            var match = Regex.Match(inputString, regexString, RegexOptions.IgnoreCase | RegexOptions.Multiline)
                .Groups[1]
                .Value;

            return !match.IsNullOrEmpty();
        }

        private static string ModifyImageSourceContent(string imgTag, string imageGuid)
        {
            var searchString = "data:image/gif;base64,";
            var imgSrcStartIndex = imgTag.IndexOf(searchString, StringComparison.Ordinal);
            var imgSrcStartSubstring = imgTag.Substring(imgSrcStartIndex);
            var imageEndIndex = imgSrcStartSubstring.IndexOf('"');

            var imgSrcContent = imgTag.Substring(imgSrcStartIndex, imageEndIndex);

            var contentId = string.Format("cid:{0}", imageGuid);
            imgTag = imgTag.Replace(imgSrcContent, contentId);

            return imgTag;
        }

        private static string GetImageSourceContent(string imgTag)
        {
            var searchString = "src=\"data:image/gif;base64,";
            var imgSrcStartIndex = imgTag.IndexOf(searchString, StringComparison.Ordinal) + searchString.Length;
            var imgSrcStartSubstring = imgTag.Substring(imgSrcStartIndex);
            var imageEndIndex = imgSrcStartSubstring.IndexOf('"');

            var imgSrcContent = imgTag.Substring(imgSrcStartIndex, imageEndIndex);

            return imgSrcContent;
        }
    }
}