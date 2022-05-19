using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    // TODO: Document IHostPlatformListItem interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IHostPlatformListItem : IHostPlatformRow
    {
        string FileSystemDisplayName { get; }

        long HostDeviceCount { get; }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
