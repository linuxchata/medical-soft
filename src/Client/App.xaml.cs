using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using DataAccess;
using Logger;
using Microsoft.Practices.Unity;
using Models.Enumeration;

namespace Client
{
    /// <summary>
    /// Interaction logic for App
    /// </summary>
    public partial class App
    {
        private static Bootstrap bootstraper;

        /// <summary>
        /// Initializes static members of the App class.
        /// </summary>
        static App()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += UnhandledExceptionHandler;

            InitBootstrap();

            var language = GetLanguage();

            SetCulture(language);
        }

        /// <summary>
        /// Override startup method.
        /// </summary>
        /// <param name="e">The vent argument.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if RELEASE
            this.MainWindow = bootstraper.Container.Resolve<Client.Views.Dialogs.LoginDialog>();
            this.MainWindow.Show();
#endif
#if (!RELEASE)
            Log.Debug("Resolve MainWindow.");
            this.MainWindow = bootstraper.Container.Resolve<MainWindow>();
            this.MainWindow.Show();
#endif
        }

        private static void InitBootstrap()
        {
            Log.Debug("Initializing bootstrap.");

            bootstraper = new Bootstrap();
            bootstraper.Register();

            Log.Debug("Bootstrap was initialized.");
        }

        private static string GetLanguage()
        {
            var unitOfWork = bootstraper.Container.Resolve<IUnitOfWork>();
            var language = unitOfWork.SettingRepository.GetById(AvailableSettings.Language).NvValue;
            Log.Debug("Languages {0} was defined.", Log.Args(language));

            return language;
        }

        private static void SetCulture(string language)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture(language ?? Thread.CurrentThread.CurrentCulture.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Client.Properties.Resources.Culture = cultureInfo;

            Log.Debug("Culture {0} for current session was defined.", Log.Args(cultureInfo));
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;

            if (exception != null)
            {
                Log.Error("Unhandled exception ." + exception.Message);
                Log.Error("Runtime terminating ." + e.IsTerminating);
            }
        }
    }
}