using Common.ViewModel;
using Models;
using WpfEditor.Core;

namespace WpfEditor.ViewModel
{
    /// <summary>
    /// Represents add link view model.
    /// </summary>
    public sealed class AddLinkViewModel : ViewModelDialogBase2<EditorLink>
    {
        private readonly DocumentFormatter documentFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddLinkViewModel"/> class.
        /// </summary>
        public AddLinkViewModel()
        {
            this.Model = new EditorLink();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddLinkViewModel"/> class.
        /// </summary>
        /// <param name="documentFormatter">Document formatter.</param>
        public AddLinkViewModel(DocumentFormatter documentFormatter)
            : this()
        {
            this.documentFormatter = documentFormatter;
        }

        /// <summary>
        /// Handle model.
        /// </summary>
        protected override void Handle()
        {
            this.documentFormatter.AddLink(this.Model.Location, this.Model.Name);

            this.CloseDialog();
        }
    }
}
