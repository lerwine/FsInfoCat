using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for FS InfoCat database context.
    /// </summary>
    /// <seealso cref="Local.Model.ILocalDbContext"/>
    /// <seealso cref="Upstream.Model.IUpstreamDbContext"/>
    /// <seealso cref="BaseDbContext"/>
    /// <seealso cref="IAudioPropertiesListItem" />
    /// <seealso cref="IAudioPropertySet" />
    /// <seealso cref="IBinaryPropertySet" />
    /// <seealso cref="IComparison" />
    /// <seealso cref="ICrawlConfigReportItem" />
    /// <seealso cref="ICrawlConfiguration" />
    /// <seealso cref="ICrawlConfigurationListItem" />
    /// <seealso cref="ICrawlJobListItem" />
    /// <seealso cref="ICrawlJobLog" />
    /// <seealso cref="IDocumentPropertiesListItem" />
    /// <seealso cref="IDocumentPropertySet" />
    /// <seealso cref="IDRMPropertiesListItem" />
    /// <seealso cref="IDRMPropertySet" />
    /// <seealso cref="IFile" />
    /// <seealso cref="IFileAccessError" />
    /// <seealso cref="IFileListItemWithAncestorNames" />
    /// <seealso cref="IFileListItemWithBinaryProperties" />
    /// <seealso cref="IFileListItemWithBinaryPropertiesAndAncestorNames" />
    /// <seealso cref="IFileSystem" />
    /// <seealso cref="IFileSystemListItem" />
    /// <seealso cref="IGPSPropertiesListItem" />
    /// <seealso cref="IGPSPropertySet" />
    /// <seealso cref="IImagePropertiesListItem" />
    /// <seealso cref="IImagePropertySet" />
    /// <seealso cref="IMediaPropertiesListItem" />
    /// <seealso cref="IMediaPropertySet" />
    /// <seealso cref="IMusicPropertiesListItem" />
    /// <seealso cref="IMusicPropertySet" />
    /// <seealso cref="IPersonalFileTag" />
    /// <seealso cref="IPersonalSubdirectoryTag" />
    /// <seealso cref="IPersonalTagDefinition" />
    /// <seealso cref="IPersonalVolumeTag" />
    /// <seealso cref="IPhotoPropertiesListItem" />
    /// <seealso cref="IPhotoPropertySet" />
    /// <seealso cref="IRecordedTVPropertiesListItem" />
    /// <seealso cref="IRecordedTVPropertySet" />
    /// <seealso cref="IRedundancy" />
    /// <seealso cref="IRedundantSet" />
    /// <seealso cref="IRedundantSetListItem" />
    /// <seealso cref="ISharedFileTag" />
    /// <seealso cref="ISharedSubdirectoryTag" />
    /// <seealso cref="ISharedTagDefinition" />
    /// <seealso cref="ISharedVolumeTag" />
    /// <seealso cref="ISubdirectory" />
    /// <seealso cref="ISubdirectoryAccessError" />
    /// <seealso cref="ISubdirectoryAncestorName" />
    /// <seealso cref="ISubdirectoryListItem" />
    /// <seealso cref="ISubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="ISummaryPropertiesListItem" />
    /// <seealso cref="ISummaryPropertySet" />
    /// <seealso cref="ISymbolicName" />
    /// <seealso cref="ISymbolicNameListItem" />
    /// <seealso cref="IVideoPropertiesListItem" />
    /// <seealso cref="IVideoPropertySet" />
    /// <seealso cref="IVolume" />
    /// <seealso cref="IVolumeAccessError" />
    /// <seealso cref="IVolumeListItem" />
    /// <seealso cref="IVolumeListItemWithFileSystem" />
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Enumerates generic file comparison entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IComparison" /> entities.</value>
        IEnumerable<IComparison> Comparisons { get; }

        /// <summary>
        /// Enumerates generic binary property set entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IBinaryPropertySet" /> entities.</value>
        IEnumerable<IBinaryPropertySet> BinaryPropertySets { get; }

        /// <summary>
        /// Enumerates generic summary property set entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ISummaryPropertySet" /> entities.</value>
        IEnumerable<ISummaryPropertySet> SummaryPropertySets { get; }

        /// <summary>
        /// Enumerates generic document property set entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IDocumentPropertySet" /> entities.</value>
        IEnumerable<IDocumentPropertySet> DocumentPropertySets { get; }

        /// <summary>
        /// Enumerates generic audio property set entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IAudioPropertySet" /> entities.</value>
        IEnumerable<IAudioPropertySet> AudioPropertySets { get; }

        /// <summary>
        /// Enumerates generic DRM property set entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IDRMPropertySet" /> entities.</value>
        IEnumerable<IDRMPropertySet> DRMPropertySets { get; }

        /// <summary>
        /// Enumerates generic GPS property set entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IGPSPropertySet" /> entities.</value>
        IEnumerable<IGPSPropertySet> GPSPropertySets { get; }

        /// <summary>
        /// Enumerates generic image property set entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IImagePropertySet" /> entities.</value>
        IEnumerable<IImagePropertySet> ImagePropertySets { get; }

        /// <summary>
        /// Enumerates generic media property set entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IMediaPropertySet" /> entities.</value>
        IEnumerable<IMediaPropertySet> MediaPropertySets { get; }

        /// <summary>
        /// Enumerates generic music property set entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IMusicPropertySet" /> entities.</value>
        IEnumerable<IMusicPropertySet> MusicPropertySets { get; }

        /// <summary>
        /// Enumerates generic photo property set entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IPhotoPropertySet" /> entities.</value>
        IEnumerable<IPhotoPropertySet> PhotoPropertySets { get; }

        /// <summary>
        /// Enumerates generic recorded TV property set entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IRecordedTVPropertySet" /> entities.</value>
        IEnumerable<IRecordedTVPropertySet> RecordedTVPropertySets { get; }

        /// <summary>
        /// Enumerates generic video property set entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IVideoPropertySet" /> entities.</value>
        IEnumerable<IVideoPropertySet> VideoPropertySets { get; }

        /// <summary>
        /// Enumerates generic file access error entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IFileAccessError" /> entities.</value>
        IEnumerable<IFileAccessError> FileAccessErrors { get; }

        /// <summary>
        /// Enumerates generic file entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IFile" /> entities.</value>
        IEnumerable<IFile> Files { get; }

        /// <summary>
        /// Enumerates generic file system entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IFileSystem" /> entities.</value>
        IEnumerable<IFileSystem> FileSystems { get; }

        /// <summary>
        /// Enumerates generic redundancy entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IRedundancy" /> entities.</value>
        IEnumerable<IRedundancy> Redundancies { get; }

        /// <summary>
        /// Enumerates generic redundant set entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IRedundantSet" /> entities.</value>
        IEnumerable<IRedundantSet> RedundantSets { get; }

        /// <summary>
        /// Enumerates generic sub-directory entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ISubdirectory" /> entities.</value>
        IEnumerable<ISubdirectory> Subdirectories { get; }

        /// <summary>
        /// Enumerates generic sub-directory access error entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ISubdirectoryAccessError" /> entities.</value>
        IEnumerable<ISubdirectoryAccessError> SubdirectoryAccessErrors { get; }

        /// <summary>
        /// Enumerates generic symbolic name entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ISymbolicName" /> entities.</value>
        IEnumerable<ISymbolicName> SymbolicNames { get; }

        /// <summary>
        /// Enumerates generic volume access error entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IVolumeAccessError" /> entities.</value>
        IEnumerable<IVolumeAccessError> VolumeAccessErrors { get; }

        /// <summary>
        /// Enumerates generic volume entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IVolume" /> entities.</value>
        IEnumerable<IVolume> Volumes { get; }

        /// <summary>
        /// Enumerates generic personal tag definition entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IPersonalTagDefinition" /> entities.</value>
        IEnumerable<IPersonalTagDefinition> PersonalTagDefinitions { get; }

        /// <summary>
        /// Enumerates generic personal file tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IPersonalFileTag" /> entities.</value>
        IEnumerable<IPersonalFileTag> PersonalFileTags { get; }

        /// <summary>
        /// Enumerates generic personal subdirectory tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IPersonalSubdirectoryTag" /> entities.</value>
        IEnumerable<IPersonalSubdirectoryTag> PersonalSubdirectoryTags { get; }

        /// <summary>
        /// Enumerates generic personal volume tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="IPersonalVolumeTag" /> entities.</value>
        IEnumerable<IPersonalVolumeTag> PersonalVolumeTags { get; }

        /// <summary>
        /// Enumerates generic shared tag definition entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ISharedTagDefinition" /> entities.</value>
        IEnumerable<ISharedTagDefinition> SharedTagDefinitions { get; }

        /// <summary>
        /// Enumerates generic shared file tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ISharedFileTag" /> entities.</value>
        IEnumerable<ISharedFileTag> SharedFileTags { get; }

        /// <summary>
        /// Enumerates generic shared subdirectory tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ISharedSubdirectoryTag" /> entities.</value>
        IEnumerable<ISharedSubdirectoryTag> SharedSubdirectoryTags { get; }

        /// <summary>
        /// Enumerates generic shared volume tag entities.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}" /> that enumerates <see cref="ISharedVolumeTag" /> entities.</value>
        IEnumerable<ISharedVolumeTag> SharedVolumeTags { get; }

        /// <summary>
        /// Enumerates generic crawl configuration entities.
        /// </summary>
        IEnumerable<ICrawlConfiguration> CrawlConfigurations { get; }

        /// <summary>
        /// Enumerates generic crawl log entities from the upstream (remote) database.
        /// </summary>
        IEnumerable<ICrawlJobLog> CrawlJobLogs { get; }

        /// <summary>
        /// Enumerates entites from the symblic name listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SymbolicNames"/> joined with <see cref="FileSystems"/>.</value>
        IEnumerable<ISymbolicNameListItem> SymbolicNameListing { get; }

        /// <summary>
        /// Enumerates entites from the file system listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="FileSystems"/> joined with <see cref="SymbolicNames"/> and <see cref="Volumes"/>.</value>
        IEnumerable<IFileSystemListItem> FileSystemListing { get; }

        /// <summary>
        /// Enumerates entites from the personal tag definition listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PersonalTagDefinitions"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="Volumes"/>.</value>
        IEnumerable<ITagDefinitionListItem> PersonalTagDefinitionListing { get; }

        /// <summary>
        /// Enumerates entites from the shared tag definition listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SharedTagDefinitions"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="Volumes"/>.</value>
        IEnumerable<ITagDefinitionListItem> SharedTagDefinitionListing { get; }

        /// <summary>
        /// Enumerates entites from the redundant set listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="RedundantSets"/> joined with <see cref="BinaryPropertySets"/> and <see cref="Redundancies"/>.</value>
        IEnumerable<IRedundantSetListItem> RedundantSetListing { get; }

        /// <summary>
        /// Enumerates entites from the volume listing view intended for display listings of a specific file system.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="Volumes"/> joined with <see cref="Subdirectories"/>, <see cref="VolumeAccessErrors"/>, <see cref="SharedVolumeTags"/>
        /// and <see cref="PersonalVolumeTags"/>.
        /// </value>
        IEnumerable<IVolumeListItem> VolumeListing { get; }

        /// <summary>
        /// Enumerates entites from the volume listing view.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="Volumes"/> joined with <see cref="FileSystems"/>, <see cref="Subdirectories"/>, <see cref="VolumeAccessErrors"/>,
        /// <see cref="SharedVolumeTags"/> and <see cref="PersonalVolumeTags"/>.
        /// </value>
        IEnumerable<IVolumeListItemWithFileSystem> VolumeListingWithFileSystem { get; }

        /// <summary>
        /// Enumerates entites from the subdirectory listing view.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Subdirectories"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="CrawlConfigurations"/>.</value>
        IEnumerable<ISubdirectoryListItem> SubdirectoryListing { get; }

        /// <summary>
        /// Enumerates entites from the subdirectory listing view that includes ancestor path element names.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Subdirectories"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="CrawlConfigurations"/>.</value>
        IEnumerable<ISubdirectoryListItemWithAncestorNames> SubdirectoryListingWithAncestorNames { get; }

        /// <summary>
        /// Enumerates entites from a simplified subdirectory listing view that includes ancestor path element names.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Subdirectories"/> joined with <see cref="Subdirectories"/> and <see cref="CrawlConfigurations"/>.</value>
        IEnumerable<ISubdirectoryAncestorName> SubdirectoryAncestorNames { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes ancestor path element names.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Files"/> joined with <see cref="Subdirectories"/>.</value>
        IEnumerable<IFileListItemWithAncestorNames> FileListingWithAncestorNames { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes size and hash.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Files"/> joined with <see cref="BinaryPropertySets"/>.</value>
        IEnumerable<IFileListItemWithBinaryProperties> FileListingWithBinaryProperties { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes size, hash and ancestor path element names.
        /// </summary>
        /// <value>Result entities from a view of <see cref="Files"/> joined with <see cref="Subdirectories"/> and <see cref="BinaryPropertySets"/>.</value>
        IEnumerable<IFileListItemWithBinaryPropertiesAndAncestorNames> FileListingWithBinaryPropertiesAndAncestorNames { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes size and hash.
        /// </summary>
        /// <value>Result entities from a view of <see cref="CrawlConfigurations"/> joined with <see cref="Subdirectories"/>, <see cref="Volumes"/>,  and <see cref="FileSystems"/>.</value>
        IEnumerable<ICrawlConfigurationListItem> CrawlConfigListing { get; }

        /// <summary>
        /// Enumerates entities representing crawl configuration result report rows.
        /// </summary>
        /// <value>Result entities from a view of <see cref="CrawlConfigurations"/> joined with <see cref="Subdirectories"/>, <see cref="Volumes"/>,  and <see cref="FileSystems"/>.</value>
        IEnumerable<ICrawlConfigReportItem> CrawlConfigReport { get; }

        /// <summary>
        /// Enumerates entites from the crawl job logs view that includes configuration information.
        /// </summary>
        /// <value>Result entities from a view of <see cref="CrawlJobLogs"/> joined with <see cref="CrawlConfigurations"/>.</value>
        IEnumerable<ICrawlJobListItem> CrawlJobListing { get; }

        /// <summary>
        /// Enumerates entites from the summary properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SummaryPropertySets" /> joined with <see cref="Files" />.</value>
        IEnumerable<ISummaryPropertiesListItem> SummaryPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the document properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="DocumentPropertySets" /> joined with <see cref="Files" />.</value>
        IEnumerable<IDocumentPropertiesListItem> DocumentPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the audio properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="AudioPropertySets" /> joined with <see cref="Files" />.</value>
        IEnumerable<IAudioPropertiesListItem> AudioPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the DRM properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="DRMPropertySets" /> joined with <see cref="Files" />.</value>
        IEnumerable<IDRMPropertiesListItem> DRMPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the GPS properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="GPSPropertySets" /> joined with <see cref="Files" />.</value>
        IEnumerable<IGPSPropertiesListItem> GPSPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the image properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="ImagePropertySets" /> joined with <see cref="Files" />.</value>
        IEnumerable<IImagePropertiesListItem> ImagePropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the media properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="MediaPropertySets" /> joined with <see cref="Files" />.</value>
        IEnumerable<IMediaPropertiesListItem> MediaPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the music properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="MusicPropertySets" /> joined with <see cref="Files" />.</value>
        IEnumerable<IMusicPropertiesListItem> MusicPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the photo properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PhotoPropertySets" /> joined with <see cref="Files" />.</value>
        IEnumerable<IPhotoPropertiesListItem> PhotoPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the recorded TV properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="RecordedTVPropertySets" /> joined with <see cref="Files" />.</value>
        IEnumerable<IRecordedTVPropertiesListItem> RecordedTVPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the video properties listing view that includes file counts.
        /// </summary>
        /// <value>Result entities from a view of <see cref="VideoPropertySets" /> joined with <see cref="Files" />.</value>
        IEnumerable<IVideoPropertiesListItem> VideoPropertiesListing { get; }

        /// <summary>
        /// Enumerates entites from the personal volume tags listing view that includes volume and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PersonalVolumeTags" /> joined with <see cref="Volumes" /> and <see cref="PersonalTagDefinitions" />.</value>
        IEnumerable<IItemTagListItem> PersonalVolumeTagListing { get; }

        /// <summary>
        /// Enumerates entites from the shared volume tags listing view that includes volume and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SharedVolumeTags" /> joined with <see cref="Volumes" /> and <see cref="SharedTagDefinitions" />.</value>
        IEnumerable<IItemTagListItem> SharedVolumeTagListing { get; }

        /// <summary>
        /// Enumerates entites from the personal subdirectory tags listing view that includes subdirectory and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PersonalSubdirectoryTags" /> joined with <see cref="Subdirectories" /> and <see cref="PersonalTagDefinitions" />.</value>
        IEnumerable<IItemTagListItem> PersonalSubdirectoryTagListing { get; }

        /// <summary>
        /// Enumerates entites from the shared subdirectory tags listing view that includes file and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SharedSubdirectoryTags" /> joined with <see cref="Subdirectories" /> and <see cref="SharedTagDefinitions" />.</value>
        IEnumerable<IItemTagListItem> SharedSubdirectoryTagListing { get; }

        /// <summary>
        /// Enumerates entites from the personal file tags listing view that includes file and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="PersonalFileTags" /> joined with <see cref="Files" /> and <see cref="PersonalTagDefinitions" />.</value>
        IEnumerable<IItemTagListItem> PersonalFileTagListing { get; }

        /// <summary>
        /// Enumerates entites from the shared file tag listing view that includes file and tag properties.
        /// </summary>
        /// <value>Result entities from a view of <see cref="SharedFileTags" /> joined with <see cref="Files" /> and <see cref="SharedTagDefinitions" />.</value>
        IEnumerable<IItemTagListItem> SharedFileTagListing { get; }

        /// <summary>
        /// Finds the generic <see cref="ISummaryPropertySet"/> that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="ISummaryProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="ISummaryPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        Task<ISummaryPropertySet> FindMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IDocumentPropertySet"/> that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IDocumentProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IDocumentPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        Task<IDocumentPropertySet> FindMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IAudioPropertySet"/> that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IAudioProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IAudioPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        Task<IAudioPropertySet> FindMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IDRMPropertySet"/> that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IDRMProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IDRMPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        Task<IDRMPropertySet> FindMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IGPSPropertySet"/> that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IGPSProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IGPSPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        Task<IGPSPropertySet> FindMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IImagePropertySet"/> that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IImageProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IImagePropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        Task<IImagePropertySet> FindMatchingAsync(IImageProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IMediaPropertySet"/> that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IMediaProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IMediaPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        Task<IMediaPropertySet> FindMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IMusicPropertySet"/> that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IMusicProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IMusicPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        Task<IMusicPropertySet> FindMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IPhotoPropertySet"/> that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IPhotoProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IPhotoPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        Task<IPhotoPropertySet> FindMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IRecordedTVPropertySet"/> that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IRecordedTVProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IRecordedTVPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        Task<IRecordedTVPropertySet> FindMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the generic <see cref="IVideoPropertySet"/> that matches the specified summary properties.
        /// </summary>
        /// <param name="properties">The <see cref="IVideoProperties"/> object containing the property values to match.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The generic <see cref="IVideoPropertySet"/> for the matching entity or <see langword="null"/> if no match was found.</returns>
        Task<IVideoPropertySet> FindMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int SaveChanges();

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">if set to <c>true</c> [accept all changes on success].</param>
        /// <returns>System.Int32.</returns>
        int SaveChanges(bool acceptAllChangesOnSuccess);

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">if set to <c>true</c> [accept all changes on success].</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Entries the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>EntityEntry&lt;TEntity&gt;.</returns>
        EntityEntry<TEntity> Entry<TEntity>([DisallowNull] TEntity entity) where TEntity : class;
    }
}
