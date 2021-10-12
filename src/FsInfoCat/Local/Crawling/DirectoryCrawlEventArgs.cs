using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FsInfoCat.Local.Crawling
{
    public abstract class DirectoryCrawlEventArgs : CrawlActivityEventArgs, ICrawlManagerFsItemEventArgs
    {
        public ILocalSubdirectory Entity { get; }

        public DirectoryInfo Target { get; }

        public string Name => Target?.Name ?? Entity?.Name ?? "";

        [Obsolete("Use GetFullName() or GetRelativeParentPath()")]
        public string FullName { get; }

        ILocalDbFsItem ICurrentItem.Entity { get; }

        FileSystemInfo ICurrentItem.Target => Target;

        public DirectoryCrawlEventArgs Parent { get; }

        protected DirectoryCrawlEventArgs([DisallowNull] DirectoryInfo target, ILocalSubdirectory entity, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(message, StatusMessageLevel.Information, AsyncJobStatus.Running, concurrencyId)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Entity = entity;
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        }

        protected DirectoryCrawlEventArgs([DisallowNull] ILocalSubdirectory entity, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(message, StatusMessageLevel.Information, AsyncJobStatus.Running, concurrencyId)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        }

        protected DirectoryCrawlEventArgs([DisallowNull] DirectoryCrawlEventArgs args, string message, StatusMessageLevel level)
            : base(message, level, AsyncJobStatus.Running, (args ?? throw new ArgumentNullException(nameof(args))).ConcurrencyId)
        {
            Entity = args.Entity;
            FullName = args.FullName;
        }

        public string GetFullName()
        {
            if (Target is null)
            {
                string path = Parent?.GetFullName();
                if (string.IsNullOrEmpty(path))
                    return Target?.Name ?? Entity?.Name.NullIfEmpty();
                string name = Target?.Name ?? Entity?.Name;
                return string.IsNullOrEmpty(name) ? path : Path.Combine(path, name);
            }
            return Target.FullName;
        }

        public string GetRelativeParentPath()
        {
            if (Parent is null)
                return "";
            string path = Parent.GetRelativeParentPath();
            if (string.IsNullOrEmpty(path))
                return Target?.Name ?? Entity?.Name.NullIfEmpty();
            string name = Target?.Name ?? Entity?.Name;
            return string.IsNullOrEmpty(name) ? path : Path.Combine(path, name);
        }
    }

    public class DirectoryCrawlStartEventArgs : DirectoryCrawlEventArgs
    {
        public DirectoryCrawlStartEventArgs([DisallowNull] ILocalSubdirectory target, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(target, fullName, message, concurrencyId, level) { }

    }

    public class DirectoryCrawlEndEventArgs : DirectoryCrawlEventArgs
    {
        public DirectoryCrawlEndEventArgs([DisallowNull] ILocalSubdirectory target, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(target, fullName, message, concurrencyId, level) { }

        protected DirectoryCrawlEndEventArgs([DisallowNull] DirectoryCrawlEventArgs args, string message, StatusMessageLevel level) : base(args, message, level) { }
    }
}
