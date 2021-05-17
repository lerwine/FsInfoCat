namespace FsInfoCat.Model.Remote
{
    public interface IRemoteVolume : IVolume, IRemoteTimeStampedEntity
    {
        new IRemoteSubDirectory RootDirectory { get; }
        new IRemoteFileSystem FileSystem { get; }
    }
}
