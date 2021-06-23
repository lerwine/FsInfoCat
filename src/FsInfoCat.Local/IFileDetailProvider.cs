using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public interface IFileDetailProvider : IDisposable
    {
        Task<EntityEntry<SummaryPropertySet>> GetSummaryPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<EntityEntry<DocumentPropertySet>> GetDocumentPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<EntityEntry<AudioPropertySet>> GetAudioPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<EntityEntry<DRMPropertySet>> GetDRMPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<EntityEntry<GPSPropertySet>> GetGPSPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<EntityEntry<ImagePropertySet>> GetImagePropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<EntityEntry<MediaPropertySet>> GetMediaPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<EntityEntry<MusicPropertySet>> GetMusicPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<EntityEntry<PhotoPropertySet>> GetPhotoPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<EntityEntry<RecordedTVPropertySet>> GetRecordedTVPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<EntityEntry<VideoPropertySet>> GetVideoPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
    }
}
