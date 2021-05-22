namespace FsInfoCat.Upstream
{
    public interface IUpstreamVolume : IVolume, IUpstreamDbEntity
    {
        IHostDevice HostDevice { get; }

        new IUpstreamFileSystem FileSystem { get; set; }
    }

}
