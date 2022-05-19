namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFileSystemRowViewModel : IDbEntityRowViewModel
    {
        new Model.IFileSystemRow Entity { get; }
    }
}
