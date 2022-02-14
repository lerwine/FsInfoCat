using System;
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
}
