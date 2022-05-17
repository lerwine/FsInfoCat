using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for the local FS InfoCat database context.
    /// Extends the <see cref="IDbContext" /> interface to represent the local database.
    /// </summary>
    /// <seealso cref="IDbContext" />
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
        new IEnumerable<ILocalFileAccessError> FileAccessErrors { get; }

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
        new IEnumerable<ILocalSubdirectoryAccessError> SubdirectoryAccessErrors { get; }

        /// <summary>
        /// Enumerates generic symbolic name entities from the local database.
        /// </summary>
        new IEnumerable<ILocalSymbolicName> SymbolicNames { get; }

        /// <summary>
        /// Enumerates generic volume access error entities from the local database.
        /// </summary>
        new IEnumerable<ILocalVolumeAccessError> VolumeAccessErrors { get; }

        /// <summary>
        /// Enumerates generic volume entities from the local database.
        /// </summary>
        new IEnumerable<ILocalVolume> Volumes { get; }

        // TODO: ILocalDbContext members
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        new IEnumerable<ILocalPersonalTagDefinition> PersonalTagDefinitions { get; }

        new IEnumerable<ILocalPersonalFileTag> PersonalFileTags { get; }

        new IEnumerable<ILocalPersonalSubdirectoryTag> PersonalSubdirectoryTags { get; }

        new IEnumerable<ILocalPersonalVolumeTag> PersonalVolumeTags { get; }

        new IEnumerable<ILocalSharedTagDefinition> SharedTagDefinitions { get; }

        new IEnumerable<ILocalSharedFileTag> SharedFileTags { get; }

        new IEnumerable<ILocalSharedSubdirectoryTag> SharedSubdirectoryTags { get; }

        new IEnumerable<ILocalSharedVolumeTag> SharedVolumeTags { get; }

        /// <summary>
        /// Enumerates generic crawl configuration entities from the local database.
        /// </summary>
        new IEnumerable<ILocalCrawlConfiguration> CrawlConfigurations { get; }

        /// <summary>
        /// Enumerates generic crawl log entities from the local database.
        /// </summary>
        new IEnumerable<ILocalCrawlJobLog> CrawlJobLogs { get; }

        new IEnumerable<ILocalFileSystemListItem> FileSystemListing { get; }

        new IEnumerable<ILocalTagDefinitionListItem> PersonalTagDefinitionListing { get; }

        new IEnumerable<ILocalTagDefinitionListItem> SharedTagDefinitionListing { get; }

        new IEnumerable<ILocalRedundantSetListItem> RedundantSetListing { get; }

        new IEnumerable<ILocalVolumeListItem> VolumeListing { get; }

        new IEnumerable<ILocalVolumeListItemWithFileSystem> VolumeListingWithFileSystem { get; }

        new IEnumerable<ILocalSubdirectoryListItem> SubdirectoryListing { get; }

        new IEnumerable<ILocalSubdirectoryListItemWithAncestorNames> SubdirectoryListingWithAncestorNames { get; }

        new IEnumerable<ILocalFileListItemWithAncestorNames> FileListingWithAncestorNames { get; }

        new IEnumerable<ILocalFileListItemWithBinaryProperties> FileListingWithBinaryProperties { get; }

        new IEnumerable<ILocalFileListItemWithBinaryPropertiesAndAncestorNames> FileListingWithBinaryPropertiesAndAncestorNames { get; }

        new IEnumerable<ILocalCrawlConfigurationListItem> CrawlConfigListing { get; }

        new IEnumerable<ILocalCrawlConfigReportItem> CrawlConfigReport { get; }

        new IEnumerable<ILocalCrawlJobListItem> CrawlJobListing { get; }

        new IEnumerable<ILocalSummaryPropertiesListItem> SummaryPropertiesListing { get; }

        new IEnumerable<ILocalDocumentPropertiesListItem> DocumentPropertiesListing { get; }

        new IEnumerable<ILocalAudioPropertiesListItem> AudioPropertiesListing { get; }

        new IEnumerable<ILocalDRMPropertiesListItem> DRMPropertiesListing { get; }

        new IEnumerable<ILocalGPSPropertiesListItem> GPSPropertiesListing { get; }

        new IEnumerable<ILocalImagePropertiesListItem> ImagePropertiesListing { get; }

        new IEnumerable<ILocalMediaPropertiesListItem> MediaPropertiesListing { get; }

        new IEnumerable<ILocalMusicPropertiesListItem> MusicPropertiesListing { get; }

        new IEnumerable<ILocalPhotoPropertiesListItem> PhotoPropertiesListing { get; }

        new IEnumerable<ILocalRecordedTVPropertiesListItem> RecordedTVPropertiesListing { get; }

        new IEnumerable<ILocalVideoPropertiesListItem> VideoPropertiesListing { get; }

        new IEnumerable<ILocalItemTagListItem> PersonalVolumeTagListing { get; }

        new IEnumerable<ILocalItemTagListItem> SharedVolumeTagListing { get; }

        new IEnumerable<ILocalItemTagListItem> PersonalSubdirectoryTagListing { get; }

        new IEnumerable<ILocalItemTagListItem> SharedSubdirectoryTagListing { get; }

        new IEnumerable<ILocalItemTagListItem> PersonalFileTagListing { get; }

        new IEnumerable<ILocalItemTagListItem> SharedFileTagListing { get; }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

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
    }
}
