using System.IO;

namespace FsInfoCat.Desktop
{
    public record FileSystemRecord : IFileSystemProperties
    {
        public string DisplayName { get; init; }
        public bool ReadOnly { get; init; }
        public uint MaxNameLength { get; init; }
        public DriveType? DefaultDriveType { get; init; }
    }
}
