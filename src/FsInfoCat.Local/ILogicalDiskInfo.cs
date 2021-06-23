using System.IO;

namespace FsInfoCat.Local
{
    public interface ILogicalDiskInfo
    {
        bool ReadOnly { get; }

        string DisplayName { get; }

        bool Compressed { get; }

        DriveType DriveType { get; }

        string FileSystemName { get; }

        uint MaxNameLength { get; }

        string Name { get; }

        string ProviderName { get; }

        string VolumeName { get; }

        string VolumeSerialNumber { get; }
    }
}
