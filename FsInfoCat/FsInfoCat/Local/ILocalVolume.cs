namespace FsInfoCat.Local
{
    public interface ILocalVolume : IVolume, ILocalDbEntity
    {
        new ILocalFileSystem FileSystem { get; set; }
    }
}
