using System;
using System.Configuration;
using Logger;

namespace Client
{
    /// <summary>
    /// Represent class to read application setting from app.config.
    /// </summary>
    public static class ApplicationSettingsManager
    {
        /// <summary>
        /// Try read string value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Return string value.</returns>
        public static string TryReadValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw;
            }
        }

        /// <summary>
        /// Try read integer value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Return integer value.</returns>
        public static int TryReadIntValue(string key)
        {
            try
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings[key]);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw;
            }
        }

        /// <summary>
        /// Try write string value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void TryWriteValue(string key, string value)
        {
            try
            {
                var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configuration.AppSettings.Settings[key].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw;
            }
        }
    }
}
