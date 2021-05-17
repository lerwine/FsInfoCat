namespace FsInfoCat.Model.Local
{
    public interface ILocalVolume : IVolume
    {
        new ILocalSubDirectory RootDirectory { get; }
        new ILocalFileSystem FileSystem { get; }
    }
}
