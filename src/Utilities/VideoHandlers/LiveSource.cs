using System;
using System.Collections.Generic;
using System.Linq;
using Logger;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;

namespace Utilities.VideoHandlers
{
    /// <summary>
    /// Represents live source implementation (live preview from video device).
    /// </summary>
    public class LiveSource
    {
        private readonly LiveJob job;

        private readonly List<EncoderDevice> videoDevices;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveSource"/> class.
        /// </summary>
        public LiveSource()
        {
            try
            {
                // Starts new job for preview window.
                this.job = new LiveJob();

                // Aquires audio and video devices.
                this.videoDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video).ToList();
                var video = this.videoDevices.Count > 0 ? this.videoDevices[1] : null;

                // Checks for video devices.
                if (video != null)
                {
                    // Create a new device source. We use the first audio and video devices on the system.
                    this.DeviceSource = this.job.AddDeviceSource(video, null);

                    // Make this source the active one.
                    this.job.ActivateSource(this.DeviceSource);
                }
                else
                {
                    this.HasError = true;
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Gets device for live source.
        /// </summary>
        public LiveDeviceSource DeviceSource { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the error occurred.
        /// </summary>
        public bool HasError { get; private set; }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        public void Dispose()
        {
            // Dispose devices.
            for (var i = 1; i < this.videoDevices.Count; ++i)
            {
                this.videoDevices[i].Dispose();
            }

            this.videoDevices.Clear();

            // Closes devices for preview.
            if (this.DeviceSource != null)
            {
                this.job.RemoveDeviceSource(this.DeviceSource);
                this.DeviceSource.Dispose();
            }

            // Dispose the job.
            if (this.job != null)
            {
                this.job.Dispose();
            }
        }
    }
}