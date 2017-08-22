using System;
using System.IO;
using mshtml;

namespace WpfEditor.Core
{
    /// <summary>
    /// Represents document formatter.
    /// </summary>
    public class DocumentFormatter
    {
        /// <summary>
        /// Html document.
        /// </summary>
        private HTMLDocument document;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFormatter"/> class.
        /// </summary>
        public DocumentFormatter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFormatter"/> class.
        /// </summary>
        /// <param name="document">Html document.</param>
        public DocumentFormatter(HTMLDocument document)
        {
            this.document = document;
        }

        /// <summary>
        /// Set document.
        /// </summary>
        /// <param name="document">Html document.</param>
        public void SetDocument(HTMLDocument document)
        {
            this.document = document;
        }

        /// <summary>
        /// Get inner html.
        /// </summary>
        /// <returns>Returns inner html.</returns>
        public string GetInnerHtml()
        {
            var html = string.Empty;

            var element = this.document.documentElement;

            if (element != null)
            {
                html = element.innerHTML;
            }

            return html;
        }

        /// <summary>
        /// Get outer html.
        /// </summary>
        /// <returns>Returns outer html.</returns>
        public string GetOuterHtml()
        {
            var html = string.Empty;

            try
            {
                var element = this.document.documentElement;

                if (element != null)
                {
                    html = element.outerHTML;
                }
            }
            catch (Exception)
            {
            }

            return html;
        }

        /// <summary>
        /// Format to bold.
        /// </summary>
        public void Bold()
        {
            if (this.document != null)
            {
                this.document.execCommand("Bold");
            }
        }

        /// <summary>
        /// Format to italic.
        /// </summary>
        public void Italic()
        {
            if (this.document != null)
            {
                this.document.execCommand("Italic");
            }
        }

        /// <summary>
        /// Format to underline.
        /// </summary>
        public void Underline()
        {
            if (this.document != null)
            {
                this.document.execCommand("Underline");
            }
        }

        /// <summary>
        /// Justify to left.
        /// </summary>
        public void JustifyLeft()
        {
            if (this.document != null)
            {
                this.document.execCommand("JustifyLeft");
            }
        }

        /// <summary>
        /// Justify to center.
        /// </summary>
        public void JustifyCenter()
        {
            if (this.document != null)
            {
                this.document.execCommand("JustifyCenter");
            }
        }

        /// <summary>
        /// Justify to right.
        /// </summary>
        public void JustifyRight()
        {
            if (this.document != null)
            {
                this.document.execCommand("JustifyRight");
            }
        }

        /// <summary>
        /// Fully justify.
        /// </summary>
        public void JustifyFull()
        {
            if (this.document != null)
            {
                this.document.execCommand("JustifyFull");
            }
        }

        /// <summary>
        /// Insert ordered list.
        /// </summary>
        public void InsertOrderedList()
        {
            if (this.document != null)
            {
                this.document.execCommand("InsertOrderedList");
            }
        }

        /// <summary>
        /// Insert unordered list.
        /// </summary>
        public void InsertUnorderedList()
        {
            if (this.document != null)
            {
                this.document.execCommand("InsertUnorderedList");
            }
        }

        /// <summary>
        /// Negative indent paragraph.
        /// </summary>
        public void NegativeIndent()
        {
            if (this.document != null)
            {
                this.document.execCommand("Outdent");
            }
        }

        /// <summary>
        /// Indent paragraph.
        /// </summary>
        public void Indent()
        {
            if (this.document != null)
            {
                this.document.execCommand("Indent");
            }
        }

        /// <summary>
        /// Set font color.
        /// </summary>
        /// <param name="color">The color.</param>
        public void SetFontColor(string color)
        {
            if (this.document != null && !color.IsNullOrEmpty())
            {
                this.document.execCommand("ForeColor", false, color);
            }
        }

        /// <summary>
        /// Set back color.
        /// </summary>
        /// <param name="color">The color.</param>
        public void SetBackgroundColor(string color)
        {
            if (this.document != null && !color.IsNullOrEmpty())
            {
                this.document.body.style.background = color;
            }
        }

        /// <summary>
        /// Change font.
        /// </summary>
        /// <param name="fontName">The font name.</param>
        public void SetFontName(string fontName)
        {
            if (this.document != null && !fontName.IsNullOrEmpty())
            {
                this.document.execCommand("FontName", false, fontName);
            }
        }

        /// <summary>
        /// Change font size.
        /// </summary>
        /// <param name="fontSize">The font size.</param>
        public void SetFontSize(object fontSize)
        {
            var document2 = this.document as IHTMLDocument2;
            if (document2 != null && fontSize != null)
            {
                document2.execCommand("FontSize", false, fontSize);
            }
        }

        /// <summary>
        /// Change font style.
        /// </summary>
        /// <param name="style">The font style.</param>
        public void SetFontStyle(object style)
        {
            if (this.document != null && style != null)
            {
                this.document.execCommand("FormatBlock", false, style);
            }
        }

        /// <summary>
        /// Add image.
        /// </summary>
        /// <param name="location">Location of the image.</param>
        /// <param name="description">Description of the image.</param>
        public void AddImage(string location, string description)
        {
            if (this.document != null && !location.IsNullOrEmpty())
            {
                if (Utilities.ImageHelper.IsImage(location))
                {
                    var fileSizeInMb = new FileInfo(location).Length / 1024 / 1024;

                    if (fileSizeInMb < 2)
                    {
                        var bytes = File.ReadAllBytes(location);
                        var file = Convert.ToBase64String(bytes);

                        var imageHtml = string.Format("<img alt=\"{0}\" src=\"data:image/gif;base64,{1}\">", description, file);

                        var range = this.document.selection.createRange();
                        range.pasteHTML(imageHtml);
                    }
                }
            }
        }

        /// <summary>
        /// Add link.
        /// </summary>
        /// <param name="location">Location of the link.</param>
        /// <param name="description">Description of the link.</param>
        public void AddLink(string location, string description)
        {
            if (this.document != null && !location.IsNullOrEmpty())
            {
                var range = this.document.selection.createRange();
                range.pasteHTML(string.Format(@"<a href='{0}'target=""_blank"">{1}</a>", location, description));
            }
        }
    }
}
