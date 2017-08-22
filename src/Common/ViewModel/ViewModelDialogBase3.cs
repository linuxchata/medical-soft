using System.Collections.ObjectModel;
using Contracts.ViewModel.Base;
using Models;

namespace Common.ViewModel
{
    /// <summary>
    /// Represents dialog base view model.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public abstract class ViewModelDialogBase3<T> :
        ViewModelDialogBase2<T>,
        IViewModelDialogBase3<T> where T : ModelBase<T>
    {
        /// <summary>
        /// Represents list model.
        /// </summary>
        private ObservableCollection<T> listModel;

        /// <summary>
        /// Represents filtered model.
        /// </summary>
        private ObservableCollection<T> filteredModel;

        /// <summary>
        /// Gets or sets list model.
        /// </summary>
        public ObservableCollection<T> ListModel
        {
            get
            {
                return this.listModel;
            }

            set
            {
                this.listModel = value;
                this.OnPropertyChanged(() => this.ListModel);
            }
        }

        /// <summary>
        /// Gets or sets filtered model.
        /// </summary>
        public ObservableCollection<T> FilteredModel
        {
            get
            {
                return this.filteredModel;
            }

            set
            {
                this.filteredModel = value;
                this.OnPropertyChanged(() => this.FilteredModel);
            }
        }

        /// <summary>
        /// Gets or sets identification of the record.
        /// </summary>
        protected int Id { get; set; }
    }
}
