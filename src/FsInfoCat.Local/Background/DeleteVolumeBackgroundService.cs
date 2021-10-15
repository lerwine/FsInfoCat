using FsInfoCat.Background;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public class DeleteVolumeBackgroundService : LongRunningAsyncService<Task<bool>>, IDeleteVolumeBackgroundService<Volume>
    {
        private readonly LocalDbContext _dbContext;
        private readonly DeleteBranchBackgroundService _deleteBranch;

        public Volume Target { get; set; }

        IVolume IDeleteVolumeBackgroundService.Target => Target;

        public bool ForceDeleteFromDb { get; set; }

        public IProgress<string> ReportProgressHandler { get; set; }

        public DeleteVolumeBackgroundService(ILogger<DeleteVolumeBackgroundService> logger, LocalDbContext dbContext, DeleteBranchBackgroundService deleteBranch)
            : base(logger)
        {
            _dbContext = dbContext;
            _deleteBranch = deleteBranch;
        }

        protected async override Task<bool> ExecuteAsync()
        {
            Volume target = Target;
            bool forceDeleteFromDb = ForceDeleteFromDb;
            IProgress<string> reportProgressHandler = ReportProgressHandler;
            EntityEntry<Volume> entry = _dbContext.Entry(Target ?? throw new InvalidOperationException($"{nameof(Target)} cannot be null"));
            _deleteBranch.ForceDeleteFromDb = forceDeleteFromDb;
            _deleteBranch.ReportProgressHandler = reportProgressHandler;
            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(StoppingToken);
            if ((_deleteBranch.Target = await entry.GetRelatedReferenceAsync(v => v.RootDirectory, StoppingToken)) is not null)
            {
                await _deleteBranch.StartAsync(StoppingToken);
                forceDeleteFromDb = _deleteBranch.Task.Result;
            }
            else
                forceDeleteFromDb = true;
            foreach (VolumeAccessError error in (await entry.GetRelatedCollectionAsync(e => e.AccessErrors, StoppingToken)).ToArray())
                _dbContext.VolumeAccessErrors.Remove(error);
            foreach (PersonalVolumeTag tag in (await entry.GetRelatedCollectionAsync(e => e.PersonalTags, StoppingToken)).ToArray())
                _dbContext.PersonalVolumeTags.Remove(tag);
            foreach (SharedVolumeTag tag in (await entry.GetRelatedCollectionAsync(e => e.SharedTags, StoppingToken)).ToArray())
                _dbContext.SharedVolumeTags.Remove(tag);
            await _dbContext.SaveChangesAsync(StoppingToken);
            if (forceDeleteFromDb)
            {
                _dbContext.Volumes.Remove(target);
                await _dbContext.SaveChangesAsync(StoppingToken);
                return true;
            }
            return false;
        }
    }
}
