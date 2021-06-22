using System.ComponentModel;

namespace FsInfoCat.Desktop
{
    public enum CIM_PowerManagementCapability
    {
        [Description("Unknown")]
        Unknown = 0,


        [Description("Not Supported")]
        NotSupported = 1,


        [Description("Disabled")]
        Disabled = 2,

        /// <summary>
        /// The power management features are currently enabled but the exact feature set is unknown or the information is unavailable.
        /// </summary>
        [Description("Enabled")]
        Enabled = 3,

        /// <summary>
        /// The device can change its power state based on usage or other criteria.
        /// </summary>
        [Description("Power Saving Modes Entered Automatically")]
        EnteredAutomatically = 4,

        /// <summary>
        /// The SetPowerState method is supported. This method is found on the parent CIM_LogicalDevice class and can be implemented.For more information, see Designing Managed Object Format(MOF) Classes.
        /// </summary>
        [Description("Power State Settable")]
        PowerStateSettable = 5,

        /// <summary>
        /// The SetPowerState method can be invoked with the PowerState parameter set to 5 (Power Cycle).
        /// </summary>
        [Description("Power Cycling Supported")]
        PowerCyclingSupported = 6,

        /// <summary>
        /// The SetPowerState method can be invoked with the PowerState parameter set to 5 (Power Cycle) and Time set to a specific date and time, or interval, for power-on.
        /// </summary>
        [Description("Timed Power-On Supported")]
        TimedPowerOnSupported = 7
    }
}
