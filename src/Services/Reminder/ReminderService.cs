using System;
using System.Threading;
using Common.Services;
using Logger;
using Models.Enumeration;
using Services.Settings;

namespace Services.Reminder
{
    /// <summary>
    /// Represents reminder logic.
    /// </summary>
    public sealed class ReminderService : ServiceBase, IReminderService
    {
        private readonly ISettingsService settingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderService"/> class.
        /// </summary>
        /// <param name="settingsService">Setting service.</param>
        public ReminderService(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        /// <summary>
        /// Represents reminder logic.
        /// </summary>
        protected override void Do()
        {
            var waitTimeInMinutes = this.settingsService.GetInt(AvailableSettings.ReminderCheckDelay);
            var waitTimeInMilliseconds = TimeSpan.FromMinutes(waitTimeInMinutes);

            Log.Debug("Timeout has been read from the database. Timeout is {0} min.", Log.Args(waitTimeInMinutes));

            var manualResetEvent = new ManualResetEvent(false);

            while (true)
            {
                this.CallBack(manualResetEvent);
                manualResetEvent.WaitOne();
                manualResetEvent.Reset();

                Log.Debug("Going to sleep for {0} min.", Log.Args(waitTimeInMilliseconds));
                Thread.Sleep(waitTimeInMilliseconds);
            }
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        /// <param name="disposing">Define whether managed objects have to be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose();
        }
    }
}
