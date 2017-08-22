using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client.Contracts;
using Client.Contracts.Dialogs;
using Client.Providers;
using Client.ViewModel.Dialogs;
using Common.Builder;
using Common.Enumeration;
using Common.Events;
using Common.ViewModel;
using DataAccess;
using Logger;
using Models;
using Scheduler;
using Utilities.EventAggregator;

namespace Client.ViewModel
{
    /// <summary>
    /// Represents view model for appointment.
    /// </summary>
    public sealed class AppointmentViewModel : ViewModelNotifyBase, IAppointmentViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IEventAggregator eventAggregator;

        private readonly IViewModelBuilder viewModelBuilder;

        private readonly IViewBuilder viewBuilder;

        private readonly IMessageBoxProvider messageBoxProvider;

        private List<ItemModel> staffItemsModel;

        private ItemModel selectedStaffItem;

        private LoadingStatus status;

        private string culture;

        private List<AppointmentModel> model;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="eventAggregator">Event aggregator.</param>
        /// <param name="viewModelBuilder">Appointment dialog view model.</param>
        /// <param name="viewBuilder">View builder.</param>
        /// <param name="messageBoxProvider">Message box provider.</param>
        public AppointmentViewModel(
            IUnitOfWork unitOfWork,
            IEventAggregator eventAggregator,
            IViewModelBuilder viewModelBuilder,
            IViewBuilder viewBuilder,
            IMessageBoxProvider messageBoxProvider)
        {
            this.unitOfWork = unitOfWork;
            this.eventAggregator = eventAggregator;
            this.viewModelBuilder = viewModelBuilder;
            this.viewBuilder = viewBuilder;
            this.messageBoxProvider = messageBoxProvider;

            this.Culture = Thread.CurrentThread.CurrentCulture.Name;

            Task.Factory.StartNewWithDefaultCulture(this.PopulateModelAndStaff);
        }

        /// <summary>
        /// Gets or sets model for staff items.
        /// </summary>
        public List<ItemModel> StaffItemsModel
        {
            get
            {
                return this.staffItemsModel;
            }

            set
            {
                this.staffItemsModel = value;
                this.OnPropertyChanged(() => this.StaffItemsModel);
            }
        }

        /// <summary>
        /// Gets or sets selected staff items.
        /// </summary>
        public ItemModel SelectedStaffItem
        {
            get
            {
                return this.selectedStaffItem;
            }

            set
            {
                this.selectedStaffItem = value;

                if (this.selectedStaffItem != null)
                {
                    this.Model = this.ModelFull.FindAll(a => a.Item1Id == this.selectedStaffItem.Id);
                }

                this.OnPropertyChanged(() => this.SelectedStaffItem);
            }
        }

        /// <summary>
        /// Gets or sets model for appointment.
        /// </summary>
        public List<AppointmentModel> Model
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

            set
            {
                this.status = value;
                this.OnPropertyChanged(() => this.Status);
            }
        }

        /// <summary>
        /// Gets or sets culture name.
        /// </summary>
        public string Culture
        {
            get
            {
                return this.culture;
            }

            set
            {
                this.culture = value;
                this.OnPropertyChanged(() => this.Culture);
            }
        }

        /// <summary>
        /// Gets or sets full model for appointment.
        /// This list include all appointments for all staff.
        /// </summary>
        private List<AppointmentModel> ModelFull { get; set; }

        /// <summary>
        /// Add an appointment event handler.
        /// </summary>
        /// <param name="currentSelectedDate">Currently selected date.</param>
        /// <param name="selectedStartHour">Selected start hour.</param>
        /// <param name="selectedStartMinute">Selected start minute.</param>
        public void OnAdd(DateTime currentSelectedDate, int selectedStartHour, int selectedStartMinute)
        {
            try
            {
                this.BuildAndShowAddDialog(currentSelectedDate, selectedStartHour, selectedStartMinute);

                this.PopulateModel();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Edit the appointment event handler.
        /// </summary>
        /// <param name="appointmentGuid">Unique number of the selected appointment.</param>
        public void OnEdit(Guid appointmentGuid)
        {
            try
            {
                var appointment = this.unitOfWork.AppointmentRepository.GetById(appointmentGuid);

                this.BuildAndShowEditDialog(appointment);

                this.PopulateModel();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Delete the appointment event handler.
        /// </summary>
        /// <param name="appointmentGuid">Unique number of the selected appointment.</param>
        public void OnDelete(Guid appointmentGuid)
        {
            if (this.IsDeletionConfirmedByUser())
            {
                var isDeleted = this.TryDeleteItemFromDatasource(appointmentGuid);
                if (isDeleted)
                {
                    this.PopulateModel();
                }
            }
        }

        /// <summary>
        /// Subscribe method.
        /// </summary>
        public void Subscribe()
        {
            this.eventAggregator.Subscribe<PatientChangedEvent>(this.PopulateModel);
            this.eventAggregator.Subscribe<StaffAddedEvent>(this.PopulateStaff);
            this.eventAggregator.Subscribe<StaffChangedEvent>(this.PopulateStaff);
            this.eventAggregator.Subscribe<StaffDeletedEvent>(this.UpdateAfterStaffDeleted);
        }

        /// <summary>
        /// Unsubscribe method.
        /// </summary>
        public void Unsubscribe()
        {
            this.eventAggregator.Unsubscribe<PatientChangedEvent>();
            this.eventAggregator.Unsubscribe<StaffAddedEvent>();
            this.eventAggregator.Unsubscribe<StaffChangedEvent>();
            this.eventAggregator.Unsubscribe<StaffDeletedEvent>();
        }

        private void PopulateModel()
        {
            this.Status = LoadingStatus.Loading;

            this.PopulateFullModel();
            this.PopulateModelForSelectedStaff();

            this.Status = LoadingStatus.Loaded;
        }

        private void PopulateModelAndStaff()
        {
            this.Status = LoadingStatus.Loading;

            this.PopulateFullModel();
            this.PopulateStaff();
            this.PopulateModelForSelectedStaff();

            this.Status = LoadingStatus.Loaded;
        }

        private void PopulateFullModel()
        {
            this.ModelFull = this.unitOfWork.AppointmentRepository.GetAll().ToList();
        }

        private void PopulateModelForSelectedStaff()
        {
            if (this.SelectedStaffItem != null)
            {
                this.Model = this.ModelFull.FindAll(a => a.Item1Id == this.SelectedStaffItem.Id);
            }
        }

        private void PopulateStaff()
        {
            this.StaffItemsModel = this.unitOfWork.StaffRepository.GetAllIsTakingForList().ToList();

            this.SelectedStaffItem = this.SelectedStaffItem ?? this.StaffItemsModel.FirstOrDefault();
        }

        private void UpdateAfterStaffDeleted()
        {
            this.PopulateModelAndStaff();

            this.SelectedStaffItem = this.StaffItemsModel.FirstOrDefault();
        }

        private void BuildAndShowAddDialog(DateTime currentDate, int startHour, int startMinute)
        {
            var viewModel = this.viewModelBuilder.Build<AppointmentDialogViewModel>();
            viewModel.InitializeForAdd(this.ModelFull, currentDate, startHour, startMinute, this.SelectedStaffItem);
            this.BuildAndShowDialog(viewModel);
        }

        private void BuildAndShowEditDialog(AppointmentModel appointment)
        {
            var viewModel = this.viewModelBuilder.Build<IAppointmentDialogViewModel>();
            viewModel.InitializeForEdit(this.ModelFull, appointment);
            this.BuildAndShowDialog(viewModel);
        }

        private void BuildAndShowDialog(IAppointmentDialogViewModel viewModel)
        {
            this.viewBuilder.Build<AppointmentView, IAppointmentDialogViewModel>(viewModel).ShowDialog();
        }

        private bool IsDeletionConfirmedByUser()
        {
            return this.messageBoxProvider.ConfirmDelete() == MessageBoxResult.Yes;
        }

        private bool TryDeleteItemFromDatasource(Guid appointmentGuid)
        {
            this.unitOfWork.AppointmentRepository.Delete(appointmentGuid);
            var response = this.unitOfWork.Save();
            return response.IsSuccessful;
        }
    }
}
