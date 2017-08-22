using System.Windows.Input;
using Common.Commands;
using Common.ViewModel;
using Models;
using WpfEditor.Core;

namespace WpfEditor.ViewModel
{
    /// <summary>
    /// Represents add image view model.
    /// </summary>
    public sealed class AddImageViewModel : ViewModelDialogBase2<EditorImage>
    {
        private readonly DocumentFormatter documentFormatter;

        /// <summary>
        /// Select image command.
        /// </summary>
        private ICommand selectImageCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddImageViewModel"/> class.
        /// </summary>
        public AddImageViewModel()
        {
            this.Model = new EditorImage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddImageViewModel"/> class.
        /// </summary>
        /// <param name="documentFormatter">Document formatter.</param>
        public AddImageViewModel(DocumentFormatter documentFormatter)
            : this()
        {
            this.documentFormatter = documentFormatter;
        }

        /// <summary>
        /// Gets select image command.
        /// </summary>
        public ICommand SelectImageCommand
        {
            get
            {
                return this.selectImageCommand ?? (this.selectImageCommand = new CommonCommand(
                    param => this.SelectImage(),
                    param => true));
            }
        }

        /// <summary>
        /// Handle model.
        /// </summary>
        protected override void Handle()
        {
            this.documentFormatter.AddImage(this.Model.Location, this.Model.Description);

            this.CloseDialog();
        }

        /// <summary>
        /// Select image.
        /// </summary>
        private void SelectImage()
        {
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Filter = "JPEG Files (*.jpg)|*.jpg;*.jpeg";
                openFileDialog.RestoreDirectory = true;

                var result = openFileDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.Model.Location = openFileDialog.FileName;
                }
            }
        }
    }
}
