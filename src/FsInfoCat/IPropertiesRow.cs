using System;

namespace FsInfoCat
{
    public interface IPropertiesRow : IDbEntity, IHasSimpleIdentifier { }

    public interface ISummaryPropertiesRow : IPropertiesRow, ISummaryProperties, IEquatable<ISummaryPropertiesRow> { }

    public interface IDocumentPropertiesRow : IPropertiesRow, IDocumentProperties, IEquatable<IDocumentPropertiesRow> { }

    public interface IAudioPropertiesRow : IPropertiesRow, IAudioProperties, IEquatable<IAudioPropertiesRow> { }

    public interface IDRMPropertiesRow : IPropertiesRow, IDRMProperties, IEquatable<IDRMPropertiesRow> { }

    public interface IGPSPropertiesRow : IPropertiesRow, IGPSProperties, IEquatable<IGPSPropertiesRow> { }

    public interface IImagePropertiesRow : IPropertiesRow, IImageProperties, IEquatable<IImagePropertiesRow> { }

    public interface IMediaPropertiesRow : IPropertiesRow, IMediaProperties, IEquatable<IMediaPropertiesRow> { }

    public interface IMusicPropertiesRow : IPropertiesRow, IMusicProperties, IEquatable<IMusicPropertiesRow> { }

    public interface IPhotoPropertiesRow : IPropertiesRow, IPhotoProperties, IEquatable<IPhotoPropertiesRow> { }

    public interface IRecordedTVPropertiesRow : IPropertiesRow, IRecordedTVProperties, IEquatable<IRecordedTVPropertiesRow> { }

    public interface IVideoPropertiesRow : IPropertiesRow, IVideoProperties, IEquatable<IVideoPropertiesRow> { }
}
