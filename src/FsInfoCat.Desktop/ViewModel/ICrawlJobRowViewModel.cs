namespace FsInfoCat.Desktop.ViewModel
{
    public interface ICrawlJobRowViewModel : IDbEntityRowViewModel
    {
        new Model.ICrawlJobLogRow Entity { get; }
    }
}
