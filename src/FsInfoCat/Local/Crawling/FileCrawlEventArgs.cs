using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FsInfoCat.Local.Crawling
{
    public abstract class FileCrawlEventArgs : CrawlActivityEventArgs, ICrawlManagerFsItemEventArgs
    {
        public ILocalFile Entity { get; }

        [System.Obsolete("Use GetFullName() or GetRelativeParentPath()")]
        public string FullName { get; }

        public FileInfo Target { get; }

        ILocalDbFsItem ICurrentItem.Entity { get; }

        public DirectoryCrawlEventArgs Parent { get; }

        public string Name => Target?.Name ?? Entity?.Name ?? "";

        FileSystemInfo ICurrentItem.Target => Target;

        [Obsolete("Use constructor without full name")]
        protected FileCrawlEventArgs([DisallowNull] FileInfo target, ILocalFile entity, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(message, StatusMessageLevel.Information, AsyncJobStatus.Running, concurrencyId)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Entity = entity;
        }

        protected FileCrawlEventArgs([DisallowNull] FileInfo target, ILocalFile entity, [DisallowNull] DirectoryCrawlEventArgs parent, string message,
            StatusMessageLevel level, AsyncJobStatus status) : base(message, level, status, (parent ?? throw new ArgumentNullException(nameof(parent))).ConcurrencyId)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Entity = entity;
        }

        [Obsolete("Use constructor without full name")]
        protected FileCrawlEventArgs([DisallowNull] ILocalFile target,  [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(message, StatusMessageLevel.Information, AsyncJobStatus.Running, concurrencyId)
        {
            Entity = target ?? throw new ArgumentNullException(nameof(target));
        }

        protected FileCrawlEventArgs([DisallowNull] ILocalFile entity, [DisallowNull] DirectoryCrawlEventArgs parent, string message,
            StatusMessageLevel level, AsyncJobStatus status) : base(message, level, status, (parent ?? throw new ArgumentNullException(nameof(parent))).ConcurrencyId)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        protected FileCrawlEventArgs([DisallowNull] FileCrawlEventArgs args, string message, StatusMessageLevel level, AsyncJobStatus status)
            : base(message, level, status, (args ?? throw new ArgumentNullException(nameof(args))).ConcurrencyId)
        {
            Entity = args.Entity;
            Target = args.Target;
            Parent = args.Parent;
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

    public class FileCrawlStartEventArgs : FileCrawlEventArgs
    {
        [Obsolete("Use constructor without full name")]
        public FileCrawlStartEventArgs([DisallowNull] ILocalFile entity, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(entity, fullName, message, concurrencyId, level) { }

        public FileCrawlStartEventArgs([DisallowNull] FileInfo target, ILocalFile entity, [DisallowNull] DirectoryCrawlEventArgs parent, string message,
            StatusMessageLevel level = StatusMessageLevel.Information, AsyncJobStatus status = AsyncJobStatus.Running) : base(target, entity, parent, message, level, status) { }

        public FileCrawlStartEventArgs([DisallowNull] ILocalFile entity, [DisallowNull] DirectoryCrawlEventArgs parent, string message, StatusMessageLevel level = StatusMessageLevel.Information, AsyncJobStatus status = AsyncJobStatus.Running)
            : base(entity, parent, message, level, status) { }
    }

    public class FileCrawlEndEventArgs : FileCrawlEventArgs
    {
        [Obsolete("Use constructor without full name")]
        public FileCrawlEndEventArgs([DisallowNull] ILocalFile target, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(target, fullName, message, concurrencyId, level) { }

        public FileCrawlEndEventArgs([DisallowNull] FileCrawlEventArgs args, string message, StatusMessageLevel level = StatusMessageLevel.Information, AsyncJobStatus status = AsyncJobStatus.Succeeded) : base(args, message, level, status) { }
    }
}
