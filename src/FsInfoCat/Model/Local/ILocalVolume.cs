namespace FsInfoCat.Model.Local
{
    public interface ILocalVolume : IVolume, ILocalModel
    {
        new ILocalSubDirectory RootDirectory { get; }
        new ILocalFileSystem FileSystem { get; }
    }
}
