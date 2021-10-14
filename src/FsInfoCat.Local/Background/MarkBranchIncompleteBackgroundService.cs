using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public class MarkBranchIncompleteBackgroundService : LongRunningAsyncService, IMarkBranchIncompleteBackgroundService
    {
        public new Task<bool> Task { get; private set; }

        Task<bool> ILongRunningAsyncService<bool>.Task => Task;

        public Subdirectory Target { get; }

        public MarkBranchIncompleteBackgroundService(Subdirectory target)
        {
            Target = target;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
