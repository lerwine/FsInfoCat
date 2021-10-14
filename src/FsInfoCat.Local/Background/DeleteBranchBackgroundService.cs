using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public class DeleteBranchBackgroundService : LongRunningAsyncService, IDeleteBranchBackgroundService
    {
        public new Task<bool> Task { get; private set; }

        Task<bool> ILongRunningAsyncService<bool>.Task => Task;

        public Subdirectory Target { get; }

        public DeleteBranchBackgroundService(Subdirectory target)
        {
            Target = target;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
