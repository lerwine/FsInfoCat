namespace FsInfoCat.Local
{
    public interface ILocalComparison : IComparison, ILocalDbEntity
    {
        new ILocalFile SourceFile { get; set; }

        new ILocalFile TargetFile { get; set; }
    }
}
