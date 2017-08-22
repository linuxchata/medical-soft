using System.Management;

namespace Utilities
{
    /// <summary>
    /// Represents logic to get information about hardware.
    /// </summary>
    public class Hardware
    {
        /// <summary>
        /// Get environment information.
        /// </summary>
        /// <returns>Returns environment specific key.</returns>
        public string GetEnvironmentInfomation()
        {
            var key = string.Empty;
            var mos = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            var moc = mos.Get();

            foreach (var mo in moc)
            {
                key = (string)mo["ProcessorId"];
                break;
            }

            mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            moc = mos.Get();

            foreach (var mo in moc)
            {
                key += "#" + (string)mo["Product"];
                break;
            }

            return key;
        }
    }
}