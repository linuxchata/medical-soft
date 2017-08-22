using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace WpfEditor.Core
{
    /// <summary>
    /// Represents initialization logic for dropdown lists.
    /// </summary>
    public class Initialization
    {
        private readonly WebEditor webEditor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Initialization"/> class.
        /// </summary>
        /// <param name="webEditor">Web editor.</param>
        public Initialization(WebEditor webEditor)
        {
            if (webEditor == null)
            {
                throw new ArgumentNullException("webEditor");
            }

            this.webEditor = webEditor;
        }

        /// <summary>
        /// Initialize dropdown lists.
        /// </summary>
        public void Initialize()
        {
            this.FontsInitialisation();
            this.FontSizeInitialisation();
            this.FormatInitionalisation();
        }

        /// <summary>
        /// Initialize fonts.
        /// </summary>
        private void FontsInitialisation()
        {
            if (this.webEditor.RibbonComboboxFonts != null)
            {
                var fonts = Fonts.SystemFontFamilies.OrderBy(a => a.Source);
                this.webEditor.RibbonComboboxFonts.ItemsSource = fonts;
                this.webEditor.RibbonComboboxFonts.Text = "Times New Roman";
            }
        }

        /// <summary>
        /// Initialize formats.
        /// </summary>
        private void FormatInitionalisation()
        {
            var list = new List<Item>
            {
                new Item("<p>", "Paragraph"),
                new Item("<h1>", "Heading 1"),
                new Item("<h2>", "Heading 2"),
                new Item("<h3>", "Heading 3"),
                new Item("<h4>", "Heading 4"),
                new Item("<h5>", "Heading 5"),
                new Item("<h6>", "Heading 6"),
                new Item("<address>", "Address"),
                new Item("<pre>", "Preformat")
            };

            if (this.webEditor.RibbonComboboxFormat != null)
            {
                this.webEditor.RibbonComboboxFormat.ItemsSource = list;
                this.webEditor.RibbonComboboxFormat.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Initialize font sizes.
        /// </summary>
        private void FontSizeInitialisation()
        {
            var items = new List<string>();

            for (var x = 1; x <= 7; x++)
            {
                items.Add(x.ToString());
            }

            if (this.webEditor.RibbonComboboxFontHeight != null)
            {
                this.webEditor.RibbonComboboxFontHeight.ItemsSource = items;
                this.webEditor.RibbonComboboxFontHeight.Text = "3";
            }
        }
    }
}
