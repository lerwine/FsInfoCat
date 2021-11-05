using System.IO;

namespace FsInfoCat.Local.Crawling
{
    public interface ICurrentFile : ICurrentItem
    {
        new FileInfo Target { get; }

        new DbFile Entity { get; }
    }
}
