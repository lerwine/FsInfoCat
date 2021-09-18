namespace FsInfoCat.Upstream
{
    public interface IHostPlatformListItem : IHostPlatformRow
    {
        string FileSystemDisplayName { get; }

        long HostDeviceCount { get; }
    }
}
