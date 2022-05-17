using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="Local.ILocalPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamPropertiesListItem" />
    public interface IPropertiesListItem : IPropertiesRow
    {
        /// <summary>
        /// Gets the number of non-deleted files associated with the current property set.
        /// </summary>
        /// <value>The number of non-deleted files associated with the current property set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        long ExistingFileCount { get; }

        /// <summary>
        /// Gets the total number of file entities associated with the current property set.
        /// </summary>
        /// <value>The number of files associated with the current property set, including entities representing deleted files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AllFiles), ResourceType = typeof(Properties.Resources))]
        long TotalFileCount { get; }
    }

    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="ISummaryPropertiesRow" />
    /// <seealso cref="IEquatable{ISummaryPropertiesListItem}" />
    /// <seealso cref="Local.ILocalSummaryPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamSummaryPropertiesListItem" />
    public interface ISummaryPropertiesListItem : IPropertiesListItem, ISummaryPropertiesRow, IEquatable<ISummaryPropertiesListItem> { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IDocumentPropertiesRow" />
    /// <seealso cref="IEquatable{IDocumentPropertiesListItem}" />
    /// <seealso cref="Local.ILocalDocumentPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamDocumentPropertiesListItem" />
    public interface IDocumentPropertiesListItem : IPropertiesListItem, IDocumentPropertiesRow, IEquatable<IDocumentPropertiesListItem> { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IAudioPropertiesRow" />
    /// <seealso cref="IEquatable{IAudioPropertiesListItem}" />
    /// <seealso cref="Local.ILocalAudioPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamAudioPropertiesListItem" />
    public interface IAudioPropertiesListItem : IPropertiesListItem, IAudioPropertiesRow, IEquatable<IAudioPropertiesListItem> { }

    /// <summary>
    /// Generic interface for list item entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IDRMPropertiesRow" />
    /// <seealso cref="IEquatable{IDRMPropertiesListItem}" />
    /// <seealso cref="Local.ILocalDRMPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamDRMPropertiesListItem" />
    public interface IDRMPropertiesListItem : IPropertiesListItem, IDRMPropertiesRow, IEquatable<IDRMPropertiesListItem> { }

    /// <summary>
    /// Generic interface for list item entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IGPSPropertiesRow" />
    /// <seealso cref="IEquatable{IGPSPropertiesListItem}" />
    /// <seealso cref="Local.ILocalGPSPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamGPSPropertiesListItem" />
    public interface IGPSPropertiesListItem : IPropertiesListItem, IGPSPropertiesRow, IEquatable<IGPSPropertiesListItem> { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IImagePropertiesRow" />
    /// <seealso cref="IEquatable{IImagePropertiesListItem}" />
    /// <seealso cref="Local.ILocalImagePropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamImagePropertiesListItem" />
    public interface IImagePropertiesListItem : IPropertiesListItem, IImagePropertiesRow, IEquatable<IImagePropertiesListItem> { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IMediaPropertiesRow" />
    /// <seealso cref="IEquatable{IMediaPropertiesListItem}" />
    /// <seealso cref="Local.ILocalMediaPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamMediaPropertiesListItem" />
    public interface IMediaPropertiesListItem : IPropertiesListItem, IMediaPropertiesRow, IEquatable<IMediaPropertiesListItem> { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IMusicPropertiesRow" />
    /// <seealso cref="IEquatable{IMusicPropertiesListItem}" />
    /// <seealso cref="Local.ILocalMusicPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamMusicPropertiesListItem" />
    public interface IMusicPropertiesListItem : IPropertiesListItem, IMusicPropertiesRow, IEquatable<IMusicPropertiesListItem> { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IPhotoPropertiesRow" />
    /// <seealso cref="IEquatable{IPhotoPropertiesListItem}" />
    /// <seealso cref="Local.ILocalPhotoPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamPhotoPropertiesListItem" />
    public interface IPhotoPropertiesListItem : IPropertiesListItem, IPhotoPropertiesRow, IEquatable<IPhotoPropertiesListItem> { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IRecordedTVPropertiesRow" />
    /// <seealso cref="IEquatable{IRecordedTVPropertiesListItem}" />
    /// <seealso cref="Local.ILocalRecordedTVPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamRecordedTVPropertiesListItem" />
    public interface IRecordedTVPropertiesListItem : IPropertiesListItem, IRecordedTVPropertiesRow, IEquatable<IRecordedTVPropertiesListItem> { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IVideoPropertiesRow" />
    /// <seealso cref="IEquatable{IVideoPropertiesListItem}" />
    /// <seealso cref="Local.ILocalVideoPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamVideoPropertiesListItem" />
    public interface IVideoPropertiesListItem : IPropertiesListItem, IVideoPropertiesRow, IEquatable<IVideoPropertiesListItem> { }
}
