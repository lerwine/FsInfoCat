using System.IO;

namespace FsInfoCat.Local.Crawling
{
    public interface ICurrentItem
    {
        string Name { get; }

        [System.Obsolete("Use GetFullName() or GetRelativeParentPath()")]
        string FullName { get; }

        FileSystemInfo Target { get; }

        ILocalDbFsItem Entity { get; }

        string GetFullName();

        string GetRelativeParentPath();
    }
}
