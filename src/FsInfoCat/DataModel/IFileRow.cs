using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a structural instance of file.
    /// </summary>
    /// <seealso cref="IDbFsItem" />
    public interface IFileRow : IDbFsItemRow
    {
        /// <summary>
        /// Gets the visibility and crawl options for the current file.
        /// </summary>
        /// <value>A <see cref="FileCrawlOptions" /> value that contains the crawl options for the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Options), ResourceType = typeof(Properties.Resources))]
        FileCrawlOptions Options { get; }

        /// <summary>
        /// Gets the correlative status of the current file.
        /// </summary>
        /// <value>A <see cref="FileCorrelationStatus" /> value that indicates the file's correlation status.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        FileCorrelationStatus Status { get; }

        /// <summary>
        /// Gets the date and time that the <see cref="MD5Hash">MD5 hash</see> was calculated for the current file.
        /// </summary>
        /// <value>The date and time that the <see cref="MD5Hash">MD5 hash</see> was calculated for the current file or <see langword="null" /> if no <see cref="MD5Hash">MD5 hash</see> has been calculated, yet.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastHashCalculation), ResourceType = typeof(Properties.Resources))]
        DateTime? LastHashCalculation { get; }

        Guid ParentId { get; }

        Guid BinaryPropertySetId { get; }

        Guid? SummaryPropertySetId { get; }

        Guid? DocumentPropertySetId { get; }

        Guid? AudioPropertySetId { get; }

        Guid? DRMPropertySetId { get; }

        Guid? GPSPropertySetId { get; }

        Guid? ImagePropertySetId { get; }

        Guid? MediaPropertySetId { get; }

        Guid? MusicPropertySetId { get; }

        Guid? PhotoPropertySetId { get; }

        Guid? RecordedTVPropertySetId { get; }

        Guid? VideoPropertySetId { get; }
    }
}
