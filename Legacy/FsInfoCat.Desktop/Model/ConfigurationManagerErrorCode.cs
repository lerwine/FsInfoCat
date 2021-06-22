using System.ComponentModel;

namespace FsInfoCat.Desktop
{
    public enum ConfigurationManagerErrorCode
    {
        /// <summary>
        /// Device is working properly.
        /// </summary>
        [Description("This device is working properly.")]
        DeviceWorkingProperly = 0,

        /// <summary>
        /// Device is not configured correctly.
        /// </summary>
        [Description("This device is not configured correctly.")]
        NotConfiguredCorrectly = 1,

        /// <summary>
        /// Windows cannot load the driver for this device.
        /// </summary>
        [Description("Windows cannot load the driver for this device.")]
        CannotLoadDriver = 2,

        /// <summary>
        /// Driver for this device might be corrupted, or the system may be low on memory or other resources.
        /// </summary>
        [Description("The driver for this device might be corrupted, or your system may be running low on memory or other resources.")]
        CorruptOrLowResources = 3,

        /// <summary>
        /// Device is not working properly. One of its drivers or the registry might be corrupted.
        /// </summary>
        [Description("This device is not working properly. One of its drivers or your registry might be corrupted.")]
        NotWorkingProperly = 4,

        /// <summary>
        /// Driver for the device requires a resource that Windows cannot manage.
        /// </summary>
        [Description("The driver for this device needs a resource that Windows cannot manage.")]
        ResourceMissing = 5,

        /// <summary>
        /// Boot configuration for the device conflicts with other devices.
        /// </summary>
        [Description("The boot configuration for this device conflicts with other devices.")]
        DeviceConflict = 6,

        /// <summary>
        /// Cannot filter.
        /// </summary>
        [Description("Cannot filter.")]
        CannotFilter = 7,

        /// <summary>
        /// Driver loader for the device is missing.
        /// </summary>
        [Description("The driver loader for the device is missing.")]
        DriverLoaderMissing = 8,

        /// <summary>
        /// Device is not working properly. The controlling firmware is incorrectly reporting the resources for the device.
        /// </summary>
        [Description("This device is not working properly because the controlling firmware is reporting the resources for the device incorrectly.")]
        ControllerFirmwareFailure = 9,

        /// <summary>
        /// Device cannot start.
        /// </summary>
        [Description("This device cannot start.")]
        CannotStart = 10,

        /// <summary>
        /// Device failed.
        /// </summary>
        [Description("This device failed.")]
        DeviceFailed = 11,

        /// <summary>
        /// Device cannot find enough free resources to use.
        /// </summary>
        [Description("This device cannot find enough free resources that it can use.")]
        InsufficentResources = 12,

        /// <summary>
        /// Windows cannot verify the device resources.
        /// </summary>
        [Description("Windows cannot verify this device's resources.")]
        ResourceVerificationFailure = 13,

        /// <summary>
        /// Device cannot work properly until the computer is restarted.
        /// </summary>
        [Description("This device cannot work properly until you restart your computer.")]
        RestartRequired = 14,

        /// <summary>
        /// Device is not working properly due to a possible re-enumeration problem.
        /// </summary>
        [Description("This device is not working properly because there is probably a re-enumeration problem.")]
        PossibleEnumerationProblem = 15,

        /// <summary>
        /// Windows cannot identify all of the resources that the device uses.
        /// </summary>
        [Description("Windows cannot identify all the resources this device uses.")]
        ResourceDependencyIdentificationFailure = 16,

        /// <summary>
        /// Device is requesting an unknown resource type.
        /// </summary>
        [Description("This device is asking for an unknown resource type.")]
        UnknownResourceType = 17,

        /// <summary>
        /// Device drivers must be reinstalled.
        /// </summary>
        [Description("Reinstall the drivers for this device.")]
        DriverReinstallRequired = 18,

        /// <summary>
        /// Failure using the VxD loader.
        /// </summary>
        [Description("Failure using the VxD loader.")]
        VxDLoaderFailure = 19,

        /// <summary>
        /// Registry might be corrupted.
        /// </summary>
        [Description("Your registry might be corrupted.")]
        RegistryCorrupt = 20,

        /// <summary>
        /// System failure. If changing the device driver is ineffective, see the hardware documentation. Windows is removing the device.
        /// </summary>
        [Description("System failure: Try changing the driver for this device. If that does not work, see your hardware documentation. Windows is removing this device.")]
        DriverSystemFailure = 21,

        /// <summary>
        /// Device is disabled.
        /// </summary>
        [Description("This device is disabled.")]
        DeviceDisabled = 22,

        /// <summary>
        /// System failure. If changing the device driver is ineffective, see the hardware documentation.
        /// </summary>
        [Description("System failure: Try changing the driver for this device.If that doesn't work, see your hardware documentation.")]
        HardwareSystemFailure = 23,

        /// <summary>
        /// Device is not present, not working properly, or does not have all of its drivers installed.
        /// </summary>
        [Description("This device is not present, is not working properly, or does not have all its drivers installed.")]
        DeviceNotPresent = 24,

        /// <summary>
        /// Windows is still setting up the device.
        /// </summary>
        [Description("Windows is still setting up this device.")]
        SetupInProgress = 25,

        /// <summary>
        /// Windows is still setting up the device.
        /// </summary>
        [Description("Windows is still setting up this device.")]
        SetupInProgress2 = 26,

        /// <summary>
        /// Device does not have valid log configuration.
        /// </summary>
        [Description("This device does not have valid log configuration.")]
        NoValidLogConfig = 27,


        /// <summary>
        /// Device drivers are not installed.
        /// </summary>
        [Description("The drivers for this device are not installed.")]
        DriversNotInstalled = 28,

        /// <summary>
        /// Device is disabled. The device firmware did not provide the required resources.
        /// </summary>
        [Description("This device is disabled because the firmware of the device did not give it the required resources.")]
        RequiredResourcesNotProvided = 29,

        /// <summary>
        /// Device is using an IRQ resource that another device is using.
        /// </summary>
        [Description("This device is using an Interrupt Request(IRQ) resource that another device is using.")]
        IrqConflict = 30,

        /// <summary>
        /// Device is not working properly. Windows cannot load the required device drivers.
        /// </summary>
        [Description("This device is not working properly because Windows cannot load the drivers required for this device.")]
        DriverLoadFailure = 31
    }
}
