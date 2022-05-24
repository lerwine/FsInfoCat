using FsInfoCat.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for the local FS InfoCat database context.
    /// Extends the <see cref="IDbContext" /> interface to represent the local database.
    /// </summary>
    /// <seealso cref="IDbContext" />
    /// <seealso cref="Upstream.Model.IUpstreamDbContext" />
    /// <seealso cref="ILocalAudioPropertiesListItem" />
    /// <seealso cref="ILocalAudioPropertySet" />
    /// <seealso cref="ILocalBinaryPropertySet" />
    /// <seealso cref="ILocalComparison" />
    /// <seealso cref="ILocalCrawlConfigReportItem" />
    /// <seealso cref="ILocalCrawlConfiguration" />
    /// <seealso cref="ILocalCrawlConfigurationListItem" />
    /// <seealso cref="ILocalCrawlJobListItem" />
    /// <seealso cref="ILocalCrawlJobLog" />
    /// <seealso cref="ILocalDocumentPropertiesListItem" />
    /// <seealso cref="ILocalDocumentPropertySet" />
    /// <seealso cref="ILocalDRMPropertiesListItem" />
    /// <seealso cref="ILocalDRMPropertySet" />
    /// <seealso cref="ILocalFile" />
    /// <seealso cref="ILocalFileAccessError" />
    /// <seealso cref="ILocalFileListItemWithAncestorNames" />
    /// <seealso cref="ILocalFileListItemWithBinaryProperties" />
    /// <seealso cref="ILocalFileListItemWithBinaryPropertiesAndAncestorNames" />
    /// <seealso cref="ILocalFileSystem" />
    /// <seealso cref="ILocalFileSystemListItem" />
    /// <seealso cref="ILocalGPSPropertiesListItem" />
    /// <seealso cref="ILocalGPSPropertySet" />
    /// <seealso cref="ILocalImagePropertiesListItem" />
    /// <seealso cref="ILocalImagePropertySet" />
    /// <seealso cref="ILocalMediaPropertiesListItem" />
    /// <seealso cref="ILocalMediaPropertySet" />
    /// <seealso cref="ILocalMusicPropertiesListItem" />
    /// <seealso cref="ILocalMusicPropertySet" />
    /// <seealso cref="ILocalPersonalFileTag" />
    /// <seealso cref="ILocalPersonalSubdirectoryTag" />
    /// <seealso cref="ILocalPersonalTagDefinition" />
    /// <seealso cref="ILocalPersonalVolumeTag" />
    /// <seealso cref="ILocalPhotoPropertiesListItem" />
    /// <seealso cref="ILocalPhotoPropertySet" />
    /// <seealso cref="ILocalRecordedTVPropertiesListItem" />
    /// <seealso cref="ILocalRecordedTVPropertySet" />
    /// <seealso cref="ILocalRedundancy" />
    /// <seealso cref="ILocalRedundantSet" />
    /// <seealso cref="ILocalRedundantSetListItem" />
    /// <seealso cref="ILocalSharedFileTag" />
    /// <seealso cref="ILocalSharedSubdirectoryTag" />
    /// <seealso cref="ILocalSharedTagDefinition" />
    /// <seealso cref="ILocalSharedVolumeTag" />
    /// <seealso cref="ILocalSubdirectory" />
    /// <seealso cref="ILocalSubdirectoryAccessError" />
    /// <seealso cref="ISubdirectoryAncestorName" />
    /// <seealso cref="ILocalSubdirectoryListItem" />
    /// <seealso cref="ILocalSubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="ILocalSummaryPropertiesListItem" />
    /// <seealso cref="ILocalSummaryPropertySet" />
    /// <seealso cref="ILocalSymbolicName" />
    /// <seealso cref="ILocalSymbolicNameListItem" />
    /// <seealso cref="ILocalVideoPropertiesListItem" />
    /// <seealso cref="ILocalVideoPropertySet" />
    /// <seealso cref="ILocalVolume" />
    /// <seealso cref="ILocalVolumeAccessError" />
    /// <seealso cref="ILocalVolumeListItem" />
    /// <seealso cref="ILocalVolumeListItemWithFileSystem" />
    public interface ILocalDbContext : IDbContext
    {
        /// <summary>
        /// Enumerates generic file comparison entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalComparison" /> entities.</value>
        new IEnumerable<ILocalComparison> Comparisons { get; }

        /// <summary>
        /// Enumerates generic binary property set entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalBinaryPropertySet" /> entities.</value>
        new IEnumerable<ILocalBinaryPropertySet> BinaryPropertySets { get; }

        /// <summary>
        /// Enumerates generic summary property set entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalSummaryPropertySet" /> entities.</value>
        new IEnumerable<ILocalSummaryPropertySet> SummaryPropertySets { get; }

        /// <summary>
        /// Enumerates generic document property set entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalDocumentPropertySet" /> entities.</value>
        new IEnumerable<ILocalDocumentPropertySet> DocumentPropertySets { get; }

        /// <summary>
        /// Enumerates generic audio property set entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalAudioPropertySet" /> entities.</value>
        new IEnumerable<ILocalAudioPropertySet> AudioPropertySets { get; }

        /// <summary>
        /// Enumerates generic DRM property set entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalDRMPropertySet" /> entities.</value>
        new IEnumerable<ILocalDRMPropertySet> DRMPropertySets { get; }

        /// <summary>
        /// Enumerates generic GPS property set entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalGPSPropertySet" /> entities.</value>
        new IEnumerable<ILocalGPSPropertySet> GPSPropertySets { get; }

        /// <summary>
        /// Enumerates generic image property set entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalImagePropertySet" /> entities.</value>
        new IEnumerable<ILocalImagePropertySet> ImagePropertySets { get; }

        /// <summary>
        /// Enumerates generic media property set entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalMediaPropertySet" /> entities.</value>
        new IEnumerable<ILocalMediaPropertySet> MediaPropertySets { get; }

        /// <summary>
        /// Enumerates generic music property set entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalMusicPropertySet" /> entities.</value>
        new IEnumerable<ILocalMusicPropertySet> MusicPropertySets { get; }

        /// <summary>
        /// Enumerates generic photo property set entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalPhotoPropertySet" /> entities.</value>
        new IEnumerable<ILocalPhotoPropertySet> PhotoPropertySets { get; }

        /// <summary>
        /// Enumerates generic recorded TV property set entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalRecordedTVPropertySet" /> entities.</value>
        new IEnumerable<ILocalRecordedTVPropertySet> RecordedTVPropertySets { get; }

        /// <summary>
        /// Enumerates generic video property set entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalVideoPropertySet" /> entities.</value>
        new IEnumerable<ILocalVideoPropertySet> VideoPropertySets { get; }

        /// <summary>
        /// Enumerates generic file access error entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalFileAccessError" /> entities.</value>
        new IEnumerable<ILocalFileAccessError> FileAccessErrors { get; }

        /// <summary>
        /// Enumerates generic file entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalFile" /> entities.</value>
        new IEnumerable<ILocalFile> Files { get; }

        /// <summary>
        /// Enumerates generic file system entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalFileSystem" /> entities.</value>
        new IEnumerable<ILocalFileSystem> FileSystems { get; }

        /// <summary>
        /// Enumerates generic redundancy entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalRedundancy" /> entities.</value>
        new IEnumerable<ILocalRedundancy> Redundancies { get; }

        /// <summary>
        /// Enumerates generic redundant set entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalRedundantSet" /> entities.</value>
        new IEnumerable<ILocalRedundantSet> RedundantSets { get; }

        /// <summary>
        /// Enumerates generic sub-directory entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalSubdirectory" /> entities.</value>
        new IEnumerable<ILocalSubdirectory> Subdirectories { get; }

        /// <summary>
        /// Enumerates generic sub-directory access error entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalSubdirectoryAccessError" /> entities.</value>
        new IEnumerable<ILocalSubdirectoryAccessError> SubdirectoryAccessErrors { get; }

        /// <summary>
        /// Enumerates generic symbolic name entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalSymbolicName" /> entities.</value>
        new IEnumerable<ILocalSymbolicName> SymbolicNames { get; }

        /// <summary>
        /// Enumerates generic volume access error entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalVolumeAccessError" /> entities.</value>
        new IEnumerable<ILocalVolumeAccessError> VolumeAccessErrors { get; }

        /// <summary>
        /// Enumerates generic volume entities from the local database.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalVolume" /> entities.</value>
        new IEnumerable<ILocalVolume> Volumes { get; }

        /// <summary>
        /// Enumerates generic personal tag definition entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalPersonalTagDefinition" /> entities.</value>
        new IEnumerable<ILocalPersonalTagDefinition> PersonalTagDefinitions { get; }

        /// <summary>
        /// Enumerates generic personal file tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalPersonalFileTag" /> entities.</value>
        new IEnumerable<ILocalPersonalFileTag> PersonalFileTags { get; }

        /// <summary>
        /// Enumerates generic personal subdirectory tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalPersonalSubdirectoryTag" /> entities.</value>
        new IEnumerable<ILocalPersonalSubdirectoryTag> PersonalSubdirectoryTags { get; }

        /// <summary>
        /// Enumerates generic personal volume tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalPersonalVolumeTag" /> entities.</value>
        new IEnumerable<ILocalPersonalVolumeTag> PersonalVolumeTags { get; }

        /// <summary>
        /// Enumerates generic shared tag definition entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalSharedTagDefinition" /> entities.</value>
        new IEnumerable<ILocalSharedTagDefinition> SharedTagDefinitions { get; }

        /// <summary>
        /// Enumerates generic shared file tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalSharedFileTag" /> entities.</value>
        new IEnumerable<ILocalSharedFileTag> SharedFileTags { get; }

        /// <summary>
        /// Enumerates generic shared subdirectory tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalSharedSubdirectoryTag" /> entities.</value>
        new IEnumerable<ILocalSharedSubdirectoryTag> SharedSubdirectoryTags { get; }

        /// <summary>
        /// Enumerates generic shared volume tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ILocalSharedVolumeTag" /> entities.</value>
        new IEnumerable<ILocalSharedVolumeTag> SharedVolumeTags { get; }

        /// <summary>
        /// Enumerates generic crawl configuration entities from the local database.
        /// </summary>
        new IEnumerable<ILocalCrawlConfiguration> CrawlConfigurations { get; }

        /// <summary>
        /// Enumerates generic crawl log entities from the local database.
        /// </summary>
        new IEnumerable<ILocalCrawlJobLog> CrawlJobLogs { get; }

        /// <summary>
        /// Enumerates entites from the file system listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="FileSystems"/> joined with <see cref="SymbolicNames"/> and <see cref="Volumes"/>.</value>
        new IEnumerable<ILocalFileSystemListItem> FileSystemListing { get; }

        /// <summary>
        /// Enumerates entites from the personal tag definition listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PersonalTagDefinitions"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="Volumes"/>.</value>
        new IEnumerable<ILocalTagDefinitionListItem> PersonalTagDefinitionListing { get; }

        /// <summary>
        /// Enumerates entites from the shared tag definition listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SharedTagDefinitions"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="Volumes"/>.</value>
        new IEnumerable<ILocalTagDefinitionListItem> SharedTagDefinitionListing { get; }

        /// <summary>
        /// Enumerates entites from the redundant set listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="RedundantSets"/> joined with <see cref="BinaryPropertySets"/> and <see cref="Redundancies"/>.</value>
        new IEnumerable<ILocalRedundantSetListItem> RedundantSetListing { get; }

        /// <summary>
        /// Enumerates entites from the volume listing view intended for display listings of a specific file system.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="Volumes"/> joined with <see cref="Subdirectories"/>, <see cref="VolumeAccessErrors"/>, <see cref="SharedVolumeTags"/>
        /// and <see cref="PersonalVolumeTags"/>.
        /// </value>
        new IEnumerable<ILocalVolumeListItem> VolumeListing { get; }

        /// <summary>
        /// Enumerates entites from the volume listing view.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="Volumes"/> joined with <see cref="FileSystems"/>, <see cref="Subdirectories"/>, <see cref="VolumeAccessErrors"/>,
        /// <see cref="SharedVolumeTags"/> and <see cref="PersonalVolumeTags"/>.
        /// </value>
        new IEnumerable<ILocalVolumeListItemWithFileSystem> VolumeListingWithFileSystem { get; }

        /// <summary>
        /// Enumerates entites from the subdirectory listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Subdirectories"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="CrawlConfigurations"/>.</value>
        new IEnumerable<ILocalSubdirectoryListItem> SubdirectoryListing { get; }

        /// <summary>
        /// Enumerates entites from the subdirectory listing view that includes ancestor path element names.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Subdirectories"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="CrawlConfigurations"/>.</value>
        new IEnumerable<ILocalSubdirectoryListItemWithAncestorNames> SubdirectoryListingWithAncestorNames { get; }


        /// <summary>
        /// Enumerates entites from the file listing view that includes ancestor path element names.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Files"/> joined with <see cref="Subdirectories"/>.</value>
        new IEnumerable<ILocalFileListItemWithAncestorNames> FileListingWithAncestorNames { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes size and hash.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Files"/> joined with <see cref="BinaryPropertySets"/>.</value>
        new IEnumerable<ILocalFileListItemWithBinaryProperties> FileListingWithBinaryProperties { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes size, hash and ancestor path element names.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Files"/> joined with <see cref="Subdirectories"/> and <see cref="BinaryPropertySets"/>.</value>
        new IEnumerable<ILocalFileListItemWithBinaryPropertiesAndAncestorNames> FileListingWithBinaryPropertiesAndAncestorNames { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes size and hash.
        /// </summary>
        /// <value>Result entities from a view of <see cref="CrawlConfigurations"/> joined with <see cref="Subdirectories"/>, <see cref="Volumes"/>,  and <see cref="FileSystems"/>.</value>
        new IEnumerable<ILocalCrawlConfigurationListItem> CrawlConfigListing { get; }

        /// <summary>
        /// Enumerates entities representing crawl configuration result report rows.
        /// </summary>
        /// <value>Result entities from a view of <see cref="CrawlConfigurations"/> joined with <see cref="Subdirectories"/>, <see cref="Volumes"/>,  and <see cref="FileSystems"/>.</value>
        new IEnumerable<ILocalCrawlConfigReportItem> CrawlConfigReport { get; }

        /// <summary>
        /// Enumerates entites from the crawl job logs view that includes configuration information.
        /// </summary>
        /// <value>Result entities from a view of <see cref="CrawlJobLogs"/> joined with <see cref="CrawlConfigurations"/>.</value>
        new IEnumerable<ILocalCrawlJobListItem> CrawlJobListing { get; }

        /// <summary>
        /// Enumerates entites from the summary properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SummaryPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<ILocalSummaryPropertiesListItem> SummaryPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the document properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="DocumentPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<ILocalDocumentPropertiesListItem> DocumentPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the audio properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="AudioPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<ILocalAudioPropertiesListItem> AudioPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the DRM properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="DRMPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<ILocalDRMPropertiesListItem> DRMPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the GPS properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="GPSPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<ILocalGPSPropertiesListItem> GPSPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the image properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="ImagePropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<ILocalImagePropertiesListItem> ImagePropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the media properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="MediaPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<ILocalMediaPropertiesListItem> MediaPropertiesListing { get; }


        /// <summary>
        /// Enumerates entites from the music properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="MusicPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<ILocalMusicPropertiesListItem> MusicPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the photo properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PhotoPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<ILocalPhotoPropertiesListItem> PhotoPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the recorded TV properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="RecordedTVPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<ILocalRecordedTVPropertiesListItem> RecordedTVPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the video properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="VideoPropertySets" /> joined with <see cref="Files" />.</value>
        new IEnumerable<ILocalVideoPropertiesListItem> VideoPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the personal volume tags listing view that includes volume and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PersonalVolumeTags" /> joined with <see cref="Volumes" /> and <see cref="PersonalTagDefinitions" />.</value>
        new IEnumerable<ILocalItemTagListItem> PersonalVolumeTagListing { get; }

        /// <summary>
        /// Enumerates entites from the shared volume tags listing view that includes volume and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SharedVolumeTags" /> joined with <see cref="Volumes" /> and <see cref="SharedTagDefinitions" />.</value>
        new IEnumerable<ILocalItemTagListItem> SharedVolumeTagListing { get; }

        /// <summary>
        /// Enumerates entites from the personal subdirectory tags listing view that includes subdirectory and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PersonalSubdirectoryTags" /> joined with <see cref="Subdirectories" /> and <see cref="PersonalTagDefinitions" />.</value>
        new IEnumerable<ILocalItemTagListItem> PersonalSubdirectoryTagListing { get; }

        /// <summary>
        /// Enumerates entites from the shared subdirectory tags listing view that includes file and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SharedSubdirectoryTags" /> joined with <see cref="Subdirectories" /> and <see cref="SharedTagDefinitions" />.</value>
        new IEnumerable<ILocalItemTagListItem> SharedSubdirectoryTagListing { get; }

        /// <summary>
        /// Enumerates entites from the personal file tags listing view that includes file and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PersonalFileTags" /> joined with <see cref="Files" /> and <see cref="PersonalTagDefinitions" />.</value>
        new IEnumerable<ILocalItemTagListItem> PersonalFileTagListing { get; }

        /// <summary>
        /// Enumerates entites from the shared file tag listing view that includes file and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SharedFileTags" /> joined with <see cref="Files" /> and <see cref="SharedTagDefinitions" />.</value>
        new IEnumerable<ILocalItemTagListItem> SharedFileTagListing { get; }

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
