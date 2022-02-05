using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{

    /// <summary>Represents a structural instance of file on a local host file system.</summary>
    /// <seealso cref="ILocalDbFsItem" />
    /// <seealso cref="IFile" />
    public interface ILocalFile : ILocalDbFsItem, IFile, ILocalFileRow
    {
        /// <summary>Gets the binary properties for the current file.</summary>
        /// <value>The generic <see cref="ILocalBinaryPropertySet" /> that contains the file size and optionally, the <see cref="MD5Hash">MD5 hash</see> value of its binary contents.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BinaryProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalBinaryPropertySet BinaryProperties { get; }

        /// <summary>Gets the summary properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaBinaryPropertySet" /> that contains the summary properties for the current file or <see langword="null" /> if no summary properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SummaryProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalSummaryPropertySet SummaryProperties { get; }

        /// <summary>Gets the document properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaDocumentPropertySet" /> that contains the document properties for the current file or <see langword="null" /> if no document properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DocumentProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalDocumentPropertySet DocumentProperties { get; }

        /// <summary>Gets the audio properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaAudioPropertySet" /> that contains the audio properties for the current file or <see langword="null" /> if no audio properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AudioProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalAudioPropertySet AudioProperties { get; }

        /// <summary>Gets the DRM properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaDRMPropertySet" /> that contains the DRM properties for the current file or <see langword="null" /> if no DRM properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DRMProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalDRMPropertySet DRMProperties { get; }

        /// <summary>Gets the GPS properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaGPSPropertySet" /> that contains the GPS properties for the current file or <see langword="null" /> if no GPS properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_GPSProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalGPSPropertySet GPSProperties { get; }

        /// <summary>Gets the image properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaImagePropertySet" /> that contains the image properties for the current file or <see langword="null" /> if no image properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ImageProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalImagePropertySet ImageProperties { get; }

        /// <summary>Gets the media properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaMediaPropertySet" /> that contains the media properties for the current file or <see langword="null" /> if no media properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MediaProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalMediaPropertySet MediaProperties { get; }

        /// <summary>Gets the music properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaMusicPropertySet" /> that contains the music properties for the current file or <see langword="null" /> if no music properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MusicProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalMusicPropertySet MusicProperties { get; }

        /// <summary>Gets the photo properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaPhotoPropertySet" /> that contains the photo properties for the current file or <see langword="null" /> if no photo properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PhotoProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalPhotoPropertySet PhotoProperties { get; }

        /// <summary>Gets the recorded tv properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaRecordedTVPropertySet" /> that contains the recorded TV properties for the current file or <see langword="null" /> if no recorded TV properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RecordedTVProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalRecordedTVPropertySet RecordedTVProperties { get; }

        /// <summary>Gets the video properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaVideoPropertySet" /> that contains the video properties for the current file or <see langword="null" /> if no video properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_VideoProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalVideoPropertySet VideoProperties { get; }

        /// <summary>Gets the redundancy item that indicates the membership of a collection of redundant files.</summary>
        /// <value>
        /// An <see cref="ILocaRedundancy" /> object that indicates the current file is an exact copy of other files that belong to the same <see cref="IRedundancy.RedundantSet" />
        /// or <see langword="null" /> if this file has not been identified as being redundant with any other.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Redundancy), ResourceType = typeof(Properties.Resources))]
        new ILocalRedundancy Redundancy { get; }

        /// <summary>Gets the comparisons where the current file was the <see cref="IComparison.Baseline" />.</summary>
        /// <value>The <see cref="ILocaComparison" /> entities where the current file is the <see cref="IComparison.Baseline" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BaselineComparisons), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalComparison> BaselineComparisons { get; }

        /// <summary>Gets the comparisons where the current file was the <see cref="IComparison.Correlative" /> being compared to a separate <see cref="IComparison.Baseline" /> file.</summary>
        /// <value>The <see cref="ILocaComparison" /> entities where the current file is the <see cref="IComparison.Correlative" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CorrelativeComparisons), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalComparison> CorrelativeComparisons { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalFileAccessError> AccessErrors { get; }

        new IEnumerable<ILocalPersonalFileTag> PersonalTags { get; }

        new IEnumerable<ILocalSharedFileTag> SharedTags { get; }
    }
}
