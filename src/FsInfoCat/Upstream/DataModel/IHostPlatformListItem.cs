namespace FsInfoCat.Upstream
{
    // TODO: Document IHostPlatformListItem interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IHostPlatformListItem")]
    public interface IHostPlatformListItem : IHostPlatformRow
    {
        string FileSystemDisplayName { get; }

        long HostDeviceCount { get; }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
