using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IHasSimpleIdentifier" />
    public interface IPropertiesRow : IDbEntity, IHasSimpleIdentifier { }

    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="ISummaryProperties" />
    /// <seealso cref="IEquatable{ISummaryPropertiesRow}" />
    public interface ISummaryPropertiesRow : IPropertiesRow, ISummaryProperties, IEquatable<ISummaryPropertiesRow> { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IDocumentProperties" />
    /// <seealso cref="IEquatable{IDocumentPropertiesRow}" />
    public interface IDocumentPropertiesRow : IPropertiesRow, IDocumentProperties, IEquatable<IDocumentPropertiesRow> { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IAudioProperties" />
    /// <seealso cref="IEquatable{IAudioPropertiesRow}" />
    public interface IAudioPropertiesRow : IPropertiesRow, IAudioProperties, IEquatable<IAudioPropertiesRow> { }

    /// <summary>
    /// Generic interface for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IDRMProperties" />
    /// <seealso cref="IEquatable{IDRMPropertiesRow}" />
    public interface IDRMPropertiesRow : IPropertiesRow, IDRMProperties, IEquatable<IDRMPropertiesRow> { }

    /// <summary>
    /// Generic interface for entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IGPSProperties" />
    /// <seealso cref="IEquatable{IGPSPropertiesRow}" />
    public interface IGPSPropertiesRow : IPropertiesRow, IGPSProperties, IEquatable<IGPSPropertiesRow> { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IImageProperties" />
    /// <seealso cref="IEquatable{IImagePropertiesRow}" />
    public interface IImagePropertiesRow : IPropertiesRow, IImageProperties, IEquatable<IImagePropertiesRow> { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IMediaProperties" />
    /// <seealso cref="IEquatable{IMediaPropertiesRow}" />
    public interface IMediaPropertiesRow : IPropertiesRow, IMediaProperties, IEquatable<IMediaPropertiesRow> { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IMusicProperties" />
    /// <seealso cref="IEquatable{IMusicPropertiesRow}" />
    public interface IMusicPropertiesRow : IPropertiesRow, IMusicProperties, IEquatable<IMusicPropertiesRow> { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IPhotoProperties" />
    /// <seealso cref="IEquatable{IPhotoPropertiesRow}" />
    public interface IPhotoPropertiesRow : IPropertiesRow, IPhotoProperties, IEquatable<IPhotoPropertiesRow> { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IRecordedTVProperties" />
    /// <seealso cref="IEquatable{IRecordedTVPropertiesRow}" />
    public interface IRecordedTVPropertiesRow : IPropertiesRow, IRecordedTVProperties, IEquatable<IRecordedTVPropertiesRow> { }

    /// <summary>
    /// Generic interface for entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IVideoProperties" />
    /// <seealso cref="IEquatable{IVideoPropertiesRow}" />
    public interface IVideoPropertiesRow : IPropertiesRow, IVideoProperties, IEquatable<IVideoPropertiesRow> { }
}
