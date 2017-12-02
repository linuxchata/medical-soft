using System;
using System.Windows.Controls;
using Common.Builder;
using WpfEditor.View;

namespace WpfEditor.Core
{
    /// <summary>
    /// Represents HTML handler.
    /// </summary>
    public class HtmlHandler
    {
        private readonly DocumentFormatter documentFormatter;

        private readonly string colorFormat = "#{0:X2}{1:X2}{2:X2}";

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlHandler"/> class.
        /// </summary>
        public HtmlHandler()
        {
            this.documentFormatter = new DocumentFormatter();
        }

        /// <summary>
        /// Create document.
        /// </summary>
        /// <param name="webBrowserContainer">Web browser container.</param>
        /// <param name="callback">Update call back.</param>
        /// <param name="content">Document content.</param>
        public void CreateDocument(WebBrowserContainer webBrowserContainer, Action callback, string content = null)
        {
            var document = webBrowserContainer.CreateWebBrowser(callback, content);
            this.documentFormatter.SetDocument(document);
        }

        /// <summary>
        /// Get inner HTML.
        /// </summary>
        /// <returns>Returns inner HTML.</returns>
        public string GetInnerHtml()
        {
            return this.documentFormatter.GetInnerHtml();
        }

        /// <summary>
        /// Get outer HTML.
        /// </summary>
        /// <returns>Returns outer HTML.</returns>
        public string GetOuterHtml()
        {
            return this.documentFormatter.GetOuterHtml();
        }

        /// <summary>
        /// Format to bold.
        /// </summary>
        public void Bold()
        {
            this.documentFormatter.Bold();
        }

        /// <summary>
        /// Format to italic.
        /// </summary>
        public void Italic()
        {
            this.documentFormatter.Italic();
        }

        /// <summary>
        /// Format to underline.
        /// </summary>
        public void Underline()
        {
            this.documentFormatter.Underline();
        }

        /// <summary>
        /// Justify to left.
        /// </summary>
        public void JustifyLeft()
        {
            this.documentFormatter.JustifyLeft();
        }

        /// <summary>
        /// Justify to center.
        /// </summary>
        public void JustifyCenter()
        {
            this.documentFormatter.JustifyCenter();
        }

        /// <summary>
        /// Justify to right.
        /// </summary>
        public void JustifyRight()
        {
            this.documentFormatter.JustifyRight();
        }

        /// <summary>
        /// Fully justify.
        /// </summary>
        public void JustifyFull()
        {
            this.documentFormatter.JustifyFull();
        }

        /// <summary>
        /// Insert ordered list.
        /// </summary>
        public void InsertOrderedList()
        {
            this.documentFormatter.InsertOrderedList();
        }

        /// <summary>
        /// Insert unordered list.
        /// </summary>
        public void InsertUnorderedList()
        {
            this.documentFormatter.InsertUnorderedList();
        }

        /// <summary>
        /// Negative indent  paragraph.
        /// </summary>
        public void NegativeIndent()
        {
            this.documentFormatter.NegativeIndent();
        }

        /// <summary>
        /// Indent paragraph.
        /// </summary>
        public void Indent()
        {
            this.documentFormatter.Indent();
        }

        /// <summary>
        /// Set font color.
        /// </summary>
        public void SettingsFontColor()
        {
            var color = new ColorDialogBox().SelectColor();

            if (color.HasValue)
            {
                var colorString = string.Format(this.colorFormat, color.Value.R, color.Value.G, color.Value.B);
                this.documentFormatter.SetFontColor(colorString);
            }
        }

        /// <summary>
        /// Set back color.
        /// </summary>
        public void SettingsBackColor()
        {
            var color = new ColorDialogBox().SelectColor();

            if (color.HasValue)
            {
                var colorString = string.Format(this.colorFormat, color.Value.R, color.Value.G, color.Value.B);
                this.documentFormatter.SetBackgroundColor(colorString);
            }
        }

        /// <summary>
        /// Change font.
        /// </summary>
        /// <param name="font">The font name.</param>
        public void ChangeFont(ComboBox font)
        {
            if (font != null)
            {
                this.documentFormatter.SetFontName(font.SelectedItem.ToString());
            }
        }

        /// <summary>
        /// Change font size.
        /// </summary>
        /// <param name="fontSize">The font size.</param>
        public void ChangeFontSize(ComboBox fontSize)
        {
            if (fontSize != null)
            {
                this.documentFormatter.SetFontSize(fontSize.SelectedItem);
            }
        }

        /// <summary>
        /// Change font style.
        /// </summary>
        /// <param name="fontStyle">The font style.</param>
        public void ChangeFontStyle(ComboBox fontStyle)
        {
            if (fontStyle != null)
            {
                var id = fontStyle.SelectedItem as Item;

                if (id != null)
                {
                    this.documentFormatter.SetFontStyle(id.Value);
                }
            }
        }

        /// <summary>
        /// Add link.
        /// </summary>
        public void AddLink()
        {
            var builder = new ViewBuilder();
            var viewModel = new ViewModel.AddLinkViewModel(this.documentFormatter);
            builder.Build<AddLink, ViewModel.AddLinkViewModel>(viewModel).ShowDialog();
        }

        /// <summary>
        /// Add image.
        /// </summary>
        public void AddImage()
        {
            var builder = new ViewBuilder();
            var viewModel = new ViewModel.AddImageViewModel(this.documentFormatter);
            builder.Build<AddImage, ViewModel.AddImageViewModel>(viewModel).ShowDialog();
        }
    }
}
