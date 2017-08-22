using System.Windows;

namespace Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AmountSummary
    /// </summary>
    public partial class AmountSummary
    {
        /// <summary>
        /// Amount content.
        /// </summary>
        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register(
            "Amount",
            typeof(string),
            typeof(AmountSummary));

        /// <summary>
        /// Initializes a new instance of the AmountSummary class.
        /// </summary>
        public AmountSummary()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets amount.
        /// </summary>
        public string Amount
        {
            get
            {
                return (string)this.GetValue(AmountProperty);
            }

            set
            {
                this.SetValue(AmountProperty, value);
            }
        }
    }
}
