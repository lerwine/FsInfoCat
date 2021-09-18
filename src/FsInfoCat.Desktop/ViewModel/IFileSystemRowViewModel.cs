namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFileSystemRowViewModel : IDbEntityRowViewModel
    {
        new IFileSystemRow Entity { get; }
    }
}
