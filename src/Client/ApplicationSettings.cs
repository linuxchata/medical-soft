using System;
using Common.Enumeration;

namespace Client
{
    /// <summary>
    /// Represents application settings.
    /// </summary>
    public class ApplicationSettings : IApplicationSettings
    {
        /// <summary>
        /// Gets photo size limit in bytes
        /// </summary>
        public int PhotoSizeLimitInBytes
        {
            get
            {
                return ApplicationSettingsManager.TryReadIntValue("PhotoSizeLimitInBytes");
            }
        }

        /// <summary>
        /// Gets photo size in pixels
        /// </summary>
        public int PhotoSizeInPixels
        {
            get
            {
                return ApplicationSettingsManager.TryReadIntValue("PhotoSizeInPixels");
            }
        }

        /// <summary>
        /// Gets active video device
        /// </summary>
        public string VideoDevice
        {
            get
            {
                return ApplicationSettingsManager.TryReadValue("VideoDevice");
            }
        }

        /// <summary>
        /// Gets running mode
        /// </summary>
        public RunningMode RunningMode
        {
            get
            {
                var runningMode = ApplicationSettingsManager.TryReadValue("RunningMode");

                RunningMode result;

                if (!Enum.TryParse(runningMode, true, out result))
                {
                    throw new ArgumentOutOfRangeException("RunningMode", "Running mode must be set to Server or ClientOnly in application configuration file");
                }

                return result;
            }
        }
    }
}
