namespace FsInfoCat.Desktop.ViewModel
{
    public interface ICrawlConfigDetailsViewModel : ICrawlConfigurationRowViewModel
    {
        new ICrawlConfiguration Entity { get; }
    }
}
