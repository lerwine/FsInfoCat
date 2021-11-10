using System.IO;

namespace FsInfoCat.Local.Crawling
{
    public interface ICurrentDirectory : ICurrentItem
    {
        new DirectoryInfo Target { get; }

        new Subdirectory Entity { get; }
    }
}
