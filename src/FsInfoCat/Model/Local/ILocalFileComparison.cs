namespace FsInfoCat.Model.Local
{
    public interface ILocalFileComparison : IFileComparison
    {
        new ILocalFile File1 { get; }
        new ILocalFile File2 { get; }
    }
}
