using System.Windows.Input;
using Client.ViewModel;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for EducationView
    /// </summary>
    public partial class EducationView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EducationView"/> class.
        /// </summary>
        public EducationView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// On double click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnEducationDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as EducationViewModel;

            if (vm != null)
            {
                vm.EditDialogCommand.Execute(null);
            }
        }
    }
}
