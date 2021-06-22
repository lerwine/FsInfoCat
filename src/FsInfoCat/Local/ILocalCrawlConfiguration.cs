namespace FsInfoCat.Local
{
    public interface ILocalCrawlConfiguration : ICrawlConfiguration, ILocalDbEntity
    {
        new ILocalSubdirectory Root { get; }
    }
}
