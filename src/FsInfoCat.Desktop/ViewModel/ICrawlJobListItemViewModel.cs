namespace FsInfoCat.Desktop.ViewModel
{
    public interface ICrawlJobListItemViewModel : ICrawlJobRowViewModel
    {
        new ICrawlJobListItem Entity { get; }
    }
}
