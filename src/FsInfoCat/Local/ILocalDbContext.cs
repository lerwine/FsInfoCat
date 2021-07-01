using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for the local FS InfoCat database context.
    /// Extends the <see cref="FsInfoCat.IDbContext" /> interface to represent the local database.
    /// </summary>
    /// <seealso cref="FsInfoCat.IDbContext" />
    public interface ILocalDbContext : IDbContext
    {
        /// <summary>
        /// Enumerates generic file comparison entities from the local database.
        /// </summary>
        new IEnumerable<ILocalComparison> Comparisons { get; }

        /// <summary>
        /// Enumerates generic binary property set entities from the local database.
        /// </summary>
        new IEnumerable<ILocalBinaryPropertySet> BinaryPropertySets { get; }

        /// <summary>
        /// Enumerates generic summary property set entities from the local database.
        /// </summary>
        new IEnumerable<ILocalSummaryPropertySet> SummaryPropertySets { get; }

        /// <summary>
        /// Enumerates generic document property set entities from the local database.
        /// </summary>
        new IEnumerable<ILocalDocumentPropertySet> DocumentPropertySets { get; }

        /// <summary>
        /// Enumerates generic audio property set entities from the local database.
        /// </summary>
        new IEnumerable<ILocalAudioPropertySet> AudioPropertySets { get; }

        /// <summary>
        /// Enumerates generic DRM property set entities from the local database.
        /// </summary>
        new IEnumerable<ILocalDRMPropertySet> DRMPropertySets { get; }

        /// <summary>
        /// Enumerates generic GPS property set entities from the local database.
        /// </summary>
        new IEnumerable<ILocalGPSPropertySet> GPSPropertySets { get; }

        /// <summary>
        /// Enumerates generic image property set entities from the local database.
        /// </summary>
        new IEnumerable<ILocalImagePropertySet> ImagePropertySets { get; }

        /// <summary>
        /// Enumerates generic media property set entities from the local database.
        /// </summary>
        new IEnumerable<ILocalMediaPropertySet> MediaPropertySets { get; }

        /// <summary>
        /// Enumerates generic music property set entities from the local database.
        /// </summary>
        new IEnumerable<ILocalMusicPropertySet> MusicPropertySets { get; }

        /// <summary>
        /// Enumerates generic photo property set entities from the local database.
        /// </summary>
        new IEnumerable<ILocalPhotoPropertySet> PhotoPropertySets { get; }

        /// <summary>
        /// Enumerates generic recorded TV property set entities from the local database.
        /// </summary>
        new IEnumerable<ILocalRecordedTVPropertySet> RecordedTVPropertySets { get; }

        /// <summary>
        /// Enumerates generic video property set entities from the local database.
        /// </summary>
        new IEnumerable<ILocalVideoPropertySet> VideoPropertySets { get; }

        /// <summary>
        /// Enumerates generic file access error entities from the local database.
        /// </summary>
        new IEnumerable<IAccessError<ILocalFile>> FileAccessErrors { get; }

        /// <summary>
        /// Enumerates generic file entities from the local database.
        /// </summary>
        new IEnumerable<ILocalFile> Files { get; }

        /// <summary>
        /// Enumerates generic file system entities from the local database.
        /// </summary>
        new IEnumerable<ILocalFileSystem> FileSystems { get; }

        /// <summary>
        /// Enumerates generic redundancy entities from the local database.
        /// </summary>
        new IEnumerable<ILocalRedundancy> Redundancies { get; }

        /// <summary>
        /// Enumerates generic redundant set entities from the local database.
        /// </summary>
        new IEnumerable<ILocalRedundantSet> RedundantSets { get; }

        /// <summary>
        /// Enumerates generic sub-directory entities from the local database.
        /// </summary>
        new IEnumerable<ILocalSubdirectory> Subdirectories { get; }

        /// <summary>
        /// Enumerates generic sub-directory access error entities from the local database.
        /// </summary>
        new IEnumerable<IAccessError<ILocalSubdirectory>> SubdirectoryAccessErrors { get; }

        /// <summary>
        /// Enumerates generic symbolic name entities from the local database.
        /// </summary>
        new IEnumerable<ILocalSymbolicName> SymbolicNames { get; }

        /// <summary>
        /// Enumerates generic volume access error entities from the local database.
        /// </summary>
        new IEnumerable<IAccessError<ILocalVolume>> VolumeAccessErrors { get; }

        /// <summary>
        /// Enumerates generic volume entities from the local database.
        /// </summary>
        new IEnumerable<ILocalVolume> Volumes { get; }

        /// <summary>
        /// Enumerates generic crawl configuration entities from the local database.
        /// </summary>
        new IEnumerable<ILocalCrawlConfiguration> CrawlConfigurations { get; }

        /// <summary>
        /// Finds the generic <see cref="ILocalSummaryPropertySet"/> in the local database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="ISummaryProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="ILocalSummaryPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<ILocalSummaryPropertySet> FindMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="ILocalDocumentPropertySet"/> in the local database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IDocumentProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="ILocalDocumentPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<ILocalDocumentPropertySet> FindMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="ILocalAudioPropertySet"/> in the local database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IAudioProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="ILocalAudioPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<ILocalAudioPropertySet> FindMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="ILocalDRMPropertySet"/> in the local database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IDRMProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="ILocalDRMPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<ILocalDRMPropertySet> FindMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="ILocalGPSPropertySet"/> in the local database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IGPSProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="ILocalGPSPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<ILocalGPSPropertySet> FindMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="ILocalImagePropertySet"/> in the local database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IImageProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="ILocalImagePropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<ILocalImagePropertySet> FindMatchingAsync(IImageProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="ILocalMediaPropertySet"/> in the local database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IMediaProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="ILocalMediaPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<ILocalMediaPropertySet> FindMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="ILocalMusicPropertySet"/> in the local database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IMusicProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="ILocalMusicPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<ILocalMusicPropertySet> FindMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="ILocalPhotoPropertySet"/> in the local database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IPhotoProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="ILocalPhotoPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<ILocalPhotoPropertySet> FindMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="ILocalRecordedTVPropertySet"/> in the local database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IRecordedTVProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="ILocalRecordedTVPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<ILocalRecordedTVPropertySet> FindMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="ILocalVideoPropertySet"/> in the local database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IVideoProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="ILocalVideoPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<ILocalVideoPropertySet> FindMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken);

        //[Obsolete("Use ForceDeleteBinaryPropertySetAsync")]
        //void ForceDeleteBinaryPropertySet(ILocalBinaryPropertySet target);

        ///// <summary>
        ///// Deletes the specified <see cref="ILocalBinaryPropertySet"/> from the local database, including all nested dependencies.
        ///// </summary>
        ///// <param name="target">The <see cref="ILocalBinaryPropertySet"/> to delete.</param>
        ///// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        ///// <returns><see langword="true"/> if the <paramref name="target"/> was deleted; otherwise, <see langword="false"/>.</returns>
        //Task<bool> ForceDeleteBinaryPropertySetAsync(ILocalBinaryPropertySet target, CancellationToken cancellationToken);

        //[Obsolete("Use ForceDeleteRedundantSetAsync")]
        //void ForceDeleteRedundantSet(ILocalRedundantSet target);

        ///// <summary>
        ///// Deletes the specified <see cref="ILocalRedundantSet"/> from the local database, including all nested dependencies.
        ///// </summary>
        ///// <param name="target">The <see cref="ILocalRedundantSet"/> to delete.</param>
        ///// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        ///// <returns><see langword="true"/> if the <paramref name="target"/> was deleted; otherwise, <see langword="false"/>.</returns>
        //Task<bool> ForceDeleteRedundantSetAsync(ILocalRedundantSet targe, CancellationToken cancellationToken);

        //[Obsolete("Use ForceDeleteFileSystemAsync")]
        //void ForceDeleteFileSystem(ILocalFileSystem target);

        ///// <summary>
        ///// Deletes the specified <see cref="ILocalFileSystem"/> from the local database, including all nested dependencies.
        ///// </summary>
        ///// <param name="target">The <see cref="ILocalFileSystem"/> to delete.</param>
        ///// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        ///// <returns><see langword="true"/> if the <paramref name="target"/> was deleted; otherwise, <see langword="false"/>.</returns>
        //Task<bool> ForceDeleteFileSystemAsync(ILocalFileSystem target, CancellationToken cancellationToken);
    }
}
