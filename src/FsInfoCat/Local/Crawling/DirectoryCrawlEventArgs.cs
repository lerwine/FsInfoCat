using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public abstract class DirectoryCrawlEventArgs : CrawlActivityEventArgs, ICrawlManagerFsItemEventArgs
    {
        public ILocalSubdirectory Target { get; }

        public string FullName { get; }

        ILocalDbFsItem ICurrentItem.Target { get; }

        protected DirectoryCrawlEventArgs([DisallowNull] ILocalSubdirectory target, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(message, StatusMessageLevel.Information, AsyncJobStatus.Running, concurrencyId)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        }

        protected DirectoryCrawlEventArgs([DisallowNull] DirectoryCrawlEventArgs args, string message, StatusMessageLevel level)
            : base(message, level, AsyncJobStatus.Running, (args ?? throw new ArgumentNullException(nameof(args))).ConcurrencyId)
        {
            Target = args.Target;
            FullName = args.FullName;
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
