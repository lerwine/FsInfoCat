using FsInfoCat.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for the upstream (remote) FS InfoCat database context.
    /// Extends the <see cref="IDbContext" /> interface to represent the upstream (remote) database.
    /// </summary>
    /// <seealso cref="IDbContext" />
    /// <seealso cref="Local.Model.ILocalDbContext" />
    /// <seealso cref="IFileAction" />
    /// <seealso cref="IGroupMemberListItem" />
    /// <seealso cref="IGroupMemberOfListItem" />
    /// <seealso cref="IGroupMembership" />
    /// <seealso cref="IHostDevice" />
    /// <seealso cref="IHostDeviceListItem" />
    /// <seealso cref="IHostPlatform" />
    /// <seealso cref="IHostPlatformListItem" />
    /// <seealso cref="IMitigationTask" />
    /// <seealso cref="IMitigationTaskListItem" />
    /// <seealso cref="ISubdirectoryAction" />
    /// <seealso cref="ISubdirectoryAncestorName" />
    /// <seealso cref="IUpstreamAudioPropertiesListItem" />
    /// <seealso cref="IUpstreamAudioPropertySet" />
    /// <seealso cref="IUpstreamBinaryPropertySet" />
    /// <seealso cref="IUpstreamComparison" />
    /// <seealso cref="IUpstreamCrawlConfigReportItem" />
    /// <seealso cref="IUpstreamCrawlConfiguration" />
    /// <seealso cref="IUpstreamCrawlConfigurationListItem" />
    /// <seealso cref="IUpstreamCrawlJobListItem" />
    /// <seealso cref="IUpstreamCrawlJobLog" />
    /// <seealso cref="IUpstreamDocumentPropertiesListItem" />
    /// <seealso cref="IUpstreamDocumentPropertySet" />
    /// <seealso cref="IUpstreamDRMPropertiesListItem" />
    /// <seealso cref="IUpstreamDRMPropertySet" />
    /// <seealso cref="IUpstreamFile" />
    /// <seealso cref="IUpstreamFileAccessError" />
    /// <seealso cref="IUpstreamFileListItemWithAncestorNames" />
    /// <seealso cref="IUpstreamFileListItemWithBinaryProperties" />
    /// <seealso cref="IUpstreamFileListItemWithBinaryPropertiesAndAncestorNames" />
    /// <seealso cref="IUpstreamFileSystem" />
    /// <seealso cref="IUpstreamFileSystemListItem" />
    /// <seealso cref="IUpstreamGPSPropertiesListItem" />
    /// <seealso cref="IUpstreamGPSPropertySet" />
    /// <seealso cref="IUpstreamImagePropertiesListItem" />
    /// <seealso cref="IUpstreamImagePropertySet" />
    /// <seealso cref="IUpstreamMediaPropertiesListItem" />
    /// <seealso cref="IUpstreamMediaPropertySet" />
    /// <seealso cref="IUpstreamMusicPropertiesListItem" />
    /// <seealso cref="IUpstreamMusicPropertySet" />
    /// <seealso cref="IUpstreamPersonalFileTag" />
    /// <seealso cref="IUpstreamPersonalSubdirectoryTag" />
    /// <seealso cref="IUpstreamPersonalTagDefinition" />
    /// <seealso cref="IUpstreamPersonalVolumeTag" />
    /// <seealso cref="IUpstreamPhotoPropertiesListItem" />
    /// <seealso cref="IUpstreamPhotoPropertySet" />
    /// <seealso cref="IUpstreamRecordedTVPropertiesListItem" />
    /// <seealso cref="IUpstreamRecordedTVPropertySet" />
    /// <seealso cref="IUpstreamRedundancy" />
    /// <seealso cref="IUpstreamRedundantSet" />
    /// <seealso cref="IUpstreamRedundantSetListItem" />
    /// <seealso cref="IUpstreamSharedFileTag" />
    /// <seealso cref="IUpstreamSharedSubdirectoryTag" />
    /// <seealso cref="IUpstreamSharedTagDefinition" />
    /// <seealso cref="IUpstreamSharedVolumeTag" />
    /// <seealso cref="IUpstreamSubdirectory" />
    /// <seealso cref="IUpstreamSubdirectoryAccessError" />
    /// <seealso cref="IUpstreamSubdirectoryListItem" />
    /// <seealso cref="IUpstreamSubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="IUpstreamSummaryPropertiesListItem" />
    /// <seealso cref="IUpstreamSummaryPropertySet" />
    /// <seealso cref="IUpstreamSymbolicName" />
    /// <seealso cref="IUpstreamSymbolicNameListItem" />
    /// <seealso cref="IUpstreamVideoPropertiesListItem" />
    /// <seealso cref="IUpstreamVideoPropertySet" />
    /// <seealso cref="IUpstreamVolume" />
    /// <seealso cref="IUpstreamVolumeAccessError" />
    /// <seealso cref="IUpstreamVolumeListItem" />
    /// <seealso cref="IUpstreamVolumeListItemWithFileSystem" />
    /// <seealso cref="IUserGroup" />
    /// <seealso cref="IUserGroupListItem" />
    /// <seealso cref="IUserProfile" />
    /// <seealso cref="IUserProfileListItem" />
    public interface IUpstreamDbContext : IDbContext
    {
        /// <summary>
        /// Enumerates generic file comparison entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamComparison" /> entities.</value>
        new IEnumerable<IUpstreamComparison> Comparisons { get; }

        /// <summary>
        /// Enumerates generic binary property set entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamBinaryPropertySet" /> entities.</value>
        new IEnumerable<IUpstreamBinaryPropertySet> BinaryPropertySets { get; }

        /// <summary>
        /// Enumerates generic summary property set entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamSummaryPropertySet" /> entities.</value>
        new IEnumerable<IUpstreamSummaryPropertySet> SummaryPropertySets { get; }

        /// <summary>
        /// Enumerates generic document property set entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamDocumentPropertySet" /> entities.</value>
        new IEnumerable<IUpstreamDocumentPropertySet> DocumentPropertySets { get; }

        /// <summary>
        /// Enumerates generic audio property set entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamAudioPropertySet" /> entities.</value>
        new IEnumerable<IUpstreamAudioPropertySet> AudioPropertySets { get; }

        /// <summary>
        /// Enumerates generic DRM property set entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamDRMPropertySet" /> entities.</value>
        new IEnumerable<IUpstreamDRMPropertySet> DRMPropertySets { get; }

        /// <summary>
        /// Enumerates generic GPS property set entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamGPSPropertySet" /> entities.</value>
        new IEnumerable<IUpstreamGPSPropertySet> GPSPropertySets { get; }

        /// <summary>
        /// Enumerates generic image property set entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamImagePropertySet" /> entities.</value>
        new IEnumerable<IUpstreamImagePropertySet> ImagePropertySets { get; }

        /// <summary>
        /// Enumerates generic media property set entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamMediaPropertySet" /> entities.</value>
        new IEnumerable<IUpstreamMediaPropertySet> MediaPropertySets { get; }

        /// <summary>
        /// Enumerates generic music property set entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamMusicPropertySet" /> entities.</value>
        new IEnumerable<IUpstreamMusicPropertySet> MusicPropertySets { get; }

        /// <summary>
        /// Enumerates generic photo property set entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamPhotoPropertySet" /> entities.</value>
        new IEnumerable<IUpstreamPhotoPropertySet> PhotoPropertySets { get; }

        /// <summary>
        /// Enumerates generic recorded TV property set entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamRecordedTVPropertySet" /> entities.</value>
        new IEnumerable<IUpstreamRecordedTVPropertySet> RecordedTVPropertySets { get; }

        /// <summary>
        /// Enumerates generic video property set entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamVideoPropertySet" /> entities.</value>
        new IEnumerable<IUpstreamVideoPropertySet> VideoPropertySets { get; }

        /// <summary>
        /// Enumerates generic file access error entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamFileAccessError" /> entities.</value>
        new IEnumerable<IUpstreamFileAccessError> FileAccessErrors { get; }

        /// <summary>
        /// Enumerates generic file entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamFile" /> entities.</value>
        new IEnumerable<IUpstreamFile> Files { get; }

        /// <summary>
        /// Enumerates generic file system entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamFileSystem" /> entities.</value>
        new IEnumerable<IUpstreamFileSystem> FileSystems { get; }

        /// <summary>
        /// Enumerates generic redundancy entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamRedundancy" /> entities.</value>
        new IEnumerable<IUpstreamRedundancy> Redundancies { get; }

        /// <summary>
        /// Enumerates generic redundant set entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamRedundantSet" /> entities.</value>
        new IEnumerable<IUpstreamRedundantSet> RedundantSets { get; }

        /// <summary>
        /// Enumerates generic sub-directory entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamSubdirectory" /> entities.</value>
        new IEnumerable<IUpstreamSubdirectory> Subdirectories { get; }

        /// <summary>
        /// Enumerates generic sub-directory access error entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamSubdirectoryAccessError" /> entities.</value>
        new IEnumerable<IUpstreamSubdirectoryAccessError> SubdirectoryAccessErrors { get; }

        /// <summary>
        /// Enumerates generic symbolic name entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamSymbolicName" /> entities.</value>
        new IEnumerable<IUpstreamSymbolicName> SymbolicNames { get; }

        /// <summary>
        /// Enumerates generic volume access error entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamVolumeAccessError" /> entities.</value>
        new IEnumerable<IUpstreamVolumeAccessError> VolumeAccessErrors { get; }

        /// <summary>
        /// Enumerates generic volume entities from the upstream (remote) database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamVolume" /> entities.</value>
        new IEnumerable<IUpstreamVolume> Volumes { get; }

        /// <summary>
        /// Enumerates generic personal tag definition entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamPersonalTagDefinition" /> entities.</value>
        new IEnumerable<IUpstreamPersonalTagDefinition> PersonalTagDefinitions { get; }

        /// <summary>
        /// Enumerates generic personal file tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamPersonalFileTag" /> entities.</value>
        new IEnumerable<IUpstreamPersonalFileTag> PersonalFileTags { get; }

        /// <summary>
        /// Enumerates generic personal subdirectory tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamPersonalSubdirectoryTag" /> entities.</value>
        new IEnumerable<IUpstreamPersonalSubdirectoryTag> PersonalSubdirectoryTags { get; }

        /// <summary>
        /// Enumerates generic personal volume tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamPersonalVolumeTag" /> entities.</value>
        new IEnumerable<IUpstreamPersonalVolumeTag> PersonalVolumeTags { get; }

        /// <summary>
        /// Enumerates generic shared tag definition entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamSharedTagDefinition" /> entities.</value>
        new IEnumerable<IUpstreamSharedTagDefinition> SharedTagDefinitions { get; }

        /// <summary>
        /// Enumerates generic shared file tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamSharedFileTag" /> entities.</value>
        new IEnumerable<IUpstreamSharedFileTag> SharedFileTags { get; }

        /// <summary>
        /// Enumerates generic shared subdirectory tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamSharedSubdirectoryTag" /> entities.</value>
        new IEnumerable<IUpstreamSharedSubdirectoryTag> SharedSubdirectoryTags { get; }

        /// <summary>
        /// Enumerates generic shared volume tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IUpstreamSharedVolumeTag" /> entities.</value>
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

        /// <summary>
        /// Enumerates entites from the file system listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="FileSystems"/> joined with <see cref="SymbolicNames"/> and <see cref="Volumes"/>.</value>
        new IEnumerable<IUpstreamFileSystemListItem> FileSystemListing { get; }

        /// <summary>
        /// Enumerates entites from the personal tag definition listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PersonalTagDefinitions"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="Volumes"/>.</value>
        new IEnumerable<IUpstreamTagDefinitionListItem> PersonalTagDefinitionListing { get; }

        /// <summary>
        /// Enumerates entites from the shared tag definition listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SharedTagDefinitions"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="Volumes"/>.</value>
        new IEnumerable<IUpstreamTagDefinitionListItem> SharedTagDefinitionListing { get; }

        /// <summary>
        /// Enumerates entites from the redundant set listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="RedundantSets"/> joined with <see cref="BinaryPropertySets"/> and <see cref="Redundancies"/>.</value>
        new IEnumerable<IUpstreamRedundantSetListItem> RedundantSetListing { get; }

        /// <summary>
        /// Enumerates entites from the volume listing view intended for display listings of a specific file system.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="Volumes"/> joined with <see cref="Subdirectories"/>, <see cref="VolumeAccessErrors"/>, <see cref="SharedVolumeTags"/>
        /// and <see cref="PersonalVolumeTags"/>.
        /// </value>
        new IEnumerable<IUpstreamVolumeListItem> VolumeListing { get; }

        /// <summary>
        /// Enumerates entites from the volume listing view.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="Volumes"/> joined with <see cref="FileSystems"/>, <see cref="Subdirectories"/>, <see cref="VolumeAccessErrors"/>,
        /// <see cref="SharedVolumeTags"/> and <see cref="PersonalVolumeTags"/>.
        /// </value>

        new IEnumerable<IUpstreamVolumeListItemWithFileSystem> VolumeListingWithFileSystem { get; }

        /// <summary>
        /// Enumerates entites from the subdirectory listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Subdirectories"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="CrawlConfigurations"/>.</value>
        new IEnumerable<IUpstreamSubdirectoryListItem> SubdirectoryListing { get; }

        /// <summary>
        /// Enumerates entites from the subdirectory listing view that includes ancestor path element names.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Subdirectories"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="CrawlConfigurations"/>.</value>
        new IEnumerable<IUpstreamSubdirectoryListItemWithAncestorNames> SubdirectoryListingWithAncestorNames { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes ancestor path element names.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Files"/> joined with <see cref="Subdirectories"/>.</value>
        new IEnumerable<IUpstreamFileListItemWithAncestorNames> FileListingWithAncestorNames { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes size and hash.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Files"/> joined with <see cref="BinaryPropertySets"/>.</value>
        new IEnumerable<IUpstreamFileListItemWithBinaryProperties> FileListingWithBinaryProperties { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes size, hash and ancestor path element names.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Files"/> joined with <see cref="Subdirectories"/> and <see cref="BinaryPropertySets"/>.</value>
        new IEnumerable<IUpstreamFileListItemWithBinaryPropertiesAndAncestorNames> FileListingWithBinaryPropertiesAndAncestorNames { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes size and hash.
        /// </summary>
        /// <value>Result entities from a view of <see cref="CrawlConfigurations"/> joined with <see cref="Subdirectories"/>, <see cref="Volumes"/>,  and <see cref="FileSystems"/>.</value>
        new IEnumerable<IUpstreamCrawlConfigurationListItem> CrawlConfigListing { get; }

        /// <summary>
        /// Enumerates entities representing crawl configuration result report rows.
        /// </summary>
        /// <value>Result entities from a view of <see cref="CrawlConfigurations"/> joined with <see cref="Subdirectories"/>, <see cref="Volumes"/>,  and <see cref="FileSystems"/>.</value>
        new IEnumerable<IUpstreamCrawlConfigReportItem> CrawlConfigReport { get; }

        /// <summary>
        /// Enumerates entites from the crawl job logs view that includes configuration information.
        /// </summary>
        /// <value>Result entities from a view of <see cref="CrawlJobLogs"/> joined with <see cref="CrawlConfigurations"/>.</value>
        new IEnumerable<IUpstreamCrawlJobListItem> CrawlJobListing { get; }

        /// <summary>
        /// Enumerates entites from the summary properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SummaryPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<IUpstreamSummaryPropertiesListItem> SummaryPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the document properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="DocumentPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<IUpstreamDocumentPropertiesListItem> DocumentPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the audio properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="AudioPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<IUpstreamAudioPropertiesListItem> AudioPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the DRM properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="DRMPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<IUpstreamDRMPropertiesListItem> DRMPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the GPS properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="GPSPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<IUpstreamGPSPropertiesListItem> GPSPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the image properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="ImagePropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<IUpstreamImagePropertiesListItem> ImagePropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the media properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="MediaPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<IUpstreamMediaPropertiesListItem> MediaPropertiesListing { get; }


        /// <summary>
        /// Enumerates entites from the music properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="MusicPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<IUpstreamMusicPropertiesListItem> MusicPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the photo properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PhotoPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<IUpstreamPhotoPropertiesListItem> PhotoPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the recorded TV properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="RecordedTVPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<IUpstreamRecordedTVPropertiesListItem> RecordedTVPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the video properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="VideoPropertySets" /> joined with <see cref="Files" />.</value>

        new IEnumerable<IUpstreamVideoPropertiesListItem> VideoPropertiesListing { get; }

#pragma warning disable CS1591
        // TODO: Document IUpstreamDbContext members
        IEnumerable<IGroupMemberOfListItem> GroupMemberOfListing { get; }

        IEnumerable<IGroupMemberListItem> GroupMemberListing { get; }

        IEnumerable<IHostDeviceListItem> HostDeviceListing { get; }

        IEnumerable<IHostPlatformListItem> HostPlatformListing { get; }

        IEnumerable<IMitigationTaskListItem> MitigationTaskListing { get; }

        IEnumerable<IUserGroupListItem> UserGroupListing { get; }

        IEnumerable<IUserProfileListItem> UserProfileListing { get; }
#pragma warning restore CS1591

        /// <summary>
        /// Enumerates entites from the personal volume tags listing view that includes volume and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PersonalVolumeTags" /> joined with <see cref="Volumes" /> and <see cref="PersonalTagDefinitions" />.</value>
        new IEnumerable<IUpstreamItemTagListItem> PersonalVolumeTagListing { get; }

        /// <summary>
        /// Enumerates entites from the shared volume tags listing view that includes volume and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SharedVolumeTags" /> joined with <see cref="Volumes" /> and <see cref="SharedTagDefinitions" />.</value>
        new IEnumerable<IUpstreamItemTagListItem> SharedVolumeTagListing { get; }

        /// <summary>
        /// Enumerates entites from the personal subdirectory tags listing view that includes subdirectory and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PersonalSubdirectoryTags" /> joined with <see cref="Subdirectories" /> and <see cref="PersonalTagDefinitions" />.</value>
        new IEnumerable<IUpstreamItemTagListItem> PersonalSubdirectoryTagListing { get; }

        /// <summary>
        /// Enumerates entites from the shared subdirectory tags listing view that includes file and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SharedSubdirectoryTags" /> joined with <see cref="Subdirectories" /> and <see cref="SharedTagDefinitions" />.</value>
        new IEnumerable<IUpstreamItemTagListItem> SharedSubdirectoryTagListing { get; }

        /// <summary>
        /// Enumerates entites from the personal file tags listing view that includes file and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PersonalFileTags" /> joined with <see cref="Files" /> and <see cref="PersonalTagDefinitions" />.</value>
        new IEnumerable<IUpstreamItemTagListItem> PersonalFileTagListing { get; }

        /// <summary>
        /// Enumerates entites from the shared file tag listing view that includes file and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SharedFileTags" /> joined with <see cref="Files" /> and <see cref="SharedTagDefinitions" />.</value>
        new IEnumerable<IUpstreamItemTagListItem> SharedFileTagListing { get; }

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
