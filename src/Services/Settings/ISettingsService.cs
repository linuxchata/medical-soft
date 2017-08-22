using Models.Enumeration;

namespace Services.Settings
{
    /// <summary>
    /// Represents interface to read settings.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Get setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>Returns string setting.</returns>
        string Get(AvailableSettings setting);

        /// <summary>
        /// Get setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>Returns integer setting.</returns>
        int GetInt(AvailableSettings setting);

        /// <summary>
        /// Get setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>Returns boolean setting.</returns>
        bool GetBit(AvailableSettings setting);
    }
}