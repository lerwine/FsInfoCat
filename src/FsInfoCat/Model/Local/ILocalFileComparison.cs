namespace FsInfoCat.Model.Local
{
    public interface ILocalFileComparison : IFileComparison, ILocalModel
    {
        new ILocalFile File1 { get; }
        new ILocalFile File2 { get; }
    }
}
