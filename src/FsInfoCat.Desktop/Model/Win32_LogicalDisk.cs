using System;
using System.Collections.Generic;
using System.IO;
using System.Management;

namespace FsInfoCat.Desktop.Model
{
    /// <summary>
    /// Wraps the Win32_LogicalDisk WMI class which represents a data source that resolves to an actual local storage device on a computer system running Windows.
    /// </summary>
    /// <remarks>References
    /// <list type="bullet">
    ///     <item><term>White, S. (2021, January 06)</term>
    ///         <description>Win32_LogicalDisk class - Win32 apps
    ///         <para>Retrieved from Microsoft Docs: https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-logicaldisk</para></description>
    ///     </item>
    /// </list></remarks>
    public class Win32_LogicalDisk
    {
        private readonly ManagementObject _managementObject;

        /// <summary>
        /// Type of media access available.
        /// </summary>
        public Win32_MediaAccess Access => _managementObject.ToEnumPropertyValue<Win32_MediaAccess>(nameof(Access)).Value;

        /// <summary>
        /// Availability and status of the device.
        /// </summary>
        public Win32_DeviceAvailability? Availability => _managementObject.ToEnumPropertyValue<Win32_DeviceAvailability>(nameof(Availability));

        /// <summary>
        /// Size, in bytes, of the blocks that form this storage extent.
        /// </summary>
        public ulong? BlockSize => (ulong?)_managementObject[nameof(BlockSize)];

        /// <summary>
        /// Short description of the object a one-line string.
        /// </summary>
        public string Caption => (string)_managementObject[nameof(Caption)];

        /// <summary>
        /// If <see langword="true"/>, the logical volume exists as a single compressed entity, such as a DoubleSpace volume.
        /// </summary>
        /// <remarks>If file based compression is supported, such as on <c>NTFS</c>, this property is <see langword="false"/>.</remarks>
        public bool Compressed => (bool)_managementObject[nameof(Compressed)];

        /// <summary>
        /// Windows Configuration Manager error code.
        /// </summary>
        public ConfigurationManagerErrorCode? ConfigManagerErrorCode => _managementObject.ToEnumPropertyValue<ConfigurationManagerErrorCode>(nameof(ConfigManagerErrorCode));

        /// <summary>
        /// If <see langword="true"/>, the device is using a user-defined configuration.
        /// </summary>
        public bool? ConfigManagerUserConfig => (bool?)_managementObject[nameof(ConfigManagerUserConfig)];

        /// <summary>
        /// Name of the first concrete class to appear in the inheritance chain used in the creation of an instance.
        /// </summary>
        public string CreationClassName => (string)_managementObject[nameof(CreationClassName)];

        /// <summary>
        /// Description of the object.
        /// </summary>
        public string Description => (string)_managementObject[nameof(Description)];

        /// <summary>
        /// Unique identifier of the logical disk from other devices on the system.
        /// </summary>
        public string DeviceID => (string)_managementObject[nameof(DeviceID)];

        /// <summary>
        /// Numeric value that corresponds to the type of disk drive this logical disk represents.
        /// </summary>
        public DriveType DriveType => _managementObject.ToEnumPropertyValue<DriveType>(nameof(DriveType)).Value;

        /// <summary>
        /// If <see langword="true"/>, the error reported in <seealso cref="LastErrorCode"/> is now cleared.
        /// </summary>
        public bool? ErrorCleared => (bool?)_managementObject[nameof(ErrorCleared)];

        /// <summary>
        /// More information about the error recorded in <seealso cref="LastErrorCode"/>, and information on any corrective actions that may be taken.
        /// </summary>
        public string ErrorDescription => (string)_managementObject[nameof(ErrorDescription)];

        /// <summary>
        /// Type of error detection and correction supported by this storage extent.
        /// </summary>
        public string ErrorMethodology => (string)_managementObject[nameof(ErrorMethodology)];

        /// <summary>
        /// File system on the logical disk.
        /// </summary>
        public string FileSystem => (string)_managementObject[nameof(FileSystem)];

        /// <summary>
        /// Space, in bytes, available on the logical disk.
        /// </summary>
        public ulong FreeSpace => (ulong)_managementObject[nameof(FreeSpace)];

        /// <summary>
        /// Date and time the object was installed. This property does not require a value to indicate that the object is installed.
        /// </summary>
        public Win32_LocalTime InstallDate => _managementObject.ToWin32LocalTime(nameof(InstallDate));

        /// <summary>
        /// Last error code reported by the logical device.
        /// </summary>
        public uint? LastErrorCode => (uint?)_managementObject[nameof(LastErrorCode)];

        /// <summary>
        /// Maximum length of a filename component supported by the Windows drive.
        /// </summary>
        /// <remarks>A filename component is that portion of a filename between backslashes. The value can be used to indicate that long names are supported by the specified file system.
        /// For example, for a FAT file system supporting long names, the function stores the value 255, rather than the previous 8.3 indicator.
        /// Long names can also be supported on systems that use the NTFS file system.</remarks>
        public uint MaximumComponentLength => (uint)_managementObject[nameof(MaximumComponentLength)];

        /// <summary>
        /// Type of media currently present in the logical drive.
        /// </summary>
        public WIN32_MEDIA_TYPE MediaType => _managementObject.ToEnumPropertyValue< WIN32_MEDIA_TYPE>(nameof(MediaType)).Value;

        /// <summary>
        /// Label by which the object is known. When subclassed, this property can be overridden to be a key property.
        /// </summary>
        public string Name => (string)_managementObject[nameof(Name)];

        /// <summary>
        /// Total number of consecutive blocks, each block the size of the value contained in the <seealso cref="BlockSize"/> property, which form this storage extent.
        /// </summary>
        /// <remarks>Total size of the storage extent can be calculated by multiplying the value of the <seealso cref="BlockSize"/> property by the value of this property.
        /// If the value of <seealso cref="BlockSize"/> is 1, this property is the total size of the storage extent.</remarks>
        public ulong? NumberOfBlocks => (ulong?)_managementObject[nameof(NumberOfBlocks)];

        /// <summary>
        /// Windows Plug and Play device identifier of the logical device.
        /// </summary>
        public string PNPDeviceID => (string)_managementObject[nameof(PNPDeviceID)];

        /// <summary>
        /// Array of the specific power-related capabilities of a logical device.
        /// </summary>
        public IEnumerable<CIM_PowerManagementCapability> PowerManagementCapabilities => _managementObject.ToEnumPropertyValues<CIM_PowerManagementCapability>(nameof(PowerManagementCapabilities));

        /// <summary>
        /// If <see langword="true"/>, the device can be power-managed (can be put into suspend mode, and so on).
        /// </summary>
        /// <remarks>This property does not indicate that power management features are currently enabled, only that the logical device is capable of power management.</remarks>
        public bool? PowerManagementSupported => (bool?)_managementObject[nameof(PowerManagementSupported)];

        /// <summary>
        /// Network path to the logical device.
        /// </summary>
        public string ProviderName => (string)_managementObject[nameof(ProviderName)];

        /// <summary>
        /// Free-form string describing the media and its use.
        /// </summary>
        public string Purpose => (string)_managementObject[nameof(Purpose)];

        /// <summary>
        /// Indicates that quota management is not enabled on this system.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if quota management is not enabled; otherwise, <see langword="false"/>.
        /// </value>
        public bool? QuotasDisabled => (bool?)_managementObject[nameof(QuotasDisabled)];

        /// <summary>
        /// Indicates that the quota management was used but has been disabled.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if quota management was used but has been disabled; otherwise, <see langword="false"/>.
        /// </value>
        /// <remarks>Incomplete refers to the information left in the file system after quota management was disabled.</remarks>
        public bool? QuotasIncomplete => (bool?)_managementObject[nameof(QuotasIncomplete)];

        /// <summary>
        /// Indicates whether the file system is in the active process of compiling information and setting the disk up for quota management.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the file system is in the active process of compiling information and setting the disk up for quota management; otherwise, <see langword="false"/>.
        /// </value>
        public bool? QuotasRebuilding => (bool?)_managementObject[nameof(QuotasRebuilding)];

        /// <summary>
        /// Size of the disk drive.
        /// </summary>
        public ulong Size => (ulong)_managementObject[nameof(Size)];

        /// <summary>
        /// Current status of the object.
        /// </summary>
        /// <value>
        /// Includes the following values:
        /// <list type="bullet">
        /// <item><c>"OK"</c></item>
        /// <item><c>"Error"</c></item>
        /// <item><c>"Degraded"</c></item>
        /// <item><c>"Unknown"</c></item>
        /// <item><c>"Pred Fail"</c></item>
        /// <item><c>"Starting"</c></item>
        /// <item><c>"Stopping"</c></item>
        /// <item><c>"Service"</c></item>
        /// <item><c>"Stressed"</c></item>
        /// <item><c>"NonRecover"</c></item>
        /// <item><c>"No Contact"</c></item>
        /// <item><c>"Lost Comm"</c></item>
        /// </list>
        /// </value>
        public string Status => (string)_managementObject[nameof(Status)];

        /// <summary>
        /// State of the logical device.
        /// </summary>
        public DeviceOperationalState? StatusInfo => _managementObject.ToEnumPropertyValue<DeviceOperationalState>(nameof(StatusInfo));

        /// <summary>
        /// Indictes whether this volume supports disk quotas.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this volume supports disk quotas; otherwise, <see langword="false"/>.
        /// </value>
        public bool SupportsDiskQuotas => (bool)_managementObject[nameof(SupportsDiskQuotas)];

        /// <summary>
        /// Indicates whether the logical disk partition supports file-based compression, such as is the case with the NTFS file system.
        /// </summary>
        /// <see langword="true"/> if this volume supports file-based compression; otherwise, <see langword="false"/>.
        /// <remarks>This property is <see langword="false"/> when the <seealso cref="Compressed"/> property is <see langword="true"/>.</remarks>
        public bool SupportsFileBasedCompression => (bool)_managementObject[nameof(SupportsFileBasedCompression)];

        /// <summary>
        /// Value of the scoping computer <code>CreationClassName</code> property.
        /// </summary>
        public string SystemCreationClassName => (string)_managementObject[nameof(SystemCreationClassName)];

        /// <summary>
        /// Name of the scoping system.
        /// </summary>
        public string SystemName => (string)_managementObject[nameof(SystemName)];

        /// <summary>
        /// Indicates whether the disk requires ChkDsk to be run at the next restart.
        /// </summary>
        /// <see langword="true"/> if the disk requires ChkDsk to be run at the next restart; otherwise, <see langword="false"/>.
        /// <remarks>This property is only applicable to those instances of logical disk that represent a physical disk in the machine. It is not applicable to mapped logical drives.</remarks>
        public bool? VolumeDirty => (bool?)_managementObject[nameof(VolumeDirty)];

        /// <summary>
        /// Volume name of the logical disk.
        /// </summary>
        public string VolumeName => (string)_managementObject[nameof(VolumeName)];

        /// <summary>
        /// Volume serial number of the logical disk.
        /// </summary>
        public string VolumeSerialNumber => (string)_managementObject[nameof(VolumeSerialNumber)];

        public Win32_LogicalDisk(ManagementObject managementObject)
        {
            _managementObject = managementObject ?? throw new ArgumentNullException(nameof(managementObject));
        }
    }
}
