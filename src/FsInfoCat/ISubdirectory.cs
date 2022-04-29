using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Interface for entities that represent a subdirectory node within a file system.</summary>
    /// <seealso cref="IDbFsItem" />
    public interface ISubdirectory : ISubdirectoryRow, IDbFsItem, IEquatable<ISubdirectory>
    {
        /// <summary>Gets the parent subdirectory of the current file system item.</summary>
        /// <value>The parent <see cref="ISubdirectory" /> of the current file system item or <see langword="null" /> if this is the root subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Parent), ResourceType = typeof(Properties.Resources))]
        new ISubdirectory Parent { get; }

        /// <summary>Gets the parent volume.</summary>
        /// <value>The parent volume (if this is the root subdirectory or <see langword="null" /> if this is a subdirectory.</value>
        /// <remarks>If this is <see langword="null" />, then <see cref="Parent" /> should not be null, and vice-versa.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volume), ResourceType = typeof(Properties.Resources))]
        IVolume Volume { get; }

        /// <summary>Gets the crawl configuration that starts with the current subdirectory.</summary>
        /// <value>The crawl configuration that starts with the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlConfiguration), ResourceType = typeof(Properties.Resources))]
        ICrawlConfiguration CrawlConfiguration { get; }

        /// <summary>Gets the files directly contained within this subdirectory.</summary>
        /// <value>The files directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IFile> Files { get; }

        /// <summary>Gets the nested subdirectories directly contained within this subdirectory.</summary>
        /// <value>The nested subdirectories directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SubDirectories), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ISubdirectory> SubDirectories { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ISubdirectoryAccessError> AccessErrors { get; }

        new IEnumerable<IPersonalSubdirectoryTag> PersonalTags { get; }

        new IEnumerable<ISharedSubdirectoryTag> SharedTags { get; }

        bool TryGetVolumeId(out Guid volumeId);
    }
}
