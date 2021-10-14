using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public class DeleteVolumeBackgroundService : LongRunningAsyncService, IDeleteVolumeBackgroundService
    {
        public new Task<bool> Task { get; private set; }

        Task<bool> ILongRunningAsyncService<bool>.Task => Task;

        public Volume Target { get; }

        public DeleteVolumeBackgroundService(Volume target)
        {
            Target = target;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
