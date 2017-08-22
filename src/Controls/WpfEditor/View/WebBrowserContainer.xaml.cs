using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using mshtml;
using WpfEditor.Core;

namespace WpfEditor.View
{
    /// <summary>
    /// Interaction logic for WebBrowserContainer
    /// </summary>
    public partial class WebBrowserContainer
    {
        /// <summary>
        /// Web browser.
        /// </summary>
        private WebBrowser webBrowser;

        /// <summary>
        /// Update call back.
        /// </summary>
        private Action callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowserContainer"/> class.
        /// </summary>
        public WebBrowserContainer()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets html document.
        /// </summary>
        public HTMLDocument Document { get; set; }

        /// <summary>
        /// Create web browser.
        /// </summary>
        /// <param name="callback">Update call back.</param>
        /// <param name="content">The document content.</param>
        /// <returns>Returns html document.</returns>
        public HTMLDocument CreateWebBrowser(Action callback, string content = null)
        {
            this.callback = callback;
            
            if (this.Document != null)
            {
                this.Document.clear();
            }

            this.webBrowser = new WebBrowser();
            this.webBrowser.LoadCompleted += this.Completed;
            this.GridWebBrowser.Children.Add(this.webBrowser);

            Script.HideScriptErrors(this.webBrowser);

            this.webBrowser.NavigateToString(!content.IsNullOrEmpty() ? content : Properties.Resources.EmptyDocument);
            
            dynamic domDocument = this.webBrowser.Document;
            domDocument.charset = "utf-8";
            domDocument.designMode = "On";
            
            this.Document = domDocument;

            Logger.Log.Info(this.Document.charset);

            return this.Document;
        }

        /// <summary>
        /// Completed event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void Completed(object sender, NavigationEventArgs e)
        {
            this.Document = this.webBrowser.Document as HTMLDocument;

            if (this.Document != null)
            {
                this.Document.designMode = "On";

                var document = (HTMLDocumentEvents2_Event)this.Document;

                // Disable context menu of the browser.
                document.oncontextmenu += obj => false;

                // Attach call back on key up event of the DOM.
                document.onkeyup += obj =>
                {
                    this.callback();
                };
            }
        }
    }
}
