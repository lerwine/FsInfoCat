using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public interface IFileDetailProvider : IDisposable
    {
        Task<SummaryPropertySet> GetSummaryPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<DocumentPropertySet> GetDocumentPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<AudioPropertySet> GetAudioPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<DRMPropertySet> GetDRMPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<GPSPropertySet> GetGPSPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<ImagePropertySet> GetImagePropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<MediaPropertySet> GetMediaPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<MusicPropertySet> GetMusicPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<PhotoPropertySet> GetPhotoPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<RecordedTVPropertySet> GetRecordedTVPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
        Task<VideoPropertySet> GetVideoPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
    }
}
