using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Represents a structural instance of file.
    /// </summary>
    /// <seealso cref="IDbFsItem" />
    /// <seealso cref="Local.Model.ILocalFile" />
    /// <seealso cref="Upstream.Model.IUpstreamFile" />
    /// <seealso cref="IBinaryPropertySet.Files" />
    /// <seealso cref="IPropertySet.Files" />
    /// <seealso cref="IRedundancy.File" />
    /// <seealso cref="IFileAccessError.Target" />
    /// <seealso cref="IFileTag.Tagged" />
    /// <seealso cref="IComparison.Baseline" />
    /// <seealso cref="IComparison.Correlative" />
    /// <seealso cref="IDbContext.Files" />
    public interface IFile : IDbFsItem, IFileRow, IEquatable<IFile>
    {
        /// <summary>
        /// Gets the binary properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IBinaryPropertySet" /> that contains the file size and optionally, the <see cref="MD5Hash">MD5 hash</see> value of its binary
        /// contents.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BinaryProperties), ResourceType = typeof(Properties.Resources))]
        IBinaryPropertySet BinaryProperties { get; }

        /// <summary>
        /// Gets the summary properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IBinaryPropertySet" /> that contains the summary properties for the current file or <see langword="null" /> if no summary properties
        /// are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SummaryProperties), ResourceType = typeof(Properties.Resources))]
        ISummaryPropertySet SummaryProperties { get; }

        /// <summary>
        /// Gets the document properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IDocumentPropertySet" /> that contains the document properties for the current file or <see langword="null" /> if no document properties
        /// are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DocumentProperties), ResourceType = typeof(Properties.Resources))]
        IDocumentPropertySet DocumentProperties { get; }

        /// <summary>
        /// Gets the audio properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IAudioPropertySet" /> that contains the audio properties for the current file or <see langword="null" /> if no audio properties are
        /// defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AudioProperties), ResourceType = typeof(Properties.Resources))]
        IAudioPropertySet AudioProperties { get; }

        /// <summary>
        /// Gets the DRM properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IDRMPropertySet" /> that contains the DRM properties for the current file or <see langword="null" /> if no DRM properties are defined
        /// on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DRMProperties), ResourceType = typeof(Properties.Resources))]
        IDRMPropertySet DRMProperties { get; }

        /// <summary>
        /// Gets the GPS properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IGPSPropertySet" /> that contains the GPS properties for the current file or <see langword="null" /> if no GPS properties are defined
        /// on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_GPSProperties), ResourceType = typeof(Properties.Resources))]
        IGPSPropertySet GPSProperties { get; }

        /// <summary>
        /// Gets the image properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IImagePropertySet" /> that contains the image properties for the current file or <see langword="null" /> if no image properties are
        /// defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ImageProperties), ResourceType = typeof(Properties.Resources))]
        IImagePropertySet ImageProperties { get; }

        /// <summary>
        /// Gets the media properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IMediaPropertySet" /> that contains the media properties for the current file or <see langword="null" /> if no media properties are
        /// defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MediaProperties), ResourceType = typeof(Properties.Resources))]
        IMediaPropertySet MediaProperties { get; }

        /// <summary>
        /// Gets the music properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IMusicPropertySet" /> that contains the music properties for the current file or <see langword="null" /> if no music properties are
        /// defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MusicProperties), ResourceType = typeof(Properties.Resources))]
        IMusicPropertySet MusicProperties { get; }

        /// <summary>
        /// Gets the photo properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IPhotoPropertySet" /> that contains the photo properties for the current file or <see langword="null" /> if no photo properties are
        /// defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PhotoProperties), ResourceType = typeof(Properties.Resources))]
        IPhotoPropertySet PhotoProperties { get; }

        /// <summary>
        /// Gets the recorded tv properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IRecordedTVPropertySet" /> that contains the recorded TV properties for the current file or <see langword="null" /> if no recorded TV
        /// properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RecordedTVProperties), ResourceType = typeof(Properties.Resources))]
        IRecordedTVPropertySet RecordedTVProperties { get; }

        /// <summary>
        /// Gets the video properties for the current file.
        /// </summary>
        /// <value>The generic <see cref="IVideoPropertySet" /> that contains the video properties for the current file or <see langword="null" /> if no video properties are
        /// defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_VideoProperties), ResourceType = typeof(Properties.Resources))]
        IVideoPropertySet VideoProperties { get; }

        /// <summary>
        /// Gets the redundancy item that indicates the membership of a collection of redundant files.
        /// </summary>
        /// <value>
        /// An <see cref="IRedundancy" /> object that indicates the current file is an exact copy of other files that belong to the same <see cref="IRedundancy.RedundantSet" />
        /// or <see langword="null" /> if this file has not been identified as being redundant with any other.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Redundancy), ResourceType = typeof(Properties.Resources))]
        IRedundancy Redundancy { get; }

        /// <summary>
        /// Gets the comparisons where the current file was the <see cref="IComparison.Baseline" />.
        /// </summary>
        /// <value>The <see cref="IComparison" /> entities where the current file is the <see cref="IComparison.Baseline" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BaselineComparisons), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IComparison> BaselineComparisons { get; }

        /// <summary>
        /// Gets the comparisons where the current file was the <see cref="IComparison.Correlative" /> being compared to a separate <see cref="IComparison.Baseline" /> file.
        /// </summary>
        /// <value>The <see cref="IComparison" /> entities where the current file is the <see cref="IComparison.Correlative" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CorrelativeComparisons), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IComparison> CorrelativeComparisons { get; }

        /// <summary>
        /// Gets the access errors for the current file system item.
        /// </summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IFileAccessError> AccessErrors { get; }

        /// <summary>
        /// Gets the personal tags associated with the current file.
        /// </summary>
        /// <value>The <see cref="ISharedFileTag"/> entities that associate <see cref="IPersonalTagDefinition"/> entities with the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PersonalTags), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IPersonalFileTag> PersonalTags { get; }

        /// <summary>
        /// Gets the shared tags associated with the current file.
        /// </summary>
        /// <value>The <see cref="ISharedFileTag"/> entities that associate <see cref="ISharedTagDefinition"/> entities with the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SharedTags), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ISharedFileTag> SharedTags { get; }

        /// <summary>
        /// Attempts to get the primary key of the binary properties entity.
        /// </summary>
        /// <param name="binaryPropertySetId">The <see cref="IHasSimpleIdentifier.Id"/> value of the associated <see cref="IBinaryPropertySet"/>r.</param>
        /// <returns><see langword="true"/> if <see cref="IFileRow.BinaryPropertySetId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetBinaryPropertySetId(out Guid binaryPropertySetId);

        /// <summary>
        /// Attempts to get the primary key of the summary properties entity.
        /// </summary>
        /// <param name="summaryPropertySetId">The <see cref="IHasSimpleIdentifier.Id"/> value of the associated <see cref="ISummaryPropertySet"/>r.</param>
        /// <returns><see langword="true"/> if <see cref="IFileRow.SummaryPropertySetId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetSummaryPropertySetId(out Guid summaryPropertySetId);

        /// <summary>
        /// Attempts to get the primary key of the document properties entity.
        /// </summary>
        /// <param name="documentPropertySetId">The <see cref="IHasSimpleIdentifier.Id"/> value of the associated <see cref="IDocumentPropertySet"/>r.</param>
        /// <returns><see langword="true"/> if <see cref="IFileRow.DocumentPropertySetId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetDocumentPropertySetId(out Guid documentPropertySetId);

        /// <summary>
        /// Attempts to get the primary key of the audio properties entity.
        /// </summary>
        /// <param name="audioPropertySetId">The <see cref="IHasSimpleIdentifier.Id"/> value of the associated <see cref="IAudioPropertySet"/>r.</param>
        /// <returns><see langword="true"/> if <see cref="IFileRow.AudioPropertySetId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetAudioPropertySetId(out Guid audioPropertySetId);

        /// <summary>
        /// Attempts to get the primary key of the DRM properties entity.
        /// </summary>
        /// <param name="drmPropertySetId">The <see cref="IHasSimpleIdentifier.Id"/> value of the associated <see cref="IDRMPropertySet"/>r.</param>
        /// <returns><see langword="true"/> if <see cref="IFileRow.DRMPropertySetId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetDRMPropertySetId(out Guid drmPropertySetId);

        /// <summary>
        /// Attempts to get the primary key of the GPS properties entity.
        /// </summary>
        /// <param name="gpsPropertySetId">The <see cref="IHasSimpleIdentifier.Id"/> value of the associated <see cref="IGPSPropertySet"/>r.</param>
        /// <returns><see langword="true"/> if <see cref="IFileRow.GPSPropertySetId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetGPSPropertySetId(out Guid gpsPropertySetId);

        /// <summary>
        /// Attempts to get the primary key of the image properties entity.
        /// </summary>
        /// <param name="imagePropertySetId">The <see cref="IHasSimpleIdentifier.Id"/> value of the associated <see cref="IImagePropertySet"/>r.</param>
        /// <returns><see langword="true"/> if <see cref="IFileRow.ImagePropertySetId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetImagePropertySetId(out Guid imagePropertySetId);

        /// <summary>
        /// Attempts to get the primary key of the media properties entity.
        /// </summary>
        /// <param name="mediaPropertySetId">The <see cref="IHasSimpleIdentifier.Id"/> value of the associated <see cref="IMediaPropertySet"/>r.</param>
        /// <returns><see langword="true"/> if <see cref="IFileRow.MediaPropertySetId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetMediaPropertySetId(out Guid mediaPropertySetId);

        /// <summary>
        /// Attempts to get the primary key of the music properties entity.
        /// </summary>
        /// <param name="musicPropertySetId">The <see cref="IHasSimpleIdentifier.Id"/> value of the associated <see cref="IMusicPropertySet"/>r.</param>
        /// <returns><see langword="true"/> if <see cref="IFileRow.MusicPropertySetId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetMusicPropertySetId(out Guid musicPropertySetId);

        /// <summary>
        /// Attempts to get the primary key of the photo properties entity.
        /// </summary>
        /// <param name="photoPropertySetId">The <see cref="IHasSimpleIdentifier.Id"/> value of the associated <see cref="IPhotoPropertySet"/>r.</param>
        /// <returns><see langword="true"/> if <see cref="IFileRow.PhotoPropertySetId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetPhotoPropertySetId(out Guid photoPropertySetId);

        /// <summary>
        /// Attempts to get the primary key of the recorded TV properties entity.
        /// </summary>
        /// <param name="recordedTVPropertySetId">The <see cref="IHasSimpleIdentifier.Id"/> value of the associated <see cref="IRecordedTVPropertySet"/>r.</param>
        /// <returns><see langword="true"/> if <see cref="IFileRow.RecordedTVPropertySetId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetRecordedTVPropertySetId(out Guid recordedTVPropertySetId);

        /// <summary>
        /// Attempts to get the primary key of the video properties entity.
        /// </summary>
        /// <param name="videoPropertySetId">The <see cref="IHasSimpleIdentifier.Id"/> value of the associated <see cref="IVideoPropertySet"/>.</param>
        /// <returns><see langword="true"/> if <see cref="IFileRow.VideoPropertySetId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetVideoPropertySetId(out Guid videoPropertySetId);
    }
}