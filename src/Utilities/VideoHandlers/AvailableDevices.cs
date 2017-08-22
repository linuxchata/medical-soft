using System;
using System.Collections.Generic;
using System.Linq;
using Logger;
using Microsoft.Expression.Encoder.Devices;

namespace Utilities.VideoHandlers
{
    /// <summary>
    /// Represents helper class to list of the all available video devices.
    /// </summary>
    public static class AvailableDevices
    {
        /// <summary>
        /// Find available video devices.
        /// </summary>
        /// <returns>Returns list of the available video devices.</returns>
        public static List<string> FindVideoDevies()
        {
            try
            {
                var devices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
                var devicesNames = devices.Select(a => a.Name).ToList();

                // Dispose devices.
                for (var i = 1; i < devices.Count; ++i)
                {
                    devices[i].Dispose();
                }

                devices.Clear();

                return devicesNames;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new List<string>();
            }
        }
    }
}