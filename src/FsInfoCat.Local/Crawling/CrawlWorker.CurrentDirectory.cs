using System.IO;

namespace FsInfoCat.Local.Crawling
{
    partial class CrawlWorker
    {
        record CurrentDirectory : ICurrentItem
        {
            internal DirectoryInfo Target { get; init; }

            internal Subdirectory Entity { get; init; }

            internal CurrentDirectory Parent { get; init; }

            string ICurrentItem.Name => Target.Name ?? Entity.Name;

            FileSystemInfo ICurrentItem.Target => Target;

            ILocalDbFsItem ICurrentItem.Entity => Entity;

            public string GetFullName()
            {
                if (Target is null)
                {
                    if (Parent is null)
                        return Entity.Name;
                    return Path.Combine(Parent.GetFullName(), Entity.Name);
                }
                return Target.FullName;
            }

            public string GetRelativeParentPath()
            {
                if (Parent is null)
                    return "";
                string path = Parent.GetRelativeParentPath();
                return (path.Length > 0) ? Path.Combine(path, Target.Name ?? Entity.Name) : Target.Name ?? Entity.Name;
            }
        }
    }
}
