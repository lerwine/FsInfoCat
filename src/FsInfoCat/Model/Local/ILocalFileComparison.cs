namespace FsInfoCat.Model.Local
{
    public interface ILocalFileComparison : IFileComparison, ILocalModel
    {
        new ILocalFile SourceFile { get; }

        new ILocalFile TargetFile { get; }
    }
}
