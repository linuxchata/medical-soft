using System;
using System.Windows;
using System.Windows.Input;
using Client.ViewModel;
using Logger;
using Microsoft.Practices.Unity;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="container">Unity container.</param>
        public MainWindow(IUnityContainer container)
        {
            #if DEBUG
                // Trace binding errors.
                BindingErrorTraceListener.SetTrace();
            #endif

            Log.Debug("InitializeComponent of MainWindow.");
            this.InitializeComponent();

            // Fix issue with no windows border.
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight - (3 * SystemParameters.FixedFrameHorizontalBorderHeight);

            Log.Debug("Initialization of MainViewModel.");
            var mainViewModel = container.Resolve<MainViewModel>();
            this.DataContext = mainViewModel;
            Log.Debug("MainViewModel was initialized.");
        }

        private void MainWindowClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Maximize(object sender, RoutedEventArgs e)
        {
            this.AdjustWindowSize();
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                {
                    this.AdjustWindowSize();
                }
                else
                {
                    var window = Application.Current.MainWindow;

                    if (window != null)
                    {
                        window.DragMove();
                    }
                }
            }
        }

        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.BorderThickness = new Thickness(1);
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.BorderThickness = new Thickness(6, 6, 6, 0);
                this.WindowState = WindowState.Maximized;
            }
        }
    }
}
