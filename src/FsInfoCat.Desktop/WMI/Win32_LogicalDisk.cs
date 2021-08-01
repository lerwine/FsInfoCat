using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.WMI
{
    public class Win32_LogicalDisk : ILogicalDiskInfo
    {
        public string DisplayName => string.IsNullOrWhiteSpace(Caption) ? Name : Caption;

        public string Name { get; private set; }

        public string VolumeName { get; private set; }

        public string VolumeSerialNumber { get; private set; }

        public bool IsReadOnly => (Access & CIMLogicalDiskAccess.Writeable) == CIMLogicalDiskAccess.Unknown;

        public string Caption { get; private set; }

        public string FileSystem { get; private set; }

        public string ProviderName { get; private set; }

        public string Description { get; private set; }

        public string DeviceID { get; private set; }

        public bool Compressed { get; private set; }

        public ulong FreeSpace { get; private set; }

        public ulong Size { get; private set; }

        public DriveType DriveType { get; private set; }

        public uint MaximumComponentLength { get; private set; }

        public CIMLogicalDiskAccess Access { get; private set; }

        public CIMLogicalDiskAvailability Availability { get; private set; }

        internal Win32_Directory RootDirectory { get; }

        string ILogicalDiskInfo.FileSystemName => FileSystem;

        uint ILogicalDiskInfo.MaxNameLength => MaximumComponentLength;

        internal Win32_LogicalDisk(ManagementObject obj)
        {
            Name = obj.GetString(nameof(Name));
            VolumeName = obj.GetString(nameof(VolumeName));
            VolumeSerialNumber = obj.GetString(nameof(VolumeSerialNumber));
            Caption = obj.GetString(nameof(Caption));
            FileSystem = obj.GetString(nameof(FileSystem));
            ProviderName = obj.GetString(nameof(ProviderName));
            Description = obj.GetString(nameof(Description));
            DeviceID = obj.GetString(nameof(DeviceID));
            Compressed = obj.GetBoolean(nameof(Compressed));
            FreeSpace = obj.GetUInt64(nameof(FreeSpace));
            Size = obj.GetUInt64(nameof(Size));
            DriveType = (DriveType)obj.GetUInt32(nameof(DriveType));
            MaximumComponentLength = obj.GetUInt32(nameof(MaximumComponentLength));
            Access = (CIMLogicalDiskAccess)obj.GetUInt16(nameof(Access));
            Availability = (CIMLogicalDiskAvailability)obj.GetUInt16(nameof(Availability));
            RootDirectory = obj.GetRelated("Win32_LogicalDiskRootDirectory").OfType<ManagementObject>().Select(m => new Win32_Directory(m)).FirstOrDefault();
        }

        internal static async Task<Win32_LogicalDisk> GetLogicalDiskAsync(DirectoryInfo directoryInfo, CancellationToken cancellationToken)
        {
            if (directoryInfo is null)
                return null;
            string fullName = directoryInfo.Root.FullName;
            return (await GetLogicalDisksAsync(cancellationToken)).FirstOrDefault(l =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                Win32_Directory d = l.RootDirectory;
                return d is not null && fullName.Equals(d.Name, StringComparison.InvariantCultureIgnoreCase);
            });
        }

        internal static async Task<Win32_LogicalDisk[]> GetLogicalDisksAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.Run(() => GetLogicalDisks(cancellationToken).ToArray(), cancellationToken);
        }

        internal static IEnumerable<Win32_LogicalDisk> GetLogicalDisks(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ManagementScope namespaceScope = new("\\\\.\\ROOT\\CIMV2");
            ObjectQuery diskQuery = new($"SELECT * FROM {nameof(Win32_LogicalDisk)}");
            ManagementObjectSearcher mgmtObjSearcher = new(namespaceScope, diskQuery);
            foreach (ManagementObject obj in mgmtObjSearcher.Get())
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return new Win32_LogicalDisk(obj);
            }
        }
    }
}
