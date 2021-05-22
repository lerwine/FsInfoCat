namespace FsInfoCat
{
    public interface IComparison : IDbEntity
    {
        IFile SourceFile { get; set; }

        IFile TargetFile { get; set; }
    }
}
