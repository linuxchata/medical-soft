using System.Windows.Input;
using Client.ViewModel;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for PatientView
    /// </summary>
    public partial class PatientView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientView"/> class.
        /// </summary>
        public PatientView()
        {
             this.InitializeComponent();
        }

        /// <summary>
        /// On Patient double click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnPatientDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as PatientViewModel;

            if (vm != null)
            {
                vm.EditDialogCommand.Execute(null);
            }
        }
    }
}
