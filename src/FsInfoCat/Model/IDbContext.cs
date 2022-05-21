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
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Enumerates generic file comparison entities.
        /// </summary>
        IEnumerable<IComparison> Comparisons { get; }

        /// <summary>
        /// Enumerates generic binary property set entities.
        /// </summary>
        IEnumerable<IBinaryPropertySet> BinaryPropertySets { get; }

        /// <summary>
        /// Enumerates generic summary property set entities.
        /// </summary>
        IEnumerable<ISummaryPropertySet> SummaryPropertySets { get; }

        /// <summary>
        /// Enumerates generic document property set entities.
        /// </summary>
        IEnumerable<IDocumentPropertySet> DocumentPropertySets { get; }

        /// <summary>
        /// Enumerates generic audio property set entities.
        /// </summary>
        IEnumerable<IAudioPropertySet> AudioPropertySets { get; }

        /// <summary>
        /// Enumerates generic DRM property set entities.
        /// </summary>
        IEnumerable<IDRMPropertySet> DRMPropertySets { get; }

        /// <summary>
        /// Enumerates generic GPS property set entities.
        /// </summary>
        IEnumerable<IGPSPropertySet> GPSPropertySets { get; }

        /// <summary>
        /// Enumerates generic image property set entities.
        /// </summary>
        IEnumerable<IImagePropertySet> ImagePropertySets { get; }

        /// <summary>
        /// Enumerates generic media property set entities.
        /// </summary>
        IEnumerable<IMediaPropertySet> MediaPropertySets { get; }

        /// <summary>
        /// Enumerates generic music property set entities.
        /// </summary>
        IEnumerable<IMusicPropertySet> MusicPropertySets { get; }

        /// <summary>
        /// Enumerates generic photo property set entities.
        /// </summary>
        IEnumerable<IPhotoPropertySet> PhotoPropertySets { get; }

        /// <summary>
        /// Enumerates generic recorded TV property set entities.
        /// </summary>
        IEnumerable<IRecordedTVPropertySet> RecordedTVPropertySets { get; }

        /// <summary>
        /// Enumerates generic video property set entities.
        /// </summary>
        IEnumerable<IVideoPropertySet> VideoPropertySets { get; }

        /// <summary>
        /// Enumerates generic file access error entities.
        /// </summary>
        IEnumerable<IFileAccessError> FileAccessErrors { get; }

        /// <summary>
        /// Enumerates generic file entities.
        /// </summary>
        IEnumerable<IFile> Files { get; }

        /// <summary>
        /// Enumerates generic file system entities.
        /// </summary>
        IEnumerable<IFileSystem> FileSystems { get; }

        /// <summary>
        /// Enumerates generic redundancy entities.
        /// </summary>
        IEnumerable<IRedundancy> Redundancies { get; }

        /// <summary>
        /// Enumerates generic redundant set entities.
        /// </summary>
        IEnumerable<IRedundantSet> RedundantSets { get; }

        /// <summary>
        /// Enumerates generic sub-directory entities.
        /// </summary>
        IEnumerable<ISubdirectory> Subdirectories { get; }

        /// <summary>
        /// Enumerates generic sub-directory access error entities.
        /// </summary>
        IEnumerable<ISubdirectoryAccessError> SubdirectoryAccessErrors { get; }

        /// <summary>
        /// Enumerates generic symbolic name entities.
        /// </summary>
        IEnumerable<ISymbolicName> SymbolicNames { get; }

        /// <summary>
        /// Enumerates generic volume access error entities.
        /// </summary>
        IEnumerable<IVolumeAccessError> VolumeAccessErrors { get; }

        /// <summary>
        /// Enumerates generic volume entities.
        /// </summary>
        IEnumerable<IVolume> Volumes { get; }

    // TODO: Document IDbContext members
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        IEnumerable<IPersonalTagDefinition> PersonalTagDefinitions { get; }

        IEnumerable<IPersonalFileTag> PersonalFileTags { get; }

        IEnumerable<IPersonalSubdirectoryTag> PersonalSubdirectoryTags { get; }

        IEnumerable<IPersonalVolumeTag> PersonalVolumeTags { get; }

        IEnumerable<ISharedTagDefinition> SharedTagDefinitions { get; }

        IEnumerable<ISharedFileTag> SharedFileTags { get; }

        IEnumerable<ISharedSubdirectoryTag> SharedSubdirectoryTags { get; }

        IEnumerable<ISharedVolumeTag> SharedVolumeTags { get; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

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
        /// <value>
        /// Result entities from a view of <see cref="SymbolicNames"/> joined with <see cref="FileSystems"/>.
        /// </value>
        IEnumerable<ISymbolicNameListItem> SymbolicNameListing { get; }

        /// <summary>
        /// Enumerates entites from the file system listing view.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="FileSystems"/> joined with <see cref="SymbolicNames"/> and <see cref="Volumes"/>.
        /// </value>
        IEnumerable<IFileSystemListItem> FileSystemListing { get; }

        /// <summary>
        /// Enumerates entites from the personal tag definition listing view.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="PersonalTagDefinitions"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="Volumes"/>.
        /// </value>
        IEnumerable<ITagDefinitionListItem> PersonalTagDefinitionListing { get; }

        /// <summary>
        /// Enumerates entites from the shared tag definition listing view.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="SharedTagDefinitions"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="Volumes"/>.
        /// </value>
        IEnumerable<ITagDefinitionListItem> SharedTagDefinitionListing { get; }

        /// <summary>
        /// Enumerates entites from the redundant set listing view.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="RedundantSets"/> joined with <see cref="BinaryPropertySets"/> and <see cref="Redundancies"/>.
        /// </value>
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
        /// <value>
        /// Result entities from a view of <see cref="Subdirectories"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="CrawlConfigurations"/>.
        /// </value>
        IEnumerable<ISubdirectoryListItem> SubdirectoryListing { get; }

        /// <summary>
        /// Enumerates entites from the subdirectory listing view that includes ancestor path element names.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="Subdirectories"/> joined with <see cref="Files"/>, <see cref="Subdirectories"/> and <see cref="CrawlConfigurations"/>.
        /// </value>
        IEnumerable<ISubdirectoryListItemWithAncestorNames> SubdirectoryListingWithAncestorNames { get; }

        /// <summary>
        /// Enumerates entites from a simplified subdirectory listing view that includes ancestor path element names.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="Subdirectories"/> joined with <see cref="Subdirectories"/> and <see cref="CrawlConfigurations"/>.
        /// </value>
        IEnumerable<ISubdirectoryAncestorName> SubdirectoryAncestorNames { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes ancestor path element names.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="Files"/> joined with <see cref="Subdirectories"/>.
        /// </value>
        IEnumerable<IFileListItemWithAncestorNames> FileListingWithAncestorNames { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes size and hash.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="Files"/> joined with <see cref="BinaryPropertySets"/>.
        /// </value>
        IEnumerable<IFileListItemWithBinaryProperties> FileListingWithBinaryProperties { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes size, hash and ancestor path element names.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="Files"/> joined with <see cref="Subdirectories"/> and <see cref="BinaryPropertySets"/>.
        /// </value>
        IEnumerable<IFileListItemWithBinaryPropertiesAndAncestorNames> FileListingWithBinaryPropertiesAndAncestorNames { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes size and hash.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="CrawlConfigurations"/> joined with <see cref="Subdirectories"/>, <see cref="Volumes"/>,  and <see cref="FileSystems"/>.
        /// </value>
        IEnumerable<ICrawlConfigurationListItem> CrawlConfigListing { get; }

    // TODO: Document IDbContext members
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        IEnumerable<ICrawlConfigReportItem> CrawlConfigReport { get; }

        /// <summary>
        /// Enumerates entites from the file listing view that includes size and hash.
        /// </summary>
        /// <value>
        /// Result entities from a view of <see cref="CrawlJobLogs"/> joined with <see cref="CrawlConfigurations"/>.
        /// </value>
        IEnumerable<ICrawlJobListItem> CrawlJobListing { get; }

        IEnumerable<ISummaryPropertiesListItem> SummaryPropertiesListing { get; }

        IEnumerable<IDocumentPropertiesListItem> DocumentPropertiesListing { get; }

        IEnumerable<IAudioPropertiesListItem> AudioPropertiesListing { get; }

        IEnumerable<IDRMPropertiesListItem> DRMPropertiesListing { get; }

        IEnumerable<IGPSPropertiesListItem> GPSPropertiesListing { get; }

        IEnumerable<IImagePropertiesListItem> ImagePropertiesListing { get; }

        IEnumerable<IMediaPropertiesListItem> MediaPropertiesListing { get; }

        IEnumerable<IMusicPropertiesListItem> MusicPropertiesListing { get; }

        IEnumerable<IPhotoPropertiesListItem> PhotoPropertiesListing { get; }

        IEnumerable<IRecordedTVPropertiesListItem> RecordedTVPropertiesListing { get; }

        IEnumerable<IVideoPropertiesListItem> VideoPropertiesListing { get; }

        IEnumerable<IItemTagListItem> PersonalVolumeTagListing { get; }

        IEnumerable<IItemTagListItem> SharedVolumeTagListing { get; }

        IEnumerable<IItemTagListItem> PersonalSubdirectoryTagListing { get; }

        IEnumerable<IItemTagListItem> SharedSubdirectoryTagListing { get; }

        IEnumerable<IItemTagListItem> PersonalFileTagListing { get; }

        IEnumerable<IItemTagListItem> SharedFileTagListing { get; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

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
