namespace WpfEditor.Core
{
    /// <summary>
    /// Represents dropdown list item.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <param name="value">The value.</param>
        public Item(string id, string value)
        {
            this.Value = value;
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets id of the item.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets value of the item.
        /// </summary>
        public string Value { get; set; }
    }
}
