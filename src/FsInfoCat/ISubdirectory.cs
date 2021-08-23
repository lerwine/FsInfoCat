using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface ISubdirectoryRow : IDbFsItemRow
    {
        /// <summary>Gets the crawl options for the current subdirectory.</summary>
        /// <value>The crawl options for the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Options), ResourceType = typeof(Properties.Resources))]
        DirectoryCrawlOptions Options { get; }

        /// <summary>Gets the status of the current subdirectory.</summary>
        /// <value>The status value for the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        DirectoryStatus Status { get; }

        Guid? ParentId { get; }

        Guid? VolumeId { get; }
    }
    public interface ISubdirectoryAncestorName : IDbFsItemAncestorName
    {
        Guid? ParentId { get; }
    }
    public interface ISubdirectoryListItem : IDbFsItemListItem, ISubdirectoryRow
    {
        long SubdirectoryCount { get; }

        long FileCount { get; }

        string CrawlConfigDisplayName { get; }
    }
    public interface ISubdirectoryListItemWithAncestorNames : IDbFsItemListItemWithAncestorNames, ISubdirectoryRow, ISubdirectoryAncestorName { }
    /// <summary>Interface for entities that represent a subdirectory node within a file system.</summary>
    /// <seealso cref="IDbFsItem" />
    public interface ISubdirectory : ISubdirectoryRow, IDbFsItem
    {
        /// <summary>Gets the parent subdirectory of the current file system item.</summary>
        /// <value>The parent <see cref="ISubdirectory" /> of the current file system item or <see langword="null" /> if this is the root subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Parent), ResourceType = typeof(Properties.Resources))]
        new ISubdirectory Parent { get; }

        /// <summary>Gets the parent volume.</summary>
        /// <value>The parent volume (if this is the root subdirectory or <see langword="null" /> if this is a subdirectory.</value>
        /// <remarks>If this is <see langword="null" />, then <see cref="ISubdirectory.Parent" /> should not be null, and vice-versa.</remarks>
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
    }

}

