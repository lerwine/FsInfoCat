using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Represents a structural instance of file in the upstream (remote) database.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    public interface IUpstreamFile : IFile, IUpstreamDbFsItem
    {
        /// <summary>
        /// Gets or sets the binary properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamBinaryPropertySet"/> that contains the file size and optionally, the <see cref="MD5Hash">MD5 hash</see> value of its binary contents.</value>
        new IUpstreamBinaryPropertySet BinaryProperties { get; set; }

        /// <summary>
        /// Gets or sets the summary properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamSummaryPropertySet"/> that contains the summary properties for the current file or <see langword="null"/> if no summary properties are defined on the current file.</value>
        new IUpstreamSummaryPropertySet SummaryProperties { get; set; }

        /// <summary>
        /// Gets or sets the document properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamDocumentPropertySet"/> that contains the document properties for the current file or <see langword="null"/> if no document properties are defined on the current file.</value>
        new IUpstreamDocumentPropertySet DocumentProperties { get; set; }

        /// <summary>
        /// Gets or sets the audio properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamAudioPropertySet"/> that contains the audio properties for the current file or <see langword="null"/> if no audio properties are defined on the current file.</value>
        new IUpstreamAudioPropertySet AudioProperties { get; set; }

        /// <summary>
        /// Gets or sets the DRM properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamDRMPropertySet"/> that contains the DRM properties for the current file or <see langword="null"/> if no DRM properties are defined on the current file.</value>
        new IUpstreamDRMPropertySet DRMProperties { get; set; }

        /// <summary>
        /// Gets or sets the GPS properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamGPSPropertySet"/> that contains the GPS properties for the current file or <see langword="null"/> if no GPS properties are defined on the current file.</value>
        new IUpstreamGPSPropertySet GPSProperties { get; set; }

        /// <summary>
        /// Gets or sets the image properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamImagePropertySet"/> that contains the image properties for the current file or <see langword="null"/> if no image properties are defined on the current file.</value>
        new IUpstreamImagePropertySet ImageProperties { get; set; }

        /// <summary>
        /// Gets or sets the media properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamMediaPropertySet"/> that contains the media properties for the current file or <see langword="null"/> if no media properties are defined on the current file.</value>
        new IUpstreamMediaPropertySet MediaProperties { get; set; }

        /// <summary>
        /// Gets or sets the music properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamMusicPropertySet"/> that contains the music properties for the current file or <see langword="null"/> if no music properties are defined on the current file.</value>
        new IUpstreamMusicPropertySet MusicProperties { get; set; }

        /// <summary>
        /// Gets or sets the photo properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamPhotoPropertySet"/> that contains the photo properties for the current file or <see langword="null"/> if no photo properties are defined on the current file.</value>
        new IUpstreamPhotoPropertySet PhotoProperties { get; set; }

        /// <summary>
        /// Gets or sets the recorded tv properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamRecordedTVPropertySet"/> that contains the recorded TV properties for the current file or <see langword="null"/> if no recorded TV properties are defined on the current file.</value>
        new IUpstreamRecordedTVPropertySet RecordedTVProperties { get; set; }

        /// <summary>
        /// Gets or sets the video properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamVideoPropertySet"/> that contains the video properties for the current file or <see langword="null"/> if no video properties are defined on the current file.</value>
        new IUpstreamVideoPropertySet VideoProperties { get; set; }

        /// <summary>
        /// Gets or sets the parent subdirectory of the current file system item.
        /// </summary>
        /// <value>The parent <see cref="IUpstreamSubdirectory"/> of the current file system item.</value>
        new IUpstreamSubdirectory Parent { get; set; }

        /// <summary>
        /// Gets the redundancy item that indicates the membership of a collection of redundant files.
        /// </summary>
        /// <value>A <see cref="IUpstreamRedundancy"/> object that indicates the current file is an exact copy of other files that belong to the same <see cref="IUpstreamRedundancy.RedundantSet"/>
        /// or <see langword="null"/> if this file has not been identified as being redundant with any other.</value>
        new IUpstreamRedundancy Redundancy { get; }

        IEnumerable<IFileAction> FileActions { get; }

        /// <summary>
        /// Gets the comparisons where the current file was the <see cref="IUpstreamComparison.Baseline"/>.
        /// </summary>
        /// <value>The <see cref="IUpstreamComparison"/> entities where the current file is the <see cref="IUpstreamComparison.Baseline"/>.</value>
        new IEnumerable<IUpstreamComparison> BaselineComparisons { get; }

        /// <summary>
        /// Gets the comparisons where the current file was the <see cref="IUpstreamComparison.Correlative"/> being compared to a separate <see cref="IUpstreamComparison.Baseline"/> file.
        /// </summary>
        /// <value>The <see cref="IUpstreamComparison"/> entities where the current file is the <see cref="IUpstreamComparison.Correlative"/>.</value>
        new IEnumerable<IUpstreamComparison> CorrelativeComparisons { get; }

        /// <summary>
        /// Gets the access errors that occurred while trying to open or read from the current file.
        /// </summary>
        /// <value>The access errors that occurred while trying to open or read from the current file.</value>
        new IEnumerable<IAccessError<IUpstreamFile>> AccessErrors { get; }
    }
}
