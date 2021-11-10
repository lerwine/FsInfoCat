using System.IO;

namespace FsInfoCat.Local.Crawling
{
    public interface ICurrentItem
    {
        string Name { get; }

        FileSystemInfo Target { get; }

        ILocalDbFsItem Entity { get; }

        ICurrentDirectory Parent { get; }

        string GetFullName();

        string GetRelativeParentPath();
    }
}
