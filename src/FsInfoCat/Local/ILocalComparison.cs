namespace FsInfoCat.Local
{
    public interface ILocalComparison : IComparison, ILocalDbEntity
    {
        new ILocalFile Baseline { get; set; }

        new ILocalFile Correlative { get; set; }
    }
}
