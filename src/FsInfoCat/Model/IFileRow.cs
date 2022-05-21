using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Represents a structural instance of file.
    /// </summary>
    /// <seealso cref="IDbFsItem" />
    /// <seealso cref="Local.ILocalFileRow" />
    /// <seealso cref="Upstream.IUpstreamFileRow" />
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
        /// <value>The date and time that the <see cref="MD5Hash">MD5 hash</see> was calculated for the current file or <see langword="null" />
        /// if no <see cref="MD5Hash">MD5 hash</see> has been calculated, yet.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastHashCalculation), ResourceType = typeof(Properties.Resources))]
        DateTime? LastHashCalculation { get; }

        /// <summary>
        /// Gets the unique identifier of the parent subdirectory.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id" /> of the parent <see cref="ISubdirectory" /> entity.</value>
        Guid ParentId { get; }

        /// <summary>
        /// Gets unique identifier of the associated binary properties entity.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id" /> of the <see cref="IBinaryPropertySet" /> that has the length and MD5 hash that matches the current file.</value>
        Guid BinaryPropertySetId { get; }

        /// <summary>
        /// Gets unique identifier of the associated summary properties entity.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id" /> of the <see cref="ISummaryPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no summary properties.</value>
        Guid? SummaryPropertySetId { get; }

        /// <summary>
        /// Gets unique identifier of the associated document properties entity.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id" /> of the <see cref="IDocumentPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no document properties.</value>
        Guid? DocumentPropertySetId { get; }

        /// <summary>
        /// Gets unique identifier of the associated audio properties entity.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id" /> of the <see cref="IAudioPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no audio properties.</value>
        Guid? AudioPropertySetId { get; }

        /// <summary>
        /// Gets unique identifier of the associated DRM properties entity.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id" /> of the <see cref="IDRMPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no DRM properties.</value>
        Guid? DRMPropertySetId { get; }

        /// <summary>
        /// Gets unique identifier of the associated GPS properties entity.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id" /> of the <see cref="IGPSPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no GPS properties.</value>
        Guid? GPSPropertySetId { get; }

        /// <summary>
        /// Gets unique identifier of the associated image properties entity.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id" /> of the <see cref="IImagePropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no image properties.</value>
        Guid? ImagePropertySetId { get; }

        /// <summary>
        /// Gets unique identifier of the associated media properties entity.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id" /> of the <see cref="IMediaPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no media properties.</value>
        Guid? MediaPropertySetId { get; }

        /// <summary>
        /// Gets unique identifier of the associated music properties entity.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id" /> of the <see cref="IMusicPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no music properties.</value>
        Guid? MusicPropertySetId { get; }

        /// <summary>
        /// Gets unique identifier of the associated photo properties entity.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id" /> of the <see cref="IPhotoPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no photo properties.</value>
        Guid? PhotoPropertySetId { get; }

        /// <summary>
        /// Gets unique identifier of the associated recorded TV properties entity.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id" /> of the <see cref="IRecordedTVPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no recorded TV properties.</value>
        Guid? RecordedTVPropertySetId { get; }

        /// <summary>
        /// Gets unique identifier of the associated video properties entity.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id" /> of the <see cref="IVideoPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no video properties.</value>
        Guid? VideoPropertySetId { get; }
    }
}
