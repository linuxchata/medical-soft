using System;
using DataAccess;
using Models.Enumeration;

namespace Services.Settings
{
    /// <summary>
    /// Represents class to read settings.
    /// </summary>
    public class SettingsService : ISettingsService
    {
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsService"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        public SettingsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>Returns string setting.</returns>
        public string Get(AvailableSettings setting)
        {
            return this.unitOfWork.SettingRepository.GetById(setting).NvValue;
        }

        /// <summary>
        /// Get setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>Returns integer setting.</returns>
        public int GetInt(AvailableSettings setting)
        {
            var intValue = this.unitOfWork.SettingRepository.GetById(setting).IntValue;

            if (!intValue.HasValue)
            {
                throw new ArgumentException("Int value of the setting cannot be read", "setting");
            }

            return intValue.Value;
        }

        /// <summary>
        /// Get setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>Returns boolean setting.</returns>
        public bool GetBit(AvailableSettings setting)
        {
            var bitValue = this.unitOfWork.SettingRepository.GetById(setting).BitValue;

            if (!bitValue.HasValue)
            {
                throw new ArgumentException("Bit value of the setting cannot be read", "setting");
            }

            return bitValue.Value;
        }
    }
}
