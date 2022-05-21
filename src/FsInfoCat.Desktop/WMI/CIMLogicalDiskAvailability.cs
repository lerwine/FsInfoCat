namespace FsInfoCat.Desktop.WMI
{
    public enum CIMLogicalDiskAvailability
    {
        Other = 1,

        Unknown = 2,

        /// <summary>
        /// Running or Full Power
        /// </summary>
        RunningFullPower = 3,

        Warning = 4,

        /// <summary>
        /// In Test
        /// </summary>
        InTest = 5,

        /// <summary>
        /// Not Applicable
        /// </summary>
        NotApplicable = 6,

        /// <summary>
        /// Power Off
        /// </summary>
        PowerOff = 7,

        /// <summary>
        /// Off Line
        /// </summary>
        OffLine = 8,

        /// <summary>
        /// Off Duty
        /// </summary>
        OffDuty = 9,

        Degraded = 10,

        /// <summary>
        /// Not Installed
        /// </summary>
        NotInstalled = 11,

        /// <summary>
        /// Install Error
        /// </summary>
        InstallError = 12,

        /// <summary>
        /// The device is known to be in a power save mode, but its exact status is unknown.
        /// </summary>
        PowerSaveUnknown = 13,

        /// <summary>
        /// The device is in a power save state, but still functioning, and may exhibit degraded performance.
        /// </summary>
        PowerSaveLowPowerMode = 14,

        /// <summary>
        /// The device is not functioning, but could be brought to full power quickly.
        /// </summary>
        PowerSaveStandby = 15,

        /// <summary>
        /// Power Cycle
        /// </summary>
        PowerCycle = 16,

        /// <summary>
        /// The device is in a warning state, but also in a power save mode.
        /// </summary>
        PowerSaveWarning = 17,

        /// <summary>
        /// The device is paused.
        /// </summary>
        Paused = 18,

        /// <summary>
        /// The device is not ready.
        /// </summary>
        NotReady = 19,

        /// <summary>
        /// The device is not configured.
        /// </summary>
        NotConfigured = 20,

        /// <summary>
        /// The device is quiet.
        /// </summary>
        Quiesced = 21
    }
}
