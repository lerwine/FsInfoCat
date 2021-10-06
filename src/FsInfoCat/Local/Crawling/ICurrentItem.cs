namespace FsInfoCat.Local.Crawling
{
    public interface ICurrentItem
    {
        string FullName { get; }

        ILocalDbFsItem Target { get; }
    }
}
