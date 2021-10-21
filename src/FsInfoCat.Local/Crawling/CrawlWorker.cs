using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    class CrawlWorker
    {
        private readonly ILogger<CrawlWorker> _logger;
        private readonly ICrawlMessageBus _crawlMessageBus;
        private readonly IFileSystemDetailService _fileSystemDetailService;

        public DateTime? StopAt { get; }

        public long? TTL { get; }

        public ICurrentItem CurrentItem { get; internal set; }

        internal CrawlWorker([DisallowNull] ILocalCrawlConfiguration crawlConfiguration, [DisallowNull] ICrawlMessageBus crawlMessageBus, [DisallowNull] IFileSystemDetailService fileSystemDetailService, DateTime? stopAt)
        {
            (_logger, _crawlMessageBus, _fileSystemDetailService) = (Hosting.GetRequiredService<ILogger<CrawlWorker>>(), crawlMessageBus, fileSystemDetailService);
            _logger.LogDebug($"{nameof(CrawlWorker)} instantiated");
        }

        // TODO: Implement CrawlWorker.DoWorkAsync #104
        internal async Task<bool?> DoWorkAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        record CurrentFile : ICurrentItem
        {
            internal FileInfo Target { get; init; }

            internal DbFile Entity { get; init; }

            internal CurrentDirectory Parent { get; init; }

            string ICurrentItem.Name => Target.Name ?? Entity.Name;

            FileSystemInfo ICurrentItem.Target => Target;

            ILocalDbFsItem ICurrentItem.Entity => Entity;

            string ICurrentItem.GetFullName() => Target?.FullName ?? Path.Combine(Parent.GetFullName(), Target.Name ?? Entity.Name);

            string ICurrentItem.GetRelativeParentPath()
            {
                string path = Parent.GetRelativeParentPath();
                return string.IsNullOrEmpty(path) ? Target.Name ?? Entity.Name : Path.Combine(path, Target.Name ?? Entity.Name);
            }
        }

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
