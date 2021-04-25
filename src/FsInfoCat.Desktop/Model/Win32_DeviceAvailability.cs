using System.ComponentModel;

namespace FsInfoCat.Desktop
{
    public enum Win32_DeviceAvailability
    {
        [Description("Other")]
        Other = 1,

        [Description("Unknown")]
        Unknown = 2,

        /// <summary>
        /// Running or Full Power
        /// </summary>
        [Description("Running/Full Power")]
        RunningFullPower = 3,

        [Description("Warning")]
        Warning = 4,

        [Description("In Test")]
        InTest = 5,

        [Description("Not Applicable")]
        NotApplicable = 6,

        [Description("Power Off")]
        PowerOff = 7,

        /// <summary>
        /// Offline
        /// </summary>
        [Description("Off Line")]
        OffLine = 8,

        [Description("Off Duty")]
        OffDuty = 9,

        [Description("Degraded")]
        Degraded = 10,

        [Description("Not Installed")]
        NotInstalled = 11,

        [Description("Install Error")]
        InstallError = 12,

        /// <summary>
        /// The device is known to be in a power save mode, but its exact status is unknown.
        /// </summary>
        [Description("Power Save - Unknown")]
        PowerSaveUnknown = 13,

        /// <summary>
        /// The device is in a power save state, but still functioning, and may exhibit degraded performance.
        /// </summary>
        [Description("Power Save - Low Power Mode")]
        PowerSaveLowPowerMode = 14,

        /// <summary>
        /// The device is not functioning, but could be brought to full power quickly.
        /// </summary>
        [Description("Power Save - Standby")]
        PowerSaveStandby = 15,

        [Description("Power Cycle")]
        PowerCycle = 16,

        /// <summary>
        /// The device is in a warning state, but also in a power save mode.
        /// </summary>
        [Description("Power Save - Warning")]
        PowerSaveWarning = 17,

        /// <summary>
        /// The device is paused.
        /// </summary>
        [Description("Paused")]
        Paused = 18,

        /// <summary>
        /// The device is not ready.
        /// </summary>
        [Description("Not Ready")]
        NotReady = 19,

        /// <summary>
        /// The device is not configured.
        /// </summary>
        [Description("Not Configured")]
        NotConfigured = 20,

        /// <summary>
        /// The device is quiet.
        /// </summary>
        [Description("Quiesced")]
        Quiesced = 21
    }
}
