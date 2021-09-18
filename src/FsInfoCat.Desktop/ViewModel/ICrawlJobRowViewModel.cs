namespace FsInfoCat.Desktop.ViewModel
{
    public interface ICrawlJobRowViewModel : IDbEntityRowViewModel
    {
        new ICrawlJobLogRow Entity { get; }
    }
}
