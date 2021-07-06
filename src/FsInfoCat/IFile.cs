using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a structural instance of file.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    public interface IFile : IDbFsItem
    {
        /// <summary>
        /// Gets or sets the visibility and crawl options for the current file.
        /// </summary>
        /// <value>A <see cref="FileCrawlOptions"/> value that contains the crawl options for the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCrawlOptions), ResourceType = typeof(Properties.Resources))]
        FileCrawlOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the correlative status of the current file.
        /// </summary>
        /// <value>A <see cref="FileCorrelationStatus"/> value that indicates the file's correlation status.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCorrelationStatus), ResourceType = typeof(Properties.Resources))]
        FileCorrelationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the date and time that the <see cref="MD5Hash">MD5 hash</see> was calculated for the current file.
        /// </summary>
        /// <value>The date and time that the <see cref="MD5Hash">MD5 hash</see> was calculated for the current file or <see langword="null"/> if no <see cref="MD5Hash">MD5 hash</see> has been calculated, yet.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastHashCalculation), ResourceType = typeof(Properties.Resources))]
        DateTime? LastHashCalculation { get; set; }

        /// <summary>
        /// Gets or sets the binary properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IBinaryPropertySet"/> that contains the file size and optionally, the <see cref="MD5Hash">MD5 hash</see> value of its binary contents.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BinaryProperties), ResourceType = typeof(Properties.Resources))]
        IBinaryPropertySet BinaryProperties { get; set; }

        /// <summary>
        /// Gets or sets the summary properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IBinaryPropertySet"/> that contains the summary properties for the current file or <see langword="null"/> if no summary properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SummaryProperties), ResourceType = typeof(Properties.Resources))]
        ISummaryPropertySet SummaryProperties { get; set; }

        /// <summary>
        /// Gets or sets the document properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IDocumentPropertySet"/> that contains the document properties for the current file or <see langword="null"/> if no document properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DocumentProperties), ResourceType = typeof(Properties.Resources))]
        IDocumentPropertySet DocumentProperties { get; set; }

        /// <summary>
        /// Gets or sets the audio properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IAudioPropertySet"/> that contains the audio properties for the current file or <see langword="null"/> if no audio properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AudioProperties), ResourceType = typeof(Properties.Resources))]
        IAudioPropertySet AudioProperties { get; set; }

        /// <summary>
        /// Gets or sets the DRM properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IDRMPropertySet"/> that contains the DRM properties for the current file or <see langword="null"/> if no DRM properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DRMProperties), ResourceType = typeof(Properties.Resources))]
        IDRMPropertySet DRMProperties { get; set; }

        /// <summary>
        /// Gets or sets the GPS properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IGPSPropertySet"/> that contains the GPS properties for the current file or <see langword="null"/> if no GPS properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_GPSProperties), ResourceType = typeof(Properties.Resources))]
        IGPSPropertySet GPSProperties { get; set; }

        /// <summary>
        /// Gets or sets the image properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IImagePropertySet"/> that contains the image properties for the current file or <see langword="null"/> if no image properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ImageProperties), ResourceType = typeof(Properties.Resources))]
        IImagePropertySet ImageProperties { get; set; }

        /// <summary>
        /// Gets or sets the media properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IMediaPropertySet"/> that contains the media properties for the current file or <see langword="null"/> if no media properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MediaProperties), ResourceType = typeof(Properties.Resources))]
        IMediaPropertySet MediaProperties { get; set; }

        /// <summary>
        /// Gets or sets the music properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IMusicPropertySet"/> that contains the music properties for the current file or <see langword="null"/> if no music properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MusicProperties), ResourceType = typeof(Properties.Resources))]
        IMusicPropertySet MusicProperties { get; set; }

        /// <summary>
        /// Gets or sets the photo properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IPhotoPropertySet"/> that contains the photo properties for the current file or <see langword="null"/> if no photo properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PhotoProperties), ResourceType = typeof(Properties.Resources))]
        IPhotoPropertySet PhotoProperties { get; set; }

        /// <summary>
        /// Gets or sets the recorded tv properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IRecordedTVPropertySet"/> that contains the recorded TV properties for the current file or <see langword="null"/> if no recorded TV properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RecordedTVProperties), ResourceType = typeof(Properties.Resources))]
        IRecordedTVPropertySet RecordedTVProperties { get; set; }

        /// <summary>
        /// Gets or sets the video properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IVideoPropertySet"/> that contains the video properties for the current file or <see langword="null"/> if no video properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_VideoProperties), ResourceType = typeof(Properties.Resources))]
        IVideoPropertySet VideoProperties { get; set; }

        /// <summary>
        /// Gets the redundancy item that indicates the membership of a collection of redundant files.
        /// </summary>
        /// <value>A <see cref="IRedundancy"/> object that indicates the current file is an exact copy of other files that belong to the same <see cref="IRedundancy.RedundantSet"/>
        [Display(Name = nameof(Properties.Resources.DisplayName_Redundancy), ResourceType = typeof(Properties.Resources))]
        /// or <see langword="null"/> if this file has not been identified as being redundant with any other.</value>
        IRedundancy Redundancy { get; }

        /// <summary>
        /// Gets the comparisons where the current file was the <see cref="IComparison.Baseline"/>.
        /// </summary>
        /// <value>The <see cref="IComparison"/> entities where the current file is the <see cref="IComparison.Baseline"/>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BaselineComparisons), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IComparison> BaselineComparisons { get; }

        /// <summary>
        /// Gets the comparisons where the current file was the <see cref="IComparison.Correlative"/> being compared to a separate <see cref="IComparison.Baseline"/> file.
        /// </summary>
        /// <value>The <see cref="IComparison"/> entities where the current file is the <see cref="IComparison.Correlative"/>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CorrelativeComparisons), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IComparison> CorrelativeComparisons { get; }

        /// <summary>
        /// Gets the access errors that occurred while trying to open or read from the current file.
        /// </summary>
        /// <value>The access errors that occurred while trying to open or read from the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IAccessError<IFile>> AccessErrors { get; }
    }
}
