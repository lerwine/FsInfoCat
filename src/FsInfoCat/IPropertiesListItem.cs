using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface IPropertiesListItem : IDbEntity, IHasSimpleIdentifier
    {
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        long ExistingFileCount { get; }

        [Display(Name = nameof(Properties.Resources.DisplayName_AllFiles), ResourceType = typeof(Properties.Resources))]
        long TotalFileCount { get; }
    }

    public interface ISummaryPropertiesListItem : IPropertiesListItem, ISummaryProperties, IEquatable<ISummaryPropertiesListItem> { }

    public interface IDocumentPropertiesListItem : IPropertiesListItem, IDocumentProperties, IEquatable<IDocumentPropertiesListItem> { }

    public interface IAudioPropertiesListItem : IPropertiesListItem, IAudioProperties, IEquatable<IAudioPropertiesListItem> { }

    public interface IDRMPropertiesListItem : IPropertiesListItem, IDRMProperties, IEquatable<IDRMPropertiesListItem> { }

    public interface IGPSPropertiesListItem : IPropertiesListItem, IGPSProperties, IEquatable<IGPSPropertiesListItem> { }

    public interface IImagePropertiesListItem : IPropertiesListItem, IImageProperties, IEquatable<IImagePropertiesListItem> { }

    public interface IMediaPropertiesListItem : IPropertiesListItem, IMediaProperties, IEquatable<IMediaPropertiesListItem> { }

    public interface IMusicPropertiesListItem : IPropertiesListItem, IMusicProperties, IEquatable<IMusicPropertiesListItem> { }

    public interface IPhotoPropertiesListItem : IPropertiesListItem, IPhotoProperties, IEquatable<IPhotoPropertiesListItem> { }

    public interface IRecordedTVPropertiesListItem : IPropertiesListItem, IRecordedTVProperties, IEquatable<IRecordedTVPropertiesListItem> { }

    public interface IVideoPropertiesListItem : IPropertiesListItem, IVideoProperties, IEquatable<IVideoPropertiesListItem> { }
}
