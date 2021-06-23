using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Interface for an object that retrieves extended file information.
    /// </summary>
    public interface IFileDetailProvider : IDisposable
    {
        /// <summary>
        /// Gets the summary properties.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="EntityEntry{TEntity}">EntityEntry</see> containing the <see cref="SummaryPropertySet"/>.</returns>
        Task<EntityEntry<SummaryPropertySet>> GetSummaryPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the document properties.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="EntityEntry{TEntity}">EntityEntry</see> containing the <see cref="DocumentPropertySet"/> or <see langword="null"/> if the file has no document properties.</returns>
        Task<EntityEntry<DocumentPropertySet>> GetDocumentPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the audio properties.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="EntityEntry{TEntity}">EntityEntry</see> containing the <see cref="AudioPropertySet"/> or <see langword="null"/> if the file has no audio properties.</returns>
        Task<EntityEntry<AudioPropertySet>> GetAudioPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the DRM properties.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="EntityEntry{TEntity}">EntityEntry</see> containing the <see cref="DRMPropertySet"/> or <see langword="null"/> if the file has no DRM properties.</returns>
        Task<EntityEntry<DRMPropertySet>> GetDRMPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the GPS properties.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="EntityEntry{TEntity}">EntityEntry</see> containing the <see cref="GPSPropertySet"/> or <see langword="null"/> if the file has no GPS properties.</returns>
        Task<EntityEntry<GPSPropertySet>> GetGPSPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the image properties.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="EntityEntry{TEntity}">EntityEntry</see> containing the <see cref="ImagePropertySet"/> or <see langword="null"/> if the file has no image properties.</returns>
        Task<EntityEntry<ImagePropertySet>> GetImagePropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the media properties.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="EntityEntry{TEntity}">EntityEntry</see> containing the <see cref="MediaPropertySet"/> or <see langword="null"/> if the file has no media properties.</returns>
        Task<EntityEntry<MediaPropertySet>> GetMediaPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the music properties.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="EntityEntry{TEntity}">EntityEntry</see> containing the <see cref="MusicPropertySet"/> or <see langword="null"/> if the file has no music properties.</returns>
        Task<EntityEntry<MusicPropertySet>> GetMusicPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the photo properties.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="EntityEntry{TEntity}">EntityEntry</see> containing the <see cref="PhotoPropertySet"/> or <see langword="null"/> if the file has no photo properties.</returns>
        Task<EntityEntry<PhotoPropertySet>> GetPhotoPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the recorded TV properties.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="EntityEntry{TEntity}">EntityEntry</see> containing the <see cref="RecordedTVPropertySet"/> or <see langword="null"/> if the file has no recorded TV properties.</returns>
        Task<EntityEntry<RecordedTVPropertySet>> GetRecordedTVPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the video properties.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="EntityEntry{TEntity}">EntityEntry</see> containing the <see cref="VideoPropertySet"/> or <see langword="null"/> if the file has no video properties.</returns>
        Task<EntityEntry<VideoPropertySet>> GetVideoPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken);
    }
}
