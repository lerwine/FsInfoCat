using FsInfoCat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Interface for entities that represent a subdirectory node within a file system.
    /// </summary>
    /// <seealso cref="IUpstreamDbFsItem" />
    /// <seealso cref="ISubdirectory" />
    /// <seealso cref="IEquatable{IUpstreamSubdirectory}" />
    /// <seealso cref="Local.Model.ILocalSubdirectory" />
    public interface IUpstreamSubdirectory : IUpstreamDbFsItem, ISubdirectory, IEquatable<IUpstreamSubdirectory>
    {
        /// <summary>
        /// Gets the parent volume.
        /// </summary>
        /// <value>The parent volume (if this is the root subdirectory or <see langword="null" /> if this is a subdirectory.</value>
        /// <remarks>If this is <see langword="null" />, then <see cref="IUpstreamDbFsItem.Parent" /> should not be null, and vice-versa.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volume), ResourceType = typeof(Properties.Resources))]
        new IUpstreamVolume Volume { get; }

        /// <summary>
        /// Gets the crawl configuration that starts with the current subdirectory.
        /// </summary>
        /// <value>The crawl configuration that starts with the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlConfiguration), ResourceType = typeof(Properties.Resources))]
        new IUpstreamCrawlConfiguration CrawlConfiguration { get; }

        /// <summary>
        /// Gets the files directly contained within this subdirectory.
        /// </summary>
        /// <value>The files directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamFile> Files { get; }

        /// <summary>
        /// Gets the nested subdirectories directly contained within this subdirectory.
        /// </summary>
        /// <value>The nested subdirectories directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SubDirectories), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamSubdirectory> SubDirectories { get; }

        /// <summary>
        /// Gets the access errors for the current file system item.
        /// </summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamSubdirectoryAccessError> AccessErrors { get; }

        /// <summary>
        /// Gets the personal tags associated with the current subdirectory.
        /// </summary>
        /// <value>The <see cref="IUpstreamPersonalSubdirectoryTag"/> entities that associate <see cref="IUpstreamPersonalTagDefinition"/> entities with the current
        /// subdirectory.</value>
        new IEnumerable<IUpstreamPersonalSubdirectoryTag> PersonalTags { get; }

        /// <summary>
        /// Gets the shared tags associated with the current subdirectory.
        /// </summary>
        /// <value>The <see cref="IUpstreamSharedSubdirectoryTag"/> entities that associate <see cref="IUpstreamSharedTagDefinition"/> entities with the current
        /// subdirectory.</value>
        new IEnumerable<IUpstreamSharedSubdirectoryTag> SharedTags { get; }
    }
}
