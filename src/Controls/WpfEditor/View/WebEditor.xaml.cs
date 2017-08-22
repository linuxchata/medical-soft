using System.Windows;
using System.Windows.Controls;
using Logger;
using WpfEditor.Core;

namespace WpfEditor
{
    /// <summary>
    /// Represents web editor.
    /// </summary>
    public partial class WebEditor
    {
        /// <summary>
        /// Html content.
        /// </summary>
        public static readonly DependencyProperty HtmlProperty =
            DependencyProperty.Register(
            "Html",
            typeof(string),
            typeof(WebEditor),
            new FrameworkPropertyMetadata(OnHtmlPropertyChanged));

        /// <summary>
        /// Indicates whether edit bar enabled.
        /// </summary>
        public static readonly DependencyProperty IsEditBarEnabledProperty =
            DependencyProperty.Register(
            "IsEditBarEnabled",
            typeof(bool),
            typeof(WebEditor));

        /// <summary>
        /// Html handler.
        /// </summary>
        private HtmlHandler htmlHandler;
        
        /// <summary>
        /// Html property initialized.
        /// </summary>
        private bool initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEditor"/> class.
        /// </summary>
        public WebEditor()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets html content.
        /// </summary>
        public string Html
        {
            get
            {
                return (string)this.GetValue(HtmlProperty);
            }

            set
            {
                this.SetValue(HtmlProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit bar enabled.
        /// </summary>
        public bool IsEditBarEnabled
        {
            get
            {
                return (bool)this.GetValue(IsEditBarEnabledProperty);
            }

            set
            {
                this.SetValue(IsEditBarEnabledProperty, value);
            }
        }

        /// <summary>
        /// Get html.
        /// </summary>
        /// <returns>Returns html.</returns>
        public string GetHtml()
        {
            var html = string.Empty;

            if (this.htmlHandler != null)
            {
                html = this.htmlHandler.GetOuterHtml();
            }

            return html;
        }

        /// <summary>
        /// On Html content property changed.
        /// </summary>
        /// <param name="d">Dependency object.</param>
        /// <param name="e">The event argument.</param>
        private static void OnHtmlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebEditor;

            if (control != null)
            {
                control.OnHtmlPropertyChanged(e);
            }
        }

        /// <summary>
        /// CultureChanged event handler.
        /// </summary>
        /// <param name="e">Event argument.</param>
        private void OnHtmlPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!this.initialized)
            {
                if (this.htmlHandler != null)
                {
                    this.htmlHandler.CreateDocument(this.WebBrowserContainer, this.UpdateHtml, e.NewValue.ToString());
                    this.initialized = true;
                }
            }
        }

        /// <summary>
        /// Format to bold.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void Bold(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.Bold();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Format to italic.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void Italic(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.Italic();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Format to underline.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void Underline(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.Underline();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Justify to left.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void AlignLeft(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.JustifyLeft();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Justify to center.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void AlignCenter(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.JustifyCenter();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Justify to right.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void AlignRight(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.JustifyRight();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Fully justify.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void AlignJustify(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.JustifyFull();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Insert ordered list.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void InsertOrderedList(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.InsertOrderedList();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Insert unordered list.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void InsertUnorderedList(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.InsertUnorderedList();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Negative indent paragraph.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void NegativeIndent(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.NegativeIndent();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Indent paragraph.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void Indent(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.Indent();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Set font color.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void FontColor(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.SettingsFontColor();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Set back color.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void BackColor(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.SettingsBackColor();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Change font.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void FontChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.ChangeFont(RibbonComboboxFonts);
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Change font size.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void FontSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.ChangeFontSize(RibbonComboboxFontHeight);
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Change font style.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void FontStyleChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.ChangeFontStyle(RibbonComboboxFormat);
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Add link.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void AddLink(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.AddLink();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Add image.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void AddImage(object sender, RoutedEventArgs e)
        {
            if (this.htmlHandler != null)
            {
                this.htmlHandler.AddImage();
                this.UpdateHtml();
            }
        }

        /// <summary>
        /// Create document.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void CreateDocument(object sender, RoutedEventArgs e)
        {
            this.htmlHandler.CreateDocument(this.WebBrowserContainer, this.UpdateHtml);
            this.UpdateHtml();
        }

        /// <summary>
        /// Edit html.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void EditHtml(object sender, RoutedEventArgs e)
        {
            if (this.WebBrowserContainer.Visibility == Visibility.Visible)
            {
                return;
            }

            this.TextEditorContainer.Visibility = Visibility.Hidden;
            this.WebBrowserContainer.Visibility = Visibility.Visible;
            this.TextEditorContainer.Editor.SelectAll();

            this.WebBrowserContainer.Document.body.innerHTML = this.TextEditorContainer.Editor.Selection.Text;

            this.IsEditBarEnabled = true;
        }

        /// <summary>
        /// Edit document.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void ViewHtml(object sender, RoutedEventArgs e)
        {
            if (this.TextEditorContainer.Visibility == Visibility.Visible)
            {
                return;
            }

            this.TextEditorContainer.Visibility = Visibility.Visible;
            this.WebBrowserContainer.Visibility = Visibility.Hidden;

            this.TextEditorContainer.Editor.Selection.Text = this.htmlHandler.GetOuterHtml();

            this.IsEditBarEnabled = false;
        }

        /// <summary>
        /// Windows loaded event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void WindowsLoaded(object sender, RoutedEventArgs e)
        {
            this.IsEditBarEnabled = true;

            var initialisation = new Initialization(this);
            initialisation.Initialize();

            this.htmlHandler = new HtmlHandler();
            this.htmlHandler.CreateDocument(this.WebBrowserContainer, this.UpdateHtml);
        }

        /// <summary>
        /// Update html.
        /// </summary>
        private void UpdateHtml()
        {
            if (this.htmlHandler != null)
            {
                this.Html = this.htmlHandler.GetOuterHtml();
                Log.Info(this.Html.Replace("{", "||").Replace("}", "||"));
            }
        }
    }
}
