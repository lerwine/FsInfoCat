using FsInfoCat.AsyncOps;
using FsInfoCat.Background;
using FsInfoCat.Local.Background;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    // TODO: Implement CrawlJob service #104
    public partial class CrawlJob : ICrawlJob
    {
        private readonly ILogger<CrawlJob> _logger;
        private readonly ICrawlMessageBus _crawlMessageBus;
        private readonly IFileSystemDetailService _fileSystemDetailService;
        //private readonly IJobResult<CrawlTerminationReason> _workItem;
        private CrawlTerminationReason _terminationReason;

        public DateTime? StopAt { get; }

        public long? TTL { get; }

        public DateTime Started { get; private set; }

        public AsyncJobStatus JobStatus { get; private set; }

        public ICurrentItem CurrentItem { get; private set; }

        object IAsyncResult.AsyncState => throw new NotImplementedException();

        WaitHandle IAsyncResult.AsyncWaitHandle => throw new NotImplementedException();

        bool IAsyncResult.CompletedSynchronously => throw new NotImplementedException();

        bool IAsyncResult.IsCompleted => throw new NotImplementedException();

        public CrawlJob(ILogger<CrawlJob> logger, ICrawlMessageBus crawlMessageBus, ICrawlQueue crawlQueueService, IFileSystemDetailService fileSystemDetailService)
        {
            (_logger, _crawlMessageBus, _fileSystemDetailService) = (logger, crawlMessageBus, fileSystemDetailService);
            _logger.LogDebug($"{nameof(CrawlJob)} Service instantiated");
        }

        [ServiceBuilderHandler(Priority = 200)]
        public static void ConfigureServices(IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(CrawlJob).FullName}.{nameof(ConfigureServices)}");
            services.AddTransient<CrawlJob>();
            //services.AddHostedService<CrawlJob>(provider => new CrawlJob(
            //    provider.GetRequiredService<DbOperationService>(),
            //    provider.GetRequiredService<ILogger<CrawlJob>>(),
            //    provider.GetRequiredService<ICrawlMessageBus>(),
            //    provider.GetRequiredService<ICrawlQueue>(),
            //    provider.GetRequiredService<IFileSystemDetailService>()
            //));
        }

        void IProgress<IJobResult<CrawlTerminationReason>>.Report(IJobResult<CrawlTerminationReason> value)
        {
            JobStatus = value.Status;
            //switch (JobStatus)
            //{
            //    case AsyncJobStatus.Cancelling:
            //        _onReportProgress?.Report(new CrawlProgress(value.Started, value.Elapsed, value.Status, "Cancelling background job..."));
            //        break;
            //    case AsyncJobStatus.Canceled:
            //        _onReportProgress?.Report(new CrawlProgress(value.Started, value.Elapsed, value.Status, "Background job canceled."));
            //        break;
            //    case AsyncJobStatus.Faulted:
            //        _onReportProgress?.Report(new CrawlProgress(value.Started, value.Elapsed, value.Status, "Background job failed."));
            //        break;
            //    case AsyncJobStatus.Succeeded:
            //        _onReportProgress?.Report(new CrawlProgress(value.Started, value.Elapsed, value.Status, "Background job completed."));
            //        break;
            //}
        }
    }
}
