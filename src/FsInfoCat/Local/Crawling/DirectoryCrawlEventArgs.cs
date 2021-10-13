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

        [Obsolete("Use constructor without full name")]
        protected DirectoryCrawlEventArgs([DisallowNull] DirectoryInfo target, ILocalSubdirectory entity, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(message, StatusMessageLevel.Information, AsyncJobStatus.Running, concurrencyId)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Entity = entity;
        }

        protected DirectoryCrawlEventArgs([DisallowNull] DirectoryInfo target, ILocalSubdirectory entity, string message, Guid concurrencyId,
            StatusMessageLevel level, AsyncJobStatus status) : base(message, level, status, concurrencyId)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Entity = entity;
        }

        protected DirectoryCrawlEventArgs([DisallowNull] DirectoryInfo target, ILocalSubdirectory entity, string message, [DisallowNull] DirectoryCrawlEventArgs parent,
            StatusMessageLevel level, AsyncJobStatus status) : base(message, level, status, (parent ?? throw new ArgumentNullException(nameof(parent))).ConcurrencyId)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Entity = entity;
            Parent = parent;
        }

        [Obsolete("Use constructor without full name")]
        protected DirectoryCrawlEventArgs([DisallowNull] ILocalSubdirectory entity, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(message, StatusMessageLevel.Information, AsyncJobStatus.Running, concurrencyId)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        }

        protected DirectoryCrawlEventArgs([DisallowNull] ILocalSubdirectory entity, string message, [DisallowNull] DirectoryCrawlEventArgs parent,
            StatusMessageLevel level, AsyncJobStatus status) : base(message, level, status, (parent ?? throw new ArgumentNullException(nameof(parent))).ConcurrencyId)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            Parent = parent;
        }

        protected DirectoryCrawlEventArgs([DisallowNull] DirectoryCrawlEventArgs args, string message, StatusMessageLevel level, AsyncJobStatus status)
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

    public class DirectoryCrawlStartEventArgs : DirectoryCrawlEventArgs
    {
        [Obsolete("Use constructor without full name")]
        public DirectoryCrawlStartEventArgs([DisallowNull] ILocalSubdirectory target, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(target, fullName, message, concurrencyId, level) { }

        public DirectoryCrawlStartEventArgs([DisallowNull] DirectoryInfo target, ILocalSubdirectory entity, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information, AsyncJobStatus status = AsyncJobStatus.Running) : base(target, entity, message, concurrencyId, level, status) { }

        public DirectoryCrawlStartEventArgs([DisallowNull] DirectoryInfo target, ILocalSubdirectory entity, string message, [DisallowNull] DirectoryCrawlEventArgs parent,
            StatusMessageLevel level = StatusMessageLevel.Information, AsyncJobStatus status = AsyncJobStatus.Running) : base(target, entity, message, parent, level, status) { }

        public DirectoryCrawlStartEventArgs([DisallowNull] ILocalSubdirectory entity, string message, [DisallowNull] DirectoryCrawlEventArgs parent,
            StatusMessageLevel level = StatusMessageLevel.Information, AsyncJobStatus status = AsyncJobStatus.Running) : base(entity, message, parent, level, status) { }
    }

    public class DirectoryCrawlEndEventArgs : DirectoryCrawlEventArgs
    {
        [Obsolete("Use constructor without full name")]
        public DirectoryCrawlEndEventArgs([DisallowNull] ILocalSubdirectory target, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(target, fullName, message, concurrencyId, level) { }

        public DirectoryCrawlEndEventArgs([DisallowNull] DirectoryCrawlEventArgs args, string message, StatusMessageLevel level = StatusMessageLevel.Information, AsyncJobStatus status = AsyncJobStatus.Succeeded) : base(args, message, level, status) { }
    }
}
