using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;

namespace FsInfoCat.Desktop.WMI
{
    public class CIM_LogicalDisk
    {
        public string Name { get; private set; }

        public string VolumeName { get; private set; }

        public string VolumeSerialNumber { get; private set; }

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
        public CIM_LogicalDisk(ManagementObject obj)
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
        }

        internal static IEnumerable<CIM_LogicalDisk> GetLogicalDisks(DirectoryInfo directoryInfo)
        {
            ManagementScope namespaceScope = new ManagementScope("\\\\.\\ROOT\\CIMV2");
            ObjectQuery diskQuery = new ObjectQuery("SELECT * FROM CIM_LogicalDisk");
            ManagementObjectSearcher mgmtObjSearcher = new ManagementObjectSearcher(namespaceScope, diskQuery);
                foreach (ManagementObject obj in mgmtObjSearcher.Get())
                    yield return new CIM_LogicalDisk(obj);
        }
    }
}
