using FsInfoCat.Local;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop
{
    public partial class FileSystemImportJob : IDisposable
    {
        private readonly object _syncRoot = new();
        private CancellationTokenSource _tokenSource = new();

        public ushort MaxRecursionDepth { get; }

        public ulong MaxTotalItems { get; }

        public string DisplayName { get; }

        public Task Task { get; }

        public FileSystemImportJob(CrawlConfiguration configuration, FileSystemImportObserver observer)
        {
            MaxRecursionDepth = configuration.MaxRecursionDepth;
            MaxTotalItems = configuration.MaxTotalItems;
            DisplayName = configuration.DisplayName;
            Task = ScanContext.Create(configuration, observer, _tokenSource.Token);
        }

        public void Dispose()
        {
            CancellationTokenSource tokenSource;
            lock (_syncRoot)
            {
                tokenSource = _tokenSource;
                _tokenSource = null;
            }
            if (tokenSource is null)
                return;
            using (tokenSource)
            {
                if (!Task.IsCompleted)
                {
                    tokenSource.Cancel(true);
                    Thread.Sleep(0);
                }
            }
            GC.SuppressFinalize(this);
        }
    }
}
