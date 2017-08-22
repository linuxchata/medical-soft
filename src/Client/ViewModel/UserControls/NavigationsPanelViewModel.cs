using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Client.Contracts;
using Common.ViewModel;
using Contracts.ViewModel;
using Contracts.ViewModel.Base;
using Contracts.ViewModel.UserControls;
using Microsoft.Practices.Unity;
using Models;
using Models.Enumeration;
using Utilities.Resource;

namespace Client.ViewModel.UserControls
{
    /// <summary>
    /// Represents navigation view model.
    /// </summary>
    public class NavigationsPanelViewModel : ViewModelNotifyBase, INavigationsPanelViewModel
    {
        private readonly IUnityContainer container;

        private readonly IResourceHandler resourceHandler;

        private Utilities.WeakReference<object> currentViewModel;

        private ObservableCollection<NavigationModel> leftPanelItems;

        private ObservableCollection<NavigationModel> rightPanelItems;

        private NavigationModel selectedLeftPanelItem;

        private NavigationModel selectedRightPanelItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationsPanelViewModel"/> class.
        /// </summary>
        /// <param name="container">Unity container.</param>
        /// <param name="resourceHandler">Resource handler.</param>
        public NavigationsPanelViewModel(IUnityContainer container, IResourceHandler resourceHandler)
        {
            this.container = container;
            this.resourceHandler = resourceHandler;

            // Left panel items.
            var leftItems = new List<NavigationModel>();
            foreach (NavigationLeftItem item in Enum.GetValues(typeof(NavigationLeftItem)))
            {
                var navigationModel = this.CreateLeftNavigationModel(item);
                leftItems.Add(navigationModel);
            }

            // Right panel items.
            var rightItems = new List<NavigationModel>();
            foreach (NavigationRightItem item in Enum.GetValues(typeof(NavigationRightItem)))
            {
                var navigationModel = this.CreateRightNavigationModel(item);
                rightItems.Add(navigationModel);
            }

            this.LeftPanelItems = new ObservableCollection<NavigationModel>(leftItems);
            this.RightPanelItems = new ObservableCollection<NavigationModel>(rightItems);

            this.SelectedLeftPanelItem = this.LeftPanelItems.FirstOrDefault();
        }

        /// <summary>
        /// Gets or sets left panel items.
        /// </summary>
        public ObservableCollection<NavigationModel> LeftPanelItems
        {
            get
            {
                return this.leftPanelItems;
            }

            set
            {
                this.leftPanelItems = value;
                this.OnPropertyChanged(() => this.LeftPanelItems);
            }
        }

        /// <summary>
        /// Gets or sets right panel items.
        /// </summary>
        public ObservableCollection<NavigationModel> RightPanelItems
        {
            get
            {
                return this.rightPanelItems;
            }

            set
            {
                this.rightPanelItems = value;
                this.OnPropertyChanged(() => this.RightPanelItems);
            }
        }

        /// <summary>
        /// Gets or sets selected left panel item.
        /// </summary>
        public NavigationModel SelectedLeftPanelItem
        {
            get
            {
                return this.selectedLeftPanelItem;
            }

            set
            {
                this.selectedLeftPanelItem = value;

                switch (this.selectedLeftPanelItem.NavigationLeftItemId)
                {
                    case NavigationLeftItem.Patient:
                        this.PatientViewModel = this.ResolveAndSetCurrentViewModel(this.PatientViewModel);
                        break;
                    case NavigationLeftItem.Staff:
                        this.StaffViewModel = this.ResolveAndSetCurrentViewModel(this.StaffViewModel);
                        break;
                    case NavigationLeftItem.Reminder:
                        this.ReminderViewModel = this.ResolveAndSetCurrentViewModel(this.ReminderViewModel);
                        break;
                    case NavigationLeftItem.NotificationTemplate:
                        this.NotificationTemplateViewModel = this.ResolveAndSetCurrentViewModel(this.NotificationTemplateViewModel);
                        break;
                    case NavigationLeftItem.NotificationGroup:
                        this.NotificationGroupViewModel = this.ResolveAndSetCurrentViewModel(this.NotificationGroupViewModel);
                        break;
                    case NavigationLeftItem.Appointment:
                        this.AppointmentViewModel = this.ResolveAndSetCurrentViewModel(this.AppointmentViewModel);
                        break;
                    case NavigationLeftItem.Login:
                        this.LoginViewModel = this.ResolveAndSetCurrentViewModel(this.LoginViewModel);
                        break;
                    case NavigationLeftItem.Education:
                        this.EducationViewModel = this.ResolveAndSetCurrentViewModel(this.EducationViewModel);
                        break;
                    case NavigationLeftItem.Position:
                        this.PositionViewModel = this.ResolveAndSetCurrentViewModel(this.PositionViewModel);
                        break;
                    case NavigationLeftItem.Backup:
                        this.BackupViewModel = this.ResolveAndSetCurrentViewModel(this.BackupViewModel);
                        break;
                }

                this.OnPropertyChanged(() => this.SelectedLeftPanelItem);
            }
        }

        /// <summary>
        /// Gets or sets selected right panel item.
        /// </summary>
        public NavigationModel SelectedRightPanelItem
        {
            get
            {
                return this.selectedRightPanelItem;
            }

            set
            {
                this.selectedRightPanelItem = value;

                switch (this.selectedRightPanelItem.NavigationRightItemId)
                {
                    case NavigationRightItem.Setting:
                        this.Resolve<ISettingViewModel>().ShowCommand.Execute(null);
                        break;
                    case NavigationRightItem.About:
                        this.Resolve<IAboutViewModel>().ShowCommand.Execute(null);
                        break;
                }

                Application.Current.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        this.selectedRightPanelItem = null;
                        OnPropertyChanged(() => this.SelectedRightPanelItem);
                    }),
                    DispatcherPriority.Normal,
                    null);

                this.OnPropertyChanged(() => this.SelectedRightPanelItem);
            }
        }

        /// <summary>
        /// Gets or sets current view model.
        /// </summary>
        public Utilities.WeakReference<object> CurrentViewModel
        {
            get
            {
                return this.currentViewModel;
            }

            set
            {
                this.currentViewModel = value;
                this.OnPropertyChanged(() => this.CurrentViewModel);
            }
        }

        /// <summary>
        /// Gets or sets patient view model.
        /// </summary>
        public Utilities.WeakReference<IPatientViewModel> PatientViewModel { get; set; }

        /// <summary>
        /// Gets or sets staff view model.
        /// </summary>
        public Utilities.WeakReference<IStaffViewModel> StaffViewModel { get; set; }

        /// <summary>
        /// Gets or sets reminder view model.
        /// </summary>
        public Utilities.WeakReference<IReminderViewModel> ReminderViewModel { get; set; }

        /// <summary>
        /// Gets or sets notification template view model.
        /// </summary>
        public Utilities.WeakReference<INotificationTemplateViewModel> NotificationTemplateViewModel { get; set; }

        /// <summary>
        /// Gets or sets notification group view model.
        /// </summary>
        public Utilities.WeakReference<INotificationGroupViewModel> NotificationGroupViewModel { get; set; }

        /// <summary>
        /// Gets or sets appointment view model.
        /// </summary>
        public Utilities.WeakReference<IAppointmentViewModel> AppointmentViewModel { get; set; }

        /// <summary>
        /// Gets or sets login view model.
        /// </summary>
        public Utilities.WeakReference<ILoginViewModel> LoginViewModel { get; set; }

        /// <summary>
        /// Gets or sets position view model.
        /// </summary>
        public Utilities.WeakReference<IPositionViewModel> PositionViewModel { get; set; }

        /// <summary>
        /// Gets or sets education view model.
        /// </summary>
        public Utilities.WeakReference<IEducationViewModel> EducationViewModel { get; set; }

        /// <summary>
        /// Gets or sets backup view model.
        /// </summary>
        public Utilities.WeakReference<IBackupViewModel> BackupViewModel { get; set; }

        /// <summary>
        /// Gets or sets setting view model.
        /// </summary>
        public ISettingViewModel SettingViewModel { get; set; }

        /// <summary>
        /// Gets or sets about box view model.
        /// </summary>
        public IAboutViewModel AboutViewModel { get; set; }

        private NavigationModel CreateRightNavigationModel(NavigationRightItem item)
        {
            var navigationModel = this.CreateNavigationModel(item.ToString());
            navigationModel.NavigationRightItemId = item;

            return navigationModel;
        }

        private NavigationModel CreateLeftNavigationModel(NavigationLeftItem item)
        {
            var navigationModel = this.CreateNavigationModel(item.ToString());
            navigationModel.NavigationLeftItemId = item;

            return navigationModel;
        }

        private NavigationModel CreateNavigationModel(string item)
        {
            var navigationModel = new NavigationModel
            {
                Name = this.resourceHandler.GetValue(string.Format("Navigation{0}", item)),
                Style = string.Format("PathNavigation{0}", item)
            };

            return navigationModel;
        }

        private Utilities.WeakReference<T> ResolveAndSetCurrentViewModel<T>(Utilities.WeakReference<T> viewModel) where T : class
        {
            if (viewModel == null || !viewModel.IsAlive)
            {
                viewModel = new Utilities.WeakReference<T>(this.container.Resolve<T>());
            }

            this.CurrentViewModel = new Utilities.WeakReference<object>(viewModel.Target);

            var subscribableObject = this.CurrentViewModel.Target as ISubscribable;
            if (subscribableObject != null)
            {
                subscribableObject.Subscribe();
            }

            return viewModel;
        }

        private T Resolve<T>() where T : class
        {
            return this.container.Resolve<T>();
        }
    }
}
