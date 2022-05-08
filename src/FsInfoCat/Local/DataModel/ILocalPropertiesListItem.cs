namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IPropertiesListItem" />
    public interface ILocalPropertiesListItem : ILocalPropertiesRow, IPropertiesListItem { }

    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="ILocalSummaryPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="ISummaryPropertiesListItem" />
    public interface ILocalSummaryPropertiesListItem : ILocalSummaryPropertiesRow, ILocalPropertiesListItem, ISummaryPropertiesListItem { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="ILocalDocumentPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IDocumentPropertiesListItem" />
    public interface ILocalDocumentPropertiesListItem : ILocalDocumentPropertiesRow, ILocalPropertiesListItem, IDocumentPropertiesListItem { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="ILocalAudioPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IAudioPropertiesListItem" />
    public interface ILocalAudioPropertiesListItem : ILocalAudioPropertiesRow, ILocalPropertiesListItem, IAudioPropertiesListItem { }

    /// <summary>
    /// Generic interface for list item entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="ILocalDRMPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IDRMPropertiesListItem" />
    public interface ILocalDRMPropertiesListItem : ILocalDRMPropertiesRow, ILocalPropertiesListItem, IDRMPropertiesListItem { }

    /// <summary>
    /// Generic interface for list item entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="ILocalGPSPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IGPSPropertiesListItem" />
    public interface ILocalGPSPropertiesListItem : ILocalGPSPropertiesRow, ILocalPropertiesListItem, IGPSPropertiesListItem { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="ILocalImagePropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IImagePropertiesListItem" />
    public interface ILocalImagePropertiesListItem : ILocalImagePropertiesRow, ILocalPropertiesListItem, IImagePropertiesListItem { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="ILocalMediaPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IMediaPropertiesListItem" />
    public interface ILocalMediaPropertiesListItem : ILocalMediaPropertiesRow, ILocalPropertiesListItem, IMediaPropertiesListItem { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="ILocalMusicPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IMusicPropertiesListItem" />
    public interface ILocalMusicPropertiesListItem : ILocalMusicPropertiesRow, ILocalPropertiesListItem, IMusicPropertiesListItem { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for photo files.
    /// Implements the <see cref="IPhotoPropertiesListItem" />
    /// </summary>
    /// <seealso cref="ILocalPhotoPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IPhotoPropertiesListItem" />
    public interface ILocalPhotoPropertiesListItem : ILocalPhotoPropertiesRow, ILocalPropertiesListItem, IPhotoPropertiesListItem { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="ILocalRecordedTVPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IRecordedTVPropertiesListItem" />
    public interface ILocalRecordedTVPropertiesListItem : ILocalRecordedTVPropertiesRow, ILocalPropertiesListItem, IRecordedTVPropertiesListItem { }

    /// <summary>
    /// Generic interface for list item entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="ILocalVideoPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IVideoPropertiesListItem" />
    public interface ILocalVideoPropertiesListItem : ILocalVideoPropertiesRow, ILocalPropertiesListItem, IVideoPropertiesListItem { }
}
