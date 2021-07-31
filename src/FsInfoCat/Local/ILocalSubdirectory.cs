using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    /// <summary>Interface for entities that represent a subdirectory node within a file system on the local host machine.</summary>
    /// <seealso cref="ILocalDbFsItem" />
    /// <seealso cref="ISubdirectory" />
    public interface ILocalSubdirectory : ILocalDbFsItem, ISubdirectory
    {
        /// <summary>Gets the parent volume.</summary>
        /// <value>The parent volume (if this is the root subdirectory or <see langword="null" /> if this is a subdirectory.</value>
        /// <remarks>If this is <see langword="null" />, then <see cref="ISubdirectory.Parent" /> should not be null, and vice-versa.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volume), ResourceType = typeof(Properties.Resources))]
        new ILocalVolume Volume { get; }

        /// <summary>Gets the crawl configuration that starts with the current subdirectory.</summary>
        /// <value>The crawl configuration that starts with the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlConfiguration), ResourceType = typeof(Properties.Resources))]
        new ILocalCrawlConfiguration CrawlConfiguration { get; }

        /// <summary>Gets the files directly contained within this subdirectory.</summary>
        /// <value>The files directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalFile> Files { get; }

        /// <summary>Gets the nested subdirectories directly contained within this subdirectory.</summary>
        /// <value>The nested subdirectories directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SubDirectories), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalSubdirectory> SubDirectories { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalSubdirectoryAccessError> AccessErrors { get; }
    }
}
