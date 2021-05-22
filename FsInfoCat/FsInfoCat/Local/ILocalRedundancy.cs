namespace FsInfoCat.Local
{
    public interface ILocalRedundancy : IRedundancy, ILocalDbEntity
    {
        new ILocalRedundantSet RedundantSet { get; set; }
    }
}
