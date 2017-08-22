using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Cache.Interface;
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
    /// Represents view model for staff view.
    /// </summary>
    public sealed class StaffDialogViewModel : ViewModelDialogBase2<StaffModel>, IStaffDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMessageBoxProvider messageBoxProvider;

        private readonly IGenderCache genderCache;

        private readonly IPositionCache positionCache;

        private readonly IEducationCache educationCache;

        private List<GenderModel> genders;

        private List<PositionModel> positions;

        private List<EducationModel> educations;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="genderCache">Gender cache.</param>
        /// <param name="positionCache">Position cache.</param>
        /// <param name="educationCache">Education cache.</param>
        /// <param name="mode">Mode (Add/Edit).</param>
        /// <param name="model">Staff model.</param>
        public StaffDialogViewModel(
            IUnitOfWork unitOfWork,
            IMessageBoxProvider messageBoxProvider,
            IGenderCache genderCache,
            IPositionCache positionCache,
            IEducationCache educationCache,
            WorkModeType mode,
            StaffModel model = null)
            : base(mode)
        {
            this.unitOfWork = unitOfWork;
            this.messageBoxProvider = messageBoxProvider;
            this.genderCache = genderCache;
            this.positionCache = positionCache;
            this.educationCache = educationCache;

            Task.Factory.StartNewWithDefaultCulture(() => this.Load(model));
        }

        /// <summary>
        /// Gets or sets all sex.
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
        /// Gets or sets all positions.
        /// </summary>
        public List<PositionModel> Positions
        {
            get
            {
                return this.positions;
            }

            set
            {
                this.positions = value;
                this.OnPropertyChanged(() => this.Positions);
            }
        }

        /// <summary>
        /// Gets or sets all educations.
        /// </summary>
        public List<EducationModel> Educations
        {
            get
            {
                return this.educations;
            }

            set
            {
                this.educations = value;
                this.OnPropertyChanged(() => this.Educations);
            }
        }

        /// <summary>
        /// Load information about staff.
        /// </summary>
        /// <param name="staffModel">Staff model.</param>
        protected override void Load(StaffModel staffModel)
        {
            this.Status = LoadingStatus.Loading;

            this.LoadModel(staffModel);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Save staff.
        /// </summary>
        protected override void Handle()
        {
            this.Status = LoadingStatus.Loading;

            this.UpdatePositionNameInModel();
            this.UpdateEducationNameInModel();

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

        private void LoadModel(StaffModel staffModel)
        {
            this.Genders = this.genderCache.Get();
            this.Positions = this.positionCache.Get();
            this.Educations = this.educationCache.Get();

            if (this.Mode == WorkModeType.Add)
            {
                this.Model = StaffModel.Create();

                if (this.Positions.Count > 0)
                {
                    this.Model.PositionId = this.Positions.First().Id;
                }

                if (this.Educations.Count > 0)
                {
                    this.Model.EducationId = this.Educations.First().Id;
                }
            }
            else if (this.Mode == WorkModeType.Edit)
            {
                this.Model = staffModel.Map();
            }
        }

        private void UpdatePositionNameInModel()
        {
            if (this.Positions.Count > 0)
            {
                var position = this.Positions.FirstOrDefault(p => p.Id == this.Model.PositionId);
                if (position != null)
                {
                    this.Model.PositionName = position.Name;
                }
            }
        }

        private void UpdateEducationNameInModel()
        {
            if (this.Educations.Count > 0)
            {
                var education = this.Educations.FirstOrDefault(e => e.Id == this.Model.EducationId);
                if (education != null)
                {
                    this.Model.EducationName = education.Name;
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

        private void AddItem()
        {
            this.unitOfWork.StaffRepository.Add(this.Model);
            this.Status = LoadingStatus.Added;
        }

        private void EditItem()
        {
            this.unitOfWork.StaffRepository.Update(this.Model);
            this.Status = LoadingStatus.Updated;
        }

        private void UpdateIdOfAddedItem(SaveChangesResponse response)
        {
            if (this.Status == LoadingStatus.Added)
            {
                this.Model.Id = response.TryGetValue(DatabaseEntity.Staffs.ToString());
            }
        }

        private void HandleFailure()
        {
            this.Status = LoadingStatus.Failed;
            this.messageBoxProvider.CannotBeSavedDueToError();
        }
    }
}