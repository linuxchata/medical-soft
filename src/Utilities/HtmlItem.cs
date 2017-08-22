using System;
using System.Net.Mime;

namespace Utilities
{
    /// <summary>
    /// Represents html item.
    /// </summary>
    public class HtmlItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlItem"/> class.
        /// </summary>
        /// <param name="html">Html content.</param>
        /// <param name="type">The type.</param>
        /// <param name="content">The content.</param>
        /// <param name="contentId">The content id.</param>
        public HtmlItem(string html, string type, string content = null, string contentId = null)
        {
            if (html.IsNullOrEmpty())
            {
                throw new ArgumentNullException("html", "Html cannot be null.");
            }

            if (type.IsNullOrEmpty())
            {
                throw new ArgumentNullException("type", "Type cannot be null.");
            }

            if (!type.Equals(MediaTypeNames.Text.Html, StringComparison.InvariantCultureIgnoreCase) &&
                !type.Equals(MediaTypeNames.Image.Jpeg, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("Only image/jpeg and text/html types are supported.");
            }

            if (type.Equals(MediaTypeNames.Image.Jpeg, StringComparison.InvariantCultureIgnoreCase))
            {
                if (content.IsNullOrEmpty())
                {
                    throw new ArgumentNullException("content", "Content cannot be null.");
                }

                if (contentId.IsNullOrEmpty())
                {
                    throw new ArgumentNullException("contentId", "ContentId cannot be null.");
                }
            }

            this.Html = html;
            this.Type = type;
            this.Content = content;
            this.ContentId = contentId;
        }

        /// <summary>
        /// Gets html content of the item.
        /// </summary>
        public string Html { get; private set; }

        /// <summary>
        /// Gets type of the item.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Gets content of the item.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Gets content id of the item.
        /// </summary>
        public string ContentId { get; private set; }
    }
}