using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public abstract class FileCrawlEventArgs : CrawlActivityEventArgs, ICrawlManagerFsItemEventArgs
    {
        public ILocalFile Target { get; }

        public string FullName { get; }

        ILocalDbFsItem ICurrentItem.Target { get; }

        protected FileCrawlEventArgs([DisallowNull] ILocalFile target, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(message, StatusMessageLevel.Information, AsyncJobStatus.Running, concurrencyId)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        }

        protected FileCrawlEventArgs([DisallowNull] FileCrawlEventArgs args, string message, StatusMessageLevel level)
            : base(message, level, AsyncJobStatus.Running, (args ?? throw new ArgumentNullException(nameof(args))).ConcurrencyId)
        {
            Target = args.Target;
            FullName = args.FullName;
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
