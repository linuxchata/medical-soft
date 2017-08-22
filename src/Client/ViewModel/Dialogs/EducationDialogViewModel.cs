using System;
using System.Threading.Tasks;
using Client.Contracts.Dialogs;
using Client.Providers;
using Common.Communication;
using Common.Enumeration;
using Common.ViewModel;
using DataAccess;
using Models;

namespace Client.ViewModel.Dialogs
{
    /// <summary>
    /// Represents view model for education dialog.
    /// </summary>
    public sealed class EducationDialogViewModel : ViewModelDialogBase2<EducationModel>, IEducationDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMessageBoxProvider messageBoxProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EducationDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        public EducationDialogViewModel(
            IUnitOfWork unitOfWork,
            IMessageBoxProvider messageBoxProvider)
        {
            this.unitOfWork = unitOfWork;
            this.messageBoxProvider = messageBoxProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EducationDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="model">Education model.</param>
        public EducationDialogViewModel(
            IUnitOfWork unitOfWork,
            IMessageBoxProvider messageBoxProvider,
            WorkModeType mode,
            EducationModel model = null)
            : base(mode)
        {
            this.unitOfWork = unitOfWork;
            this.messageBoxProvider = messageBoxProvider;

            Task.Factory.StartNewWithDefaultCulture(() => this.Load(model));
        }

        /// <summary>
        /// Load information about education.
        /// </summary>
        /// <param name="educationModel">Education model.</param>
        protected override void Load(EducationModel educationModel)
        {
            this.Status = LoadingStatus.Loading;

            this.LoadModel(educationModel);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Save education.
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

        private void LoadModel(EducationModel educationModel)
        {
            if (this.Mode == WorkModeType.Add)
            {
                this.Model = new EducationModel();
            }
            else if (this.Mode == WorkModeType.Edit)
            {
                this.Model = educationModel.Map();
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

        private void AddItem()
        {
            this.unitOfWork.EducationRepository.Add(this.Model);
            this.Status = LoadingStatus.Added;
        }

        private void EditItem()
        {
            this.unitOfWork.EducationRepository.Update(this.Model);
            this.Status = LoadingStatus.Updated;
        }

        private void UpdateIdOfAddedItem(SaveChangesResponse response)
        {
            if (this.Status == LoadingStatus.Added)
            {
                this.Model.Id = response.TryGetValue(DatabaseEntity.Educations.ToString());
            }
        }

        private void HandleFailure()
        {
            this.Status = LoadingStatus.Failed;
            this.messageBoxProvider.CannotBeSavedDueToError();
        }
    }
}
