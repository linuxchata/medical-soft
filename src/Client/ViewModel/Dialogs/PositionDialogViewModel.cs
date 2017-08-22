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
    /// Represents view model for position view.
    /// </summary>
    public sealed class PositionDialogViewModel : ViewModelDialogBase2<PositionModel>, IPositionDialogViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMessageBoxProvider messageBoxProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionDialogViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        /// <param name="mode">Mode (Add/Edit).</param>
        /// <param name="model">Position model.</param>
        public PositionDialogViewModel(
            IUnitOfWork unitOfWork,
            IMessageBoxProvider messageBoxProvider,
            WorkModeType mode,
            PositionModel model = null)
            : base(mode)
        {
            this.unitOfWork = unitOfWork;
            this.messageBoxProvider = messageBoxProvider;

            Task.Factory.StartNewWithDefaultCulture(() => this.Load(model));
        }

        /// <summary>
        /// Load information about position.
        /// </summary>
        /// <param name="positionModel">Position model.</param>
        protected override void Load(PositionModel positionModel)
        {
            this.Status = LoadingStatus.Loading;

            this.LoadModel(positionModel);

            this.Status = LoadingStatus.Loaded;
        }

        /// <summary>
        /// Save position.
        /// </summary>
        protected override void Handle()
        {
            this.Status = LoadingStatus.Loading;

            var result = this.SaveChanges();
            if (result.IsSuccessful)
            {
                this.UpdateIdOfAddedItem(result);
            }
            else
            {
                this.HandleFailure();
            }

            this.CloseDialog();
        }

        private void LoadModel(PositionModel positionModel)
        {
            if (this.Mode == WorkModeType.Add)
            {
                this.Model = new PositionModel();
            }
            else if (this.Mode == WorkModeType.Edit)
            {
                this.Model = positionModel.Map();
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
            this.unitOfWork.PositionRepository.Add(this.Model);
            this.Status = LoadingStatus.Added;
        }

        private void EditItem()
        {
            this.unitOfWork.PositionRepository.Update(this.Model);
            this.Status = LoadingStatus.Updated;
        }

        private void UpdateIdOfAddedItem(SaveChangesResponse result)
        {
            if (this.Status == LoadingStatus.Added)
            {
                this.Model.Id = result.TryGetValue(DatabaseEntity.Positions.ToString());
            }
        }

        private void HandleFailure()
        {
            this.Status = LoadingStatus.Failed;
            this.messageBoxProvider.CannotBeSavedDueToError();
        }
    }
}
