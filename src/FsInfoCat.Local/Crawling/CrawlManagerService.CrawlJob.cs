using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FsInfoCat.Local.Crawling
{
    internal partial class CrawlManagerService
    {
        class CrawlJob : ICrawlJob
        {
            private readonly CrawlManagerService _service;
            private readonly ILocalCrawlConfiguration _crawlConfiguration;
            private readonly CancellationTokenSource _tokenSource;
            private bool? _isTimedOut;

            public DateTime? StopAt { get; }
            public long? TTL { get; }

            private async Task<DirectoryCrawlContext> GetRootContext(LocalDbContext dbContext, Subdirectory subdirectory, IFileSystemDetailService fileSystemDetailService, CancellationToken cancellationToken)
            {
                EntityEntry<Subdirectory> entry = dbContext.Entry(subdirectory);
                Subdirectory parent = await entry.GetRelatedReferenceAsync(d => d.Parent, cancellationToken);
                if (parent is null)
                {
                    Volume volume = await entry.GetRelatedReferenceAsync(d => d.Volume, cancellationToken);
                    if (volume is null)
                        throw new InvalidOperationException("Subdirectory has no parent or volume");
                    ILogicalDiskInfo[] logicalDiskInfos = await fileSystemDetailService.GetLogicalDisksAsync(cancellationToken);
                    ILogicalDiskInfo matchingDiskInfo = logicalDiskInfos.FirstOrDefault(d => d.TryGetVolumeIdentifier(out VolumeIdentifier vid) && vid.Equals(volume.Identifier));
                    if (matchingDiskInfo is not null)
                        return new DirectoryCrawlContext()
                        {
                            DbContext = dbContext,
                            EventArgs = new(new DirectoryInfo(matchingDiskInfo.Name), subdirectory, "", ConcurrencyId),
                            ItemNumber = 0,
                            Token = cancellationToken,
                            Depth = 0
                        };
                }
                else
                {
                    DirectoryCrawlContext p = await GetRootContext(dbContext, parent, fileSystemDetailService, cancellationToken);
                    if (p is not null)
                        return new DirectoryCrawlContext()
                        {
                            DbContext = dbContext,
                            EventArgs = new(new DirectoryInfo(Path.Combine(p.EventArgs.GetFullName(), subdirectory.Name)), subdirectory, "", p.EventArgs),
                            ItemNumber = 0,
                            Token = cancellationToken,
                            Depth = 0
                        };
                }
                return null;
            }

            private static Task CrawlAsync(DirectoryCrawlContext crawlContext)
            {
                throw new NotImplementedException();
            }

            internal async Task CrawlAsync()
            {
                using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                Subdirectory subdirectory = await dbContext.Subdirectories.FindAsync(new object[] { _crawlConfiguration.RootId }, _tokenSource.Token);
                if (subdirectory is null)
                    throw new Exception("Could not find crawl root subdirectory record");
                DirectoryCrawlContext rootContext = await GetRootContext(dbContext, subdirectory, serviceScope.ServiceProvider.GetRequiredService<IFileSystemDetailService>(), _tokenSource.Token);
                TimeSpan? ttl = TTL.HasValue ? TimeSpan.FromSeconds(TTL.Value) : null;
                if (StopAt.HasValue)
                {
                    TimeSpan t = StopAt.Value.Subtract(DateTime.Now);
                    if (!(ttl.HasValue && ttl.Value < t))
                        ttl = t;
                }
                if (ttl.HasValue)
                {
                    if (ttl.Value > TimeSpan.Zero)
                    {
                        using Timer timer = new(o =>
                        {
                            if (!_isTimedOut.HasValue)
                                _isTimedOut = true;
                            Cancel(true);
                        }, null, ttl.Value, Timeout.InfiniteTimeSpan);
                        await CrawlAsync(rootContext);
                        return;
                    }
                    _isTimedOut = true;
                    Cancel(true);
                }
                await CrawlAsync(rootContext);
            }

            internal CrawlJob([DisallowNull] CrawlManagerService service, [DisallowNull] ILocalCrawlConfiguration crawlConfiguration, DateTime? stopAt, CancellationTokenSource tokenSource)
            {
                _service = service;
                _crawlConfiguration = crawlConfiguration;
                _tokenSource = tokenSource;
                StopAt = stopAt;
                TTL = _crawlConfiguration.TTL;
            }

            public DateTime Started => throw new NotImplementedException();

            public ICurrentItem CurrentItem { get; private set; }

            public string Title => throw new NotImplementedException();

            public string Message => throw new NotImplementedException();

            public StatusMessageLevel MessageLevel => throw new NotImplementedException();

            public Guid ConcurrencyId => throw new NotImplementedException();

            public AsyncJobStatus JobStatus => throw new NotImplementedException();

            public Task Task => throw new NotImplementedException();

            public bool IsCancellationRequested => throw new NotImplementedException();

            public TimeSpan Duration => throw new NotImplementedException();

            public object AsyncState => throw new NotImplementedException();

            public WaitHandle AsyncWaitHandle => throw new NotImplementedException();

            public bool CompletedSynchronously => throw new NotImplementedException();

            public bool IsCompleted => throw new NotImplementedException();

            public void Cancel(bool throwOnFirstException)
            {
                throw new NotImplementedException();
            }

            public void Cancel()
            {
                throw new NotImplementedException();
            }
        }
    }
}
