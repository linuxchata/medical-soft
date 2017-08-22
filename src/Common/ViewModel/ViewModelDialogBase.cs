using System.Windows.Input;
using Common.Commands;
using Common.Enumeration;
using Contracts.ViewModel.Base;

namespace Common.ViewModel
{
    /// <summary>
    /// Represents dialog base view-model.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public abstract class ViewModelDialogBase<T> : ViewModelBase, IViewModelDialogBase<T>
    {
        /// <summary>
        /// Represents model.
        /// </summary>
        private T model;

        /// <summary>
        /// Cancel command.
        /// </summary>
        private ICommand cancelCommand;

        /// <summary>
        /// Status of the operation.
        /// </summary>
        private LoadingStatus status;

        /// <summary>
        /// Gets or sets model.
        /// </summary>
        public virtual T Model
        {
            get
            {
                return this.model;
            }

            set
            {
                this.model = value;
                this.OnPropertyChanged(() => this.Model);
            }
        }

        /// <summary>
        /// Gets or sets status of the operation.
        /// </summary>
        public LoadingStatus Status
        {
            get
            {
                return this.status;
            }

            protected set
            {
                this.status = value;
                this.OnPropertyChanged(() => this.Status);
            }
        }

        /// <summary>
        /// Gets cancel command.
        /// </summary>
        public virtual ICommand CancelCommand
        {
            get
            {
                return this.cancelCommand ?? (this.cancelCommand = new CommonCommand(
                    param =>
                    {
                        this.Status = LoadingStatus.Canceled;
                        base.OnRequestClose();
                    },
                    param => this.CanCancel));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the cancel command is enabled.
        /// </summary>
        protected virtual bool CanCancel
        {
            get
            {
                return true;
            }
        }
    }
}
