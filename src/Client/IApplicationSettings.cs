using Common.Enumeration;

namespace Client
{
    /// <summary>
    /// Represents application settings.
    /// </summary>
    public interface IApplicationSettings
    {
        /// <summary>
        /// Gets photo size limit in bytes
        /// </summary>
        int PhotoSizeLimitInBytes { get; }

        /// <summary>
        /// Gets photo size in pixels
        /// </summary>
        int PhotoSizeInPixels { get; }

        /// <summary>
        /// Gets active video device
        /// </summary>
        string VideoDevice { get; }

        /// <summary>
        /// Gets running mode
        /// </summary>
        RunningMode RunningMode { get; }
    }
}