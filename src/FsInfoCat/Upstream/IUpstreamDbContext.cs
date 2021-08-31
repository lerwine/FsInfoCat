using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Upstream
{
    /// Generic interface for the upstream (remote) FS InfoCat database context.
    /// Extends the <see cref="IDbContext" /> interface to represent the upstream (remote) database.
    /// </summary>
    /// <seealso cref="IDbContext" />
    public interface IUpstreamDbContext : IDbContext
    {
        /// <summary>
        /// Enumerates generic file comparison entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamComparison> Comparisons { get; }

        /// <summary>
        /// Enumerates generic binary property set entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamBinaryPropertySet> BinaryPropertySets { get; }

        /// <summary>
        /// Enumerates generic summary property set entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamSummaryPropertySet> SummaryPropertySets { get; }

        /// <summary>
        /// Enumerates generic document property set entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamDocumentPropertySet> DocumentPropertySets { get; }

        /// <summary>
        /// Enumerates generic audio property set entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamAudioPropertySet> AudioPropertySets { get; }

        /// <summary>
        /// Enumerates generic DRM property set entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamDRMPropertySet> DRMPropertySets { get; }

        /// <summary>
        /// Enumerates generic GPS property set entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamGPSPropertySet> GPSPropertySets { get; }

        /// <summary>
        /// Enumerates generic image property set entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamImagePropertySet> ImagePropertySets { get; }

        /// <summary>
        /// Enumerates generic media property set entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamMediaPropertySet> MediaPropertySets { get; }

        /// <summary>
        /// Enumerates generic music property set entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamMusicPropertySet> MusicPropertySets { get; }

        /// <summary>
        /// Enumerates generic photo property set entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamPhotoPropertySet> PhotoPropertySets { get; }

        /// <summary>
        /// Enumerates generic recorded TV property set entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamRecordedTVPropertySet> RecordedTVPropertySets { get; }

        /// <summary>
        /// Enumerates generic video property set entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamVideoPropertySet> VideoPropertySets { get; }

        /// <summary>
        /// Enumerates generic file access error entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamFileAccessError> FileAccessErrors { get; }

        /// <summary>
        /// Enumerates generic file entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamFile> Files { get; }

        /// <summary>
        /// Enumerates generic file system entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamFileSystem> FileSystems { get; }

        /// <summary>
        /// Enumerates generic redundancy entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamRedundancy> Redundancies { get; }

        /// <summary>
        /// Enumerates generic redundant set entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamRedundantSet> RedundantSets { get; }

        /// <summary>
        /// Enumerates generic sub-directory entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamSubdirectory> Subdirectories { get; }

        /// <summary>
        /// Enumerates generic sub-directory access error entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamSubdirectoryAccessError> SubdirectoryAccessErrors { get; }

        /// <summary>
        /// Enumerates generic symbolic name entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamSymbolicName> SymbolicNames { get; }

        /// <summary>
        /// Enumerates generic volume access error entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamVolumeAccessError> VolumeAccessErrors { get; }

        /// <summary>
        /// Enumerates generic volume entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamVolume> Volumes { get; }

        new IEnumerable<IUpstreamPersonalTagDefinition> PersonalTagDefinitions { get; }

        new IEnumerable<IUpstreamPersonalFileTag> PersonalFileTags { get; }

        new IEnumerable<IUpstreamPersonalSubdirectoryTag> PersonalSubdirectoryTags { get; }

        new IEnumerable<IUpstreamPersonalVolumeTag> PersonalVolumeTags { get; }

        new IEnumerable<IUpstreamSharedTagDefinition> SharedTagDefinitions { get; }

        new IEnumerable<IUpstreamSharedFileTag> SharedFileTags { get; }

        new IEnumerable<IUpstreamSharedSubdirectoryTag> SharedSubdirectoryTags { get; }

        new IEnumerable<IUpstreamSharedVolumeTag> SharedVolumeTags { get; }

        /// <summary>
        /// Enumerates generic crawl configuration entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamCrawlConfiguration> CrawlConfigurations { get; }

        /// <summary>
        /// Enumerates generic crawl log entities from the upstream (remote) database.
        /// </summary>
        new IEnumerable<IUpstreamCrawlJobLog> CrawlJobLogs { get; }

        /// <summary>
        /// Enumerates generic file action entities from the upstream (remote) database.
        /// </summary>
        IEnumerable<IFileAction> FileActions { get; }

        /// <summary>
        /// Enumerates generic user group membership entities from the upstream (remote) database.
        /// </summary>
        IEnumerable<IGroupMembership> GroupMemberships { get; }

        /// <summary>
        /// Enumerates generic host device entities from the upstream (remote) database.
        /// </summary>
        IEnumerable<IHostDevice> HostDevices { get; }

        /// <summary>
        /// Enumerates generic host platform entities from the upstream (remote) database.
        /// </summary>
        IEnumerable<IHostPlatform> HostPlatforms { get; }

        /// <summary>
        /// Enumerates generic mitigation task entities from the upstream (remote) database.
        /// </summary>
        IEnumerable<IMitigationTask> MitigationTasks { get; }

        /// <summary>
        /// Enumerates generic subdirectory action entities from the upstream (remote) database.
        /// </summary>
        IEnumerable<ISubdirectoryAction> SubdirectoryActions { get; }

        /// <summary>
        /// Enumerates generic user group entities from the upstream (remote) database.
        /// </summary>
        IEnumerable<IUserGroup> UserGroups { get; }

        /// <summary>
        /// Enumerates generic user profile entities from the upstream (remote) database.
        /// </summary>
        IEnumerable<IUserProfile> UserProfiles { get; }

        new IEnumerable<IUpstreamFileSystemListItem> FileSystemListing { get; }

        new IEnumerable<IUpstreamTagDefinitionListItem> PersonalTagDefinitionListing { get; }

        new IEnumerable<IUpstreamTagDefinitionListItem> SharedTagDefinitionListing { get; }

        new IEnumerable<IUpstreamRedundantSetListItem> RedundantSetListing { get; }

        new IEnumerable<IUpstreamVolumeListItem> VolumeListing { get; }

        new IEnumerable<IUpstreamVolumeListItemWithFileSystem> VolumeListingWithFileSystem { get; }

        new IEnumerable<IUpstreamSubdirectoryListItem> SubdirectoryListing { get; }

        new IEnumerable<IUpstreamSubdirectoryListItemWithAncestorNames> SubdirectoryListingWithAncestorNames { get; }

        new IEnumerable<IUpstreamFileListItemWithAncestorNames> FileListingWithAncestorNames { get; }

        new IEnumerable<IUpstreamFileListItemWithBinaryProperties> FileListingWithBinaryProperties { get; }

        new IEnumerable<IUpstreamFileListItemWithBinaryPropertiesAndAncestorNames> FileListingWithBinaryPropertiesAndAncestorNames { get; }

        new IEnumerable<IUpstreamCrawlConfigurationListItem> CrawlConfigListing { get; }

        new IEnumerable<IUpstreamCrawlJobListItem> CrawlJobListing { get; }

        IEnumerable<IGroupMemberOfListItem> GroupMemberOfListing { get; }

        IEnumerable<IGroupMemberListItem> GroupMemberListing { get; }

        IEnumerable<IHostDeviceListItem> HostDeviceListing { get; }

        IEnumerable<IHostPlatformListItem> HostPlatformListing { get; }

        IEnumerable<IMitigationTaskListItem> MitigationTaskListing { get; }

        IEnumerable<IUserGroupListItem> UserGroupListing { get; }

        IEnumerable<IUserProfileListItem> UserProfileListing { get; }

        /// <summary>
        /// Finds the generic <see cref="IUpstreamSummaryPropertySet"/> in the upstream (remote) database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="ISummaryProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IUpstreamSummaryPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<IUpstreamSummaryPropertySet> FindMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IUpstreamDocumentPropertySet"/> in the upstream (remote) database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IDocumentProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IUpstreamDocumentPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<IUpstreamDocumentPropertySet> FindMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IUpstreamAudioPropertySet"/> in the upstream (remote) database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IAudioProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IUpstreamAudioPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<IUpstreamAudioPropertySet> FindMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IUpstreamDRMPropertySet"/> in the upstream (remote) database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IDRMProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IUpstreamDRMPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<IUpstreamDRMPropertySet> FindMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IUpstreamGPSPropertySet"/> in the upstream (remote) database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IGPSProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IUpstreamGPSPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<IUpstreamGPSPropertySet> FindMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IUpstreamImagePropertySet"/> in the upstream (remote) database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IImageProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IUpstreamImagePropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<IUpstreamImagePropertySet> FindMatchingAsync(IImageProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IUpstreamMediaPropertySet"/> in the upstream (remote) database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IMediaProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IUpstreamMediaPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<IUpstreamMediaPropertySet> FindMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IUpstreamMusicPropertySet"/> in the upstream (remote) database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IMusicProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IUpstreamMusicPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<IUpstreamMusicPropertySet> FindMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IUpstreamPhotoPropertySet"/> in the upstream (remote) database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IPhotoProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IUpstreamPhotoPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<IUpstreamPhotoPropertySet> FindMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IUpstreamRecordedTVPropertySet"/> in the upstream (remote) database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IRecordedTVProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IUpstreamRecordedTVPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<IUpstreamRecordedTVPropertySet> FindMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IUpstreamVideoPropertySet"/> in the upstream (remote) database that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IVideoProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IUpstreamVideoPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        new Task<IUpstreamVideoPropertySet> FindMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken);
    }
}
