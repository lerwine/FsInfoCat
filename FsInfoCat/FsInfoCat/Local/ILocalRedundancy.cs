namespace FsInfoCat.Local
{
    public interface ILocalRedundancy : IRedundancy, ILocalDbEntity
    {
        new ILocalFile File { get; set; }
        new ILocalRedundantSet RedundantSet { get; set; }
    }
}
