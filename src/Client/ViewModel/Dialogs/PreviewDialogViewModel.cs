using System.Drawing;
using Client.Contracts.Dialogs;
using Common.Enumeration;
using Common.Handler;
using Common.ViewModel;
using Models;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model for web camera dialog.
    /// </summary>
    public sealed class PreviewDialogViewModel : ViewModelDialogBase2<PatientModel>, IPreviewDialogViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewDialogViewModel"/> class.
        /// </summary>
        /// <param name="model">Patient model.</param>
        public PreviewDialogViewModel(PatientModel model)
        {
            this.Model = model;
        }

        /// <summary>
        /// Set loading status.
        /// </summary>
        /// <param name="status">The status to set.</param>
        public void SetStatus(LoadingStatus status)
        {
            this.Status = status;
        }

        /// <summary>
        /// Save photo of the patient.
        /// </summary>
        /// <param name="photo">The photo of the patient.</param>
        public void Handle(Bitmap photo)
        {
            if (photo != null)
            {
                this.Model.Photo = ImageHandler.ResizeImage(photo, this.Model.MaximumSizeOfPhotoInPixels);
            }

            this.CloseDialog();
        }
    }
}
