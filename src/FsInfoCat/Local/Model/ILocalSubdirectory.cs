using FsInfoCat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Interface for entities that represent a subdirectory node within a file system on the local host machine.
    /// </summary>
    /// <seealso cref="ILocalDbFsItem" />
    /// <seealso cref="ISubdirectory" />
    /// <seealso cref="IEquatable{ILocalSubdirectory}" />
    /// <seealso cref="Upstream.Model.IUpstreamSubdirectory" />
    public interface ILocalSubdirectory : ILocalDbFsItem, ISubdirectory, ILocalSubdirectoryRow, IEquatable<ILocalSubdirectory>
    {
        /// <summary>
        /// Gets the parent subdirectory of the current file system item.
        /// </summary>
        /// <value>The parent <see cref="ILocalSubdirectory" /> of the current file system item or <see langword="null" /> if this is the root subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.Parent), ResourceType = typeof(Properties.Resources))]
        new ILocalSubdirectory Parent { get; }

        /// <summary>
        /// Gets the parent volume.
        /// </summary>
        /// <value>The parent volume (if this is the root subdirectory or <see langword="null" /> if this is a subdirectory.</value>
        /// <remarks>If this is <see langword="null" />, then <see cref="Parent" /> should not be null, and vice-versa.</remarks>
        [Display(Name = nameof(Properties.Resources.Volume), ResourceType = typeof(Properties.Resources))]
        new ILocalVolume Volume { get; }

        /// <summary>
        /// Gets the crawl configuration that starts with the current subdirectory.
        /// </summary>
        /// <value>The crawl configuration that starts with the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.CrawlConfiguration), ResourceType = typeof(Properties.Resources))]
        new ILocalCrawlConfiguration CrawlConfiguration { get; }

        /// <summary>
        /// Gets the files directly contained within this subdirectory.
        /// </summary>
        /// <value>The files directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalFile> Files { get; }

        /// <summary>
        /// Gets the nested subdirectories directly contained within this subdirectory.
        /// </summary>
        /// <value>The nested subdirectories directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.SubDirectories), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalSubdirectory> SubDirectories { get; }

        /// <summary>
        /// Gets the access errors for the current subdirectory.
        /// </summary>
        /// <value>The access errors for the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalSubdirectoryAccessError> AccessErrors { get; }

        /// <summary>
        /// Gets the personal tags associated with the current subdirectory.
        /// </summary>
        /// <value>The <see cref="ILocalPersonalSubdirectoryTag"/> entities that associate <see cref="ILocalPersonalTagDefinition"/> entities with the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.PersonalTags), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalPersonalSubdirectoryTag> PersonalTags { get; }

        /// <summary>
        /// Gets the shared tags associated with the current subdirectory.
        /// </summary>
        /// <value>The <see cref="ILocalSharedSubdirectoryTag"/> entities that associate <see cref="ILocalSharedTagDefinition"/> entities with the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.SharedTags), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalSharedSubdirectoryTag> SharedTags { get; }
    }
}
