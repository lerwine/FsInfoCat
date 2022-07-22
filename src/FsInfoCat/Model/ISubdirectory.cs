using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for entities that represent a subdirectory node within a file system.
    /// </summary>
    /// <seealso cref="IDbFsItem" />
    /// <seealso cref="Local.Model.ILocalSubdirectory" />
    /// <seealso cref="Upstream.Model.IUpstreamSubdirectory" />
    /// <seealso cref="IDbFsItem.Parent" />
    /// <seealso cref="ISubdirectoryAccessError.Target" />
    /// <seealso cref="ISubdirectoryTag.Tagged" />
    /// <seealso cref="ICrawlConfiguration.Root" />
    /// <seealso cref="IDbContext.Subdirectories" />
    public interface ISubdirectory : ISubdirectoryRow, IDbFsItem, IEquatable<ISubdirectory>
    {
        /// <summary>
        /// Gets the parent subdirectory of the current file system item.
        /// </summary>
        /// <value>The parent <see cref="ISubdirectory" /> of the current file system item or <see langword="null" /> if this is the root subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.Parent), ResourceType = typeof(Properties.Resources))]
        new ISubdirectory Parent { get; }

        /// <summary>
        /// Gets the parent volume.
        /// </summary>
        /// <value>The parent volume (if this is the root subdirectory or <see langword="null" /> if this is a subdirectory.</value>
        /// <remarks>If this is <see langword="null" />, then <see cref="Parent" /> should not be null, and vice-versa.</remarks>
        [Display(Name = nameof(Properties.Resources.Volume), ResourceType = typeof(Properties.Resources))]
        IVolume Volume { get; }

        /// <summary>
        /// Gets the crawl configuration that starts with the current subdirectory.
        /// </summary>
        /// <value>The crawl configuration that starts with the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.CrawlConfiguration), ResourceType = typeof(Properties.Resources))]
        ICrawlConfiguration CrawlConfiguration { get; }

        /// <summary>
        /// Gets the files directly contained within this subdirectory.
        /// </summary>
        /// <value>The files directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.Files), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IFile> Files { get; }

        /// <summary>
        /// Gets the nested subdirectories directly contained within this subdirectory.
        /// </summary>
        /// <value>The nested subdirectories directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.SubDirectories), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ISubdirectory> SubDirectories { get; }

        /// <summary>
        /// Gets the access errors for the current subdirectory.
        /// </summary>
        /// <value>The access errors for the current subdirectory</value>
        [Display(Name = nameof(Properties.Resources.AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ISubdirectoryAccessError> AccessErrors { get; }

        /// <summary>
        /// Gets the personal tags associated with the current subdirectory.
        /// </summary>
        /// <value>The <see cref="IPersonalSubdirectoryTag"/> entities that associate <see cref="IPersonalTagDefinition"/> entities with the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.PersonalTags), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IPersonalSubdirectoryTag> PersonalTags { get; }

        /// <summary>
        /// Gets the shared tags associated with the current subdirectory.
        /// </summary>
        /// <value>The <see cref="ISharedFileTag"/> entities that associate <see cref="ISharedTagDefinition"/> entities with the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.SharedTags), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ISharedSubdirectoryTag> SharedTags { get; }

        /// <summary>
        /// Attempts to get the primary key of the parent volume.
        /// </summary>
        /// <param name="volumeId">The <see cref="IHasSimpleIdentifier.Id"/> of the associated <see cref="IVolume"/>.</param>
        /// <returns><see langword="true"/> if <see cref="ISubdirectoryRow.VolumeId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetVolumeId(out Guid volumeId);
    }
}
