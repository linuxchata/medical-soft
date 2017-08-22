using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Client.Cache.Interface;
using Client.Contracts.Dialogs;
using Client.Providers;
using Client.Views.Dialogs;
using Common;
using Common.Builder;
using Common.Commands;
using Common.Communication;
using Common.Enumeration;
using Common.Handler;
using Common.ViewModel;
using DataAccess;
using Models;
using Models.Enumeration;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model for patient view.
    /// </summary>
    public sealed class PatientDialogViewModel : ViewModelDialogBase2<PatientModel>, IPatientDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IViewModelBuilder viewModelBuilder;

        private readonly IViewBuilder viewBuilder;

        private readonly IMessageBoxProvider messageBoxProvider;

        private readonly IApplicationSettings applicationSettings;

        private readonly IGenderCache genderCache;

        private ImageSource imageSource;

        private bool uploadImageEnabled;

        private List<GenderModel> genders;

        private ICommand selectImageCommand;

        private ICommand takePictureCommand;

        private ICommand deleteImageCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="viewModelBuilder">View model builder.</param>
        /// <param name="viewBuilder">View builder.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="applicationSettings">Application settings.</param>
        /// <param name="genderCache">Gender cache.</param>
        /// <param name="mode">Mode (Add/Edit).</param>
        /// <param name="model">Patient model.</param>
        public PatientDialogViewModel(
            IUnitOfWork unitOfWork,
            IViewModelBuilder viewModelBuilder,
            IViewBuilder viewBuilder,
            IMessageBoxProvider messageBoxProvider,
            IApplicationSettings applicationSettings,
            IGenderCache genderCache,
            WorkModeType mode,
            PatientModel model = null)
            : base(mode)
        {
            this.unitOfWork = unitOfWork;
            this.viewModelBuilder = viewModelBuilder;
            this.viewBuilder = viewBuilder;
            this.messageBoxProvider = messageBoxProvider;
            this.applicationSettings = applicationSettings;
            this.genderCache = genderCache;

            Task.Factory.StartNewWithDefaultCulture(() => this.Load(model));
        }

        /// <summary>
        /// Gets or sets image source.
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return this.imageSource;
            }

            set
            {
                this.imageSource = value;
                this.OnPropertyChanged(() => this.ImageSource);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether upload of the image enabled
        /// </summary>
        public bool UploadImageEnabled
        {
            get
            {
                return this.uploadImageEnabled;
            }

            set
            {
                this.uploadImageEnabled = value;
                this.OnPropertyChanged(() => this.UploadImageEnabled);
            }
        }

        /// <summary>
        /// Gets or sets all genders.
        /// </summary>
        public List<GenderModel> Genders
        {
            get
            {
                return this.genders;
            }

            set
            {
                this.genders = value;
                this.OnPropertyChanged(() => this.Genders);
            }
        }

        /// <summary>
        /// Gets delete image command.
        /// </summary>
        public ICommand DeleteImageCommand
        {
            get
            {
                return this.deleteImageCommand ?? (this.deleteImageCommand = new CommonCommand(
                    param => Task.Factory.StartNewWithDefaultCulture(this.DeleteImage),
                    param => this.CanHandle));
            }
        }

        /// <summary>
        /// Gets select image command.
        /// </summary>
        public ICommand SelectImageCommand
        {
            get
            {
                return this.selectImageCommand ?? (this.selectImageCommand = new CommonCommand(
                    param => Task.Factory.StartNewWithDefaultCulture(this.SelectImage),
                    param => this.CanHandle));
            }
        }

        /// <summary>
        /// Gets take picture command.
        /// </summary>
        public ICommand TakePictureCommand
        {
            get
            {
                return this.takePictureCommand ?? (this.takePictureCommand = new CommonCommand(
                    param => this.OpenCameraDialog(),
                    param => this.CanHandle));
            }
        }

        /// <summary>
        /// Load information about patient.
        /// </summary>
        /// <param name="patientModel">Patient model.</param>
        protected override void Load(PatientModel patientModel)
        {
            this.Status = LoadingStatus.Loading;

            this.Genders = this.genderCache.Get();

            this.LoadModel(patientModel);

            this.UploadImageEnabled = true;

            this.Model.MaximumSizeOfPhotoInBytes = this.applicationSettings.PhotoSizeLimitInBytes;
            this.Model.MaximumSizeOfPhotoInPixels = this.applicationSettings.PhotoSizeInPixels;

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Save patient.
        /// </summary>
        protected override void Handle()
        {
            this.Status = LoadingStatus.Loading;

            var response = this.SaveChanges();
            if (response.IsSuccessful)
            {
                this.UpdateIdOfAddedItem(response);
            }
            else
            {
                this.HandleFailure();
            }

            this.CloseDialog();
        }

        private void SelectImage()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".jpg",
                Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png"
            };

            this.UploadImageEnabled = false;

            var result = dialog.ShowDialog();
            if (result == true)
            {
                var fileName = dialog.FileName;
                this.LoadImageAndShowPreview(fileName);
            }

            this.UploadImageEnabled = true;
        }

        private void DeleteImage()
        {
            this.Model.Photo = null;
            this.ImageSource = null;
        }

        private void LoadImageAndShowPreview(string fileName)
        {
            this.Status = LoadingStatus.Loading;

            var previousImage = this.Model.Photo;

            var loadImageTask = Task.Factory.StartNewWithDefaultCulture(() => this.LoadImageForPreview(fileName));
            var previewImageTask = Task.Factory.StartNewWithDefaultCulture(() => this.LoadImage(fileName));

            Task.WaitAll(loadImageTask, previewImageTask);

            this.RollbackPreviousImageInCaseOfError(previousImage);

            this.Status = LoadingStatus.Loaded;
        }

        private void LoadImageForPreview(string fileName)
        {
            this.ImageSource = ImageHandler.CreateBitmapImage(fileName);
        }

        private void LoadImage(string fileName)
        {
            var photo = FileHandler.TryLoadFile(fileName, this.Model.MaximumSizeOfPhotoInBytes);
            if (photo == null)
            {
                var image = Image.FromFile(fileName, true);
                this.Model.Photo = ImageHandler.ResizeImage(image, this.Model.MaximumSizeOfPhotoInPixels);
            }
            else
            {
                this.Model.Photo = photo;
            }
        }

        private void RollbackPreviousImageInCaseOfError(byte[] previousImage)
        {
            if (this.Model.Photo == null)
            {
                this.Model.Photo = previousImage;
                this.ImageSource = null;

                this.messageBoxProvider.CannotBeUploadedDueToError();
            }
        }

        private void OpenCameraDialog()
        {
            var viewModel = this.viewModelBuilder.Build<IPreviewDialogViewModel>(new ResolverParameter(ParameterName.Model, this.Model));

            this.viewBuilder.Build<PreviewDialogView, IPreviewDialogViewModel>(viewModel).ShowDialog();

            if (this.Model.Photo != null)
            {
                this.ImageSource = ImageHandler.CreateBitmapImage(this.Model.Photo);
            }
        }

        private void LoadModel(PatientModel patientModel)
        {
            if (this.Mode == WorkModeType.Add)
            {
                this.Model = new PatientModel
                {
                    IsEmailNotificationAllowed = true,
                    IsEmailChecked = false,
                    Gender = (int)GenderType.Male
                };
            }
            else if (this.Mode == WorkModeType.Edit)
            {
                this.Model = patientModel.Map();

                if (this.Model.Photo != null)
                {
                    this.ImageSource = ImageHandler.CreateBitmapImage(this.Model.Photo);
                }
            }
        }

        private SaveChangesResponse SaveChanges()
        {
            if (this.Mode == WorkModeType.Add)
            {
                this.AddItem();
            }
            else if (this.Mode == WorkModeType.Edit)
            {
                this.EditItem();
            }

            return this.unitOfWork.Save();
        }

        private void EditItem()
        {
            this.unitOfWork.PatientRepository.Update(this.Model);
            this.Status = LoadingStatus.Updated;
        }

        private void AddItem()
        {
            this.unitOfWork.PatientRepository.Add(this.Model);
            this.Status = LoadingStatus.Added;
        }

        private void UpdateIdOfAddedItem(SaveChangesResponse response)
        {
            if (this.Status == LoadingStatus.Added)
            {
                this.Model.Id = response.TryGetValue(DatabaseEntity.Patients.ToString());
            }
        }

        private void HandleFailure()
        {
            this.Status = LoadingStatus.Failed;
            this.messageBoxProvider.CannotBeSavedDueToError();
        }
    }
}