using System.Collections.Generic;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a structural instance of local file.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    public interface ILocalFile : IFile, ILocalDbFsItem
    {
        /// <summary>
        /// Gets or sets the binary properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="ILocalBinaryPropertySet"/> that contains the file size and optionally, the <see cref="MD5Hash">MD5 hash</see> value of its binary contents.</value>
        new ILocalBinaryPropertySet BinaryProperties { get; set; }

        /// <summary>
        /// Gets or sets the summary properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="ILocalSummaryPropertySet"/> that contains the summary properties for the current file or <see langword="null"/> if no summary properties are defined on the current file.</value>
        new ILocalSummaryPropertySet SummaryProperties { get; set; }

        /// <summary>
        /// Gets or sets the document properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="ILocalDocumentPropertySet"/> that contains the document properties for the current file or <see langword="null"/> if no document properties are defined on the current file.</value>
        new ILocalDocumentPropertySet DocumentProperties { get; set; }

        /// <summary>
        /// Gets or sets the audio properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="ILocalAudioPropertySet"/> that contains the audio properties for the current file or <see langword="null"/> if no audio properties are defined on the current file.</value>
        new ILocalAudioPropertySet AudioProperties { get; set; }

        /// <summary>
        /// Gets or sets the DRM properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="ILocalDRMPropertySet"/> that contains the DRM properties for the current file or <see langword="null"/> if no DRM properties are defined on the current file.</value>
        new ILocalDRMPropertySet DRMProperties { get; set; }

        /// <summary>
        /// Gets or sets the GPS properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="ILocalGPSPropertySet"/> that contains the GPS properties for the current file or <see langword="null"/> if no GPS properties are defined on the current file.</value>
        new ILocalGPSPropertySet GPSProperties { get; set; }

        /// <summary>
        /// Gets or sets the image properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="ILocalImagePropertySet"/> that contains the image properties for the current file or <see langword="null"/> if no image properties are defined on the current file.</value>
        new ILocalImagePropertySet ImageProperties { get; set; }

        /// <summary>
        /// Gets or sets the media properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="ILocalMediaPropertySet"/> that contains the media properties for the current file or <see langword="null"/> if no media properties are defined on the current file.</value>
        new ILocalMediaPropertySet MediaProperties { get; set; }

        /// <summary>
        /// Gets or sets the music properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="ILocalMusicPropertySet"/> that contains the music properties for the current file or <see langword="null"/> if no music properties are defined on the current file.</value>
        new ILocalMusicPropertySet MusicProperties { get; set; }

        /// <summary>
        /// Gets or sets the photo properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="ILocalPhotoPropertySet"/> that contains the photo properties for the current file or <see langword="null"/> if no photo properties are defined on the current file.</value>
        new ILocalPhotoPropertySet PhotoProperties { get; set; }

        /// <summary>
        /// Gets or sets the recorded tv properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="ILocalRecordedTVPropertySet"/> that contains the recorded TV properties for the current file or <see langword="null"/> if no recorded TV properties are defined on the current file.</value>
        new ILocalRecordedTVPropertySet RecordedTVProperties { get; set; }

        /// <summary>
        /// Gets or sets the video properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="ILocalVideoPropertySet"/> that contains the video properties for the current file or <see langword="null"/> if no video properties are defined on the current file.</value>
        new ILocalVideoPropertySet VideoProperties { get; set; }

        /// <summary>
        /// Gets or sets the parent subdirectory of the current file system item.
        /// </summary>
        /// <value>The parent <see cref="ILocalSubdirectory"/> of the current file system item.</value>
        new ILocalSubdirectory Parent { get; set; }

        /// <summary>
        /// Gets the redundancy item that indicates the membership of a collection of redundant files.
        /// </summary>
        /// <value>A <see cref="ILocalRedundancy"/> object that indicates the current file is an exact copy of other files that belong to the same <see cref="ILocalRedundancy.RedundantSet"/>
        /// or <see langword="null"/> if this file has not been identified as being redundant with any other.</value>
        new ILocalRedundancy Redundancy { get; }

        /// <summary>
        /// Gets the comparisons where the current file was the <see cref="ILocalComparison.Baseline"/>.
        /// </summary>
        /// <value>The <see cref="ILocalComparison"/> entities where the current file is the <see cref="ILocalComparison.Baseline"/>.</value>
        new IEnumerable<ILocalComparison> BaselineComparisons { get; }

        /// <summary>
        /// Gets the comparisons where the current file was the <see cref="ILocalComparison.Correlative"/> being compared to a separate <see cref="ILocalComparison.Baseline"/> file.
        /// </summary>
        /// <value>The <see cref="ILocalComparison"/> entities where the current file is the <see cref="ILocalComparison.Correlative"/>.</value>
        new IEnumerable<ILocalComparison> CorrelativeComparisons { get; }

        /// <summary>
        /// Gets the access errors that occurred while trying to open or read from the current file.
        /// </summary>
        /// <value>The access errors that occurred while trying to open or read from the current file.</value>
        new IEnumerable<IAccessError<ILocalFile>> AccessErrors { get; }
    }
}
