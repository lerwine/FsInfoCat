namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamVolume : IVolume, IUpstreamTimeStampedEntity
    {
        IHostDevice HostDevice { get; }
        new IUpstreamSubDirectory RootDirectory { get; }
        new IUpstreamFileSystem FileSystem { get; }
    }
}
