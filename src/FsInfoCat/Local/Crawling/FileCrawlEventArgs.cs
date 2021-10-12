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

        protected FileCrawlEventArgs([DisallowNull] FileInfo target, [DisallowNull] DirectoryCrawlEventArgs parent, ILocalFile entity, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(message, StatusMessageLevel.Information, AsyncJobStatus.Running, concurrencyId)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Entity = entity;
        }

        protected FileCrawlEventArgs([DisallowNull] ILocalFile target, [DisallowNull] DirectoryCrawlEventArgs parent, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(message, StatusMessageLevel.Information, AsyncJobStatus.Running, concurrencyId)
        {
            Entity = target ?? throw new ArgumentNullException(nameof(target));
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        protected FileCrawlEventArgs([DisallowNull] FileCrawlEventArgs args, string message, StatusMessageLevel level)
            : base(message, level, AsyncJobStatus.Running, (args ?? throw new ArgumentNullException(nameof(args))).ConcurrencyId)
        {
            Entity = args.Entity;
            FullName = args.FullName;
        }

        public string GetFullName()
        {
            throw new NotImplementedException();
        }

        public string GetRelativeParentPath()
        {
            throw new NotImplementedException();
        }
    }

    public class FileCrawlStartEventArgs : FileCrawlEventArgs
    {
        public FileCrawlStartEventArgs([DisallowNull] ILocalFile target, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(target, fullName, message, concurrencyId, level) { }

    }

    public class FileCrawlEndEventArgs : FileCrawlEventArgs
    {
        public FileCrawlEndEventArgs([DisallowNull] ILocalFile target, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(target, fullName, message, concurrencyId, level) { }

        protected FileCrawlEndEventArgs([DisallowNull] FileCrawlEventArgs args, string message, StatusMessageLevel level) : base(args, message, level) { }
    }
}
