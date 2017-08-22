using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Client.ViewModel.Dialogs;
using Common.Enumeration;
using Contracts;

namespace Client.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for PreviewDialogView.
    /// </summary>
    public partial class PreviewDialogView : IWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewDialogView"/> class.
        /// </summary>
        public PreviewDialogView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Data context changed event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = this.GetViewModel();
            Task.Factory.StartNewWithDefaultCulture(() => this.InitializePreviewAsync(viewModel));
        }

        /// <summary>
        /// On take picture event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnTakePicture(object sender, RoutedEventArgs e)
        {
            var syncContext = TaskScheduler.FromCurrentSynchronizationContext();

            var viewModel = this.GetViewModel();

            var task = Task.Factory.StartNew(() => this.TakePicture(viewModel));
            task.ContinueWith(
                antecedent =>
                {
                    viewModel.Handle(antecedent.Result);
                },
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            syncContext);
        }

        /// <summary>
        /// On closing event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var viewModel = this.GetViewModel();
            if (viewModel.Status == LoadingStatus.Loading)
            {
                e.Cancel = true;
            }
            else
            {
                this.PreviewPanel.Dispose();
            }
        }

        /// <summary>
        /// Initialize preview.
        /// </summary>
        /// <param name="viewModel">View model.</param>
        private void InitializePreviewAsync(PreviewDialogViewModel viewModel)
        {
            viewModel.SetStatus(LoadingStatus.Loading);

            this.PreviewPanel.InitializePreview();

            viewModel.SetStatus(LoadingStatus.Loaded);
        }

        /// <summary>
        /// Take picture.
        /// </summary>
        /// <param name="viewModel">View model.</param>
        /// <returns>Returns picture.</returns>
        private Bitmap TakePicture(PreviewDialogViewModel viewModel)
        {
            viewModel.SetStatus(LoadingStatus.Loading);

            var screenshot = this.PreviewPanel.TakePicture();

            viewModel.SetStatus(LoadingStatus.Added);

            return screenshot;
        }

        /// <summary>
        /// Get instance of the view model.
        /// </summary>
        /// <returns>Returns instance of the view model.</returns>
        private PreviewDialogViewModel GetViewModel()
        {
            var viewModel = this.DataContext as PreviewDialogViewModel;
            if (viewModel == null)
            {
                throw new InvalidOperationException("View model must be initialized to data context.");
            }

            return viewModel;
        }
    }
}
