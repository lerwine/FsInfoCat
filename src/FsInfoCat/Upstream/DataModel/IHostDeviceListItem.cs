namespace FsInfoCat.Upstream
{
    public interface IHostDeviceListItem : IHostDeviceRow
    {
        string PlatformDisplayName { get; }

        PlatformType PlatformType { get; }

        long VolumeCount { get; }
    }
}
