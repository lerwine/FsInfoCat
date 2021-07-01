using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Interface for an object that retrieves extended properties for a specific file.
    /// </summary>
    public interface IFileDetailProvider : IDisposable
    {
        /// <summary>
        /// Gets the summary properties for the current file.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ISummaryProperties"/> object.</returns>
        Task<ISummaryProperties> GetSummaryPropertiesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the document properties for the current file.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="IDocumentProperties"/> object or <see langword="null"/> if the file has no document properties.</returns>
        Task<IDocumentProperties> GetDocumentPropertiesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the audio properties for the current file.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="IAudioProperties"/> object or <see langword="null"/> if the file has no audio properties.</returns>
        Task<IAudioProperties> GetAudioPropertiesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the DRM properties for the current file.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="IDRMProperties"/> object or <see langword="null"/> if the file has no DRM properties.</returns>
        Task<IDRMProperties> GetDRMPropertiesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the GPS properties for the current file.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="IGPSProperties"/> object or <see langword="null"/> if the file has no GPS properties.</returns>
        Task<IGPSProperties> GetGPSPropertiesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the image properties for the current file.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="IImageProperties"/> object or <see langword="null"/> if the file has no image properties.</returns>
        Task<IImageProperties> GetImagePropertiesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the media properties for the current file.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="IMediaProperties"/> object or <see langword="null"/> if the file has no media properties.</returns>
        Task<IMediaProperties> GetMediaPropertiesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the music properties for the current file.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="IMusicProperties"/> object or <see langword="null"/> if the file has no music properties.</returns>
        Task<IMusicProperties> GetMusicPropertiesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the photo properties for the current file.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="IPhotoProperties"/> object or <see langword="null"/> if the file has no photo properties.</returns>
        Task<IPhotoProperties> GetPhotoPropertiesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the recorded TV properties for the current file.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="IRecordedTVProperties"/> object or <see langword="null"/> if the file has no recorded TV properties.</returns>
        Task<IRecordedTVProperties> GetRecordedTVPropertiesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the video properties for the current file.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="IVideoProperties"/> object or <see langword="null"/> if the file has no video properties.</returns>
        Task<IVideoProperties> GetVideoPropertiesAsync(CancellationToken cancellationToken);
    }
}
