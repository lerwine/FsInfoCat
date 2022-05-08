namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IPropertiesRow" />
    public interface ILocalPropertiesRow : ILocalDbEntity, IPropertiesRow { }

    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="ISummaryPropertiesRow" />
    public interface ILocalSummaryPropertiesRow : ILocalPropertiesRow, ISummaryPropertiesRow { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IDocumentPropertiesRow" />
    public interface ILocalDocumentPropertiesRow : ILocalPropertiesRow, IDocumentPropertiesRow { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IAudioPropertiesRow" />
    public interface ILocalAudioPropertiesRow : ILocalPropertiesRow, IAudioPropertiesRow { }

    /// <summary>
    /// Generic interface for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IDRMPropertiesRow" />
    public interface ILocalDRMPropertiesRow : ILocalPropertiesRow, IDRMPropertiesRow { }

    /// <summary>
    /// Generic interface for entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IGPSPropertiesRow" />
    public interface ILocalGPSPropertiesRow : ILocalPropertiesRow, IGPSPropertiesRow { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IImagePropertiesRow" />
    public interface ILocalImagePropertiesRow : ILocalPropertiesRow, IImagePropertiesRow { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IMediaPropertiesRow" />
    public interface ILocalMediaPropertiesRow : ILocalPropertiesRow, IMediaPropertiesRow { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IMusicPropertiesRow" />
    public interface ILocalMusicPropertiesRow : ILocalPropertiesRow, IMusicPropertiesRow { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IPhotoPropertiesRow" />
    public interface ILocalPhotoPropertiesRow : ILocalPropertiesRow, IPhotoPropertiesRow { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IRecordedTVPropertiesRow" />
    public interface ILocalRecordedTVPropertiesRow : ILocalPropertiesRow, IRecordedTVPropertiesRow { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IVideoPropertiesRow" />
    public interface ILocalVideoPropertiesRow : ILocalPropertiesRow, IVideoPropertiesRow { }
}
