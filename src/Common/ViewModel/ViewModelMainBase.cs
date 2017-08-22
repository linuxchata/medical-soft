using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Commands;
using Common.Enumeration;
using Contracts.ViewModel.Base;

namespace Common.ViewModel
{
    /// <summary>
    /// Represents main base view model.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public abstract class ViewModelMainBase<T> : ViewModelNotifyBase, IViewModelMainBase<T> where T : class
    {
        /// <summary>
        /// Represents model for T.
        /// </summary>
        private ObservableCollection<T> model;

        /// <summary>
        /// Selected item of T.
        /// </summary>
        private T selectedItem;

        /// <summary>
        /// Add education dialog command.
        /// </summary>
        private ICommand addDialogCommand;

        /// <summary>
        /// Edit education dialog command.
        /// </summary>
        private ICommand editDialogCommand;

        /// <summary>
        /// Delete education.
        /// </summary>
        private ICommand deleteCommand;

        /// <summary>
        /// Status of the operation.
        /// </summary>
        private LoadingStatus status;

        /// <summary>
        /// Gets or sets model for T.
        /// </summary>
        public ObservableCollection<T> Model
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
        /// Gets count of items in the model.
        /// </summary>
        public virtual int Count
        {
            get
            {
                var count = 0;

                if (this.Model != null)
                {
                    count = this.Model.Count;
                }

                return count;
            }
        }

        /// <summary>
        /// Gets or sets currently selected item of T.
        /// </summary>
        public T SelectedItem
        {
            get
            {
                return this.selectedItem;
            }

            set
            {
                this.selectedItem = value;
                this.OnPropertyChanged(() => this.SelectedItem);
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
        /// Gets add dialog command.
        /// </summary>
        public ICommand AddDialogCommand
        {
            get
            {
                return this.addDialogCommand ?? (this.addDialogCommand = new CommonCommand(
                    param => this.AddEditDialog(WorkModeType.Add),
                    param => true));
            }
        }

        /// <summary>
        /// Gets edit dialog command.
        /// </summary>
        public ICommand EditDialogCommand
        {
            get
            {
                return this.editDialogCommand ?? (this.editDialogCommand = new CommonCommand(
                    param => this.AddEditDialog(WorkModeType.Edit),
                    param => this.SelectedItem != null));
            }
        }

        /// <summary>
        /// Gets delete command.
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                return this.deleteCommand ?? (this.deleteCommand = new CommonCommand(
                    param => this.Delete(),
                    param => this.SelectedItem != null));
            }
        }

        /// <summary>
        /// Load information for model.
        /// </summary>
        public abstract void UpdateData();

        /// <summary>
        /// Load information for model.
        /// </summary>
        public virtual void UpdateDataAsync()
        {
            Task.Factory.StartNew(this.UpdateData);
        }

        /// <summary>
        /// Subscribe method.
        /// </summary>
        public virtual void Subscribe()
        {
        }

        /// <summary>
        /// Unsubscribe method.
        /// </summary>
        public virtual void Unsubscribe()
        {
        }

        /// <summary>
        /// Add/Edit dialog.
        /// </summary>
        /// <param name="mode">The mode (Add/Edit).</param>
        protected abstract void AddEditDialog(WorkModeType mode);

        /// <summary>
        /// Delete education.
        /// </summary>
        protected abstract void Delete();
    }
}
