namespace FsInfoCat.Desktop.ViewModel
{
    public interface ICrawlConfigListItemViewModel : ICrawlConfigurationRowViewModel
    {
        new Model.ICrawlConfigurationListItem Entity { get; }
    }
}
