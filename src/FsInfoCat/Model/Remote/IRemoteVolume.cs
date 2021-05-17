namespace FsInfoCat.Model.Remote
{
    public interface IRemoteVolume : IVolume, IRemoteTimeStampedEntity
    {
        IHostDevice HostDevice { get; }
        new IRemoteSubDirectory RootDirectory { get; }
        new IRemoteFileSystem FileSystem { get; }
    }
}
