using System.IO;

namespace FsInfoCat.Local.Crawling
{
    public interface ICurrentItem
    {
        string Name { get; }

        FileSystemInfo Target { get; }

        ILocalDbFsItem Entity { get; }

        string GetFullName();

        string GetRelativeParentPath();
    }
}
