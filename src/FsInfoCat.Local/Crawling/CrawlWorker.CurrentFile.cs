using System.IO;

namespace FsInfoCat.Local.Crawling
{
    partial class CrawlWorker
    {
        record CurrentFile : ICurrentItem
        {
            internal FileInfo Target { get; init; }

            internal DbFile Entity { get; init; }

            internal CurrentDirectory Parent { get; init; }

            string ICurrentItem.Name => Target.Name ?? Entity.Name;

            FileSystemInfo ICurrentItem.Target => Target;

            ILocalDbFsItem ICurrentItem.Entity => Entity;

            string ICurrentItem.GetFullName() => Target?.FullName ?? Path.Combine(Parent.GetFullName(), Target?.Name ?? Entity.Name);

            string ICurrentItem.GetRelativeParentPath()
            {
                string path = Parent.GetRelativeParentPath();
                return string.IsNullOrEmpty(path) ? Target?.Name ?? Entity.Name : Path.Combine(path, Target?.Name ?? Entity.Name);
            }
        }
    }
}
