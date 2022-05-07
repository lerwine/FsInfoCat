using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface IPropertiesListItem : IPropertiesRow
    {
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        long ExistingFileCount { get; }

        [Display(Name = nameof(Properties.Resources.DisplayName_AllFiles), ResourceType = typeof(Properties.Resources))]
        long TotalFileCount { get; }
    }

    public interface ISummaryPropertiesListItem : IPropertiesListItem, ISummaryPropertiesRow, IEquatable<ISummaryPropertiesListItem> { }

    public interface IDocumentPropertiesListItem : IPropertiesListItem, IDocumentPropertiesRow, IEquatable<IDocumentPropertiesListItem> { }

    public interface IAudioPropertiesListItem : IPropertiesListItem, IAudioPropertiesRow, IEquatable<IAudioPropertiesListItem> { }

    public interface IDRMPropertiesListItem : IPropertiesListItem, IDRMPropertiesRow, IEquatable<IDRMPropertiesListItem> { }

    public interface IGPSPropertiesListItem : IPropertiesListItem, IGPSPropertiesRow, IEquatable<IGPSPropertiesListItem> { }

    public interface IImagePropertiesListItem : IPropertiesListItem, IImagePropertiesRow, IEquatable<IImagePropertiesListItem> { }

    public interface IMediaPropertiesListItem : IPropertiesListItem, IMediaPropertiesRow, IEquatable<IMediaPropertiesListItem> { }

    public interface IMusicPropertiesListItem : IPropertiesListItem, IMusicPropertiesRow, IEquatable<IMusicPropertiesListItem> { }

    public interface IPhotoPropertiesListItem : IPropertiesListItem, IPhotoPropertiesRow, IEquatable<IPhotoPropertiesListItem> { }

    public interface IRecordedTVPropertiesListItem : IPropertiesListItem, IRecordedTVPropertiesRow, IEquatable<IRecordedTVPropertiesListItem> { }

    public interface IVideoPropertiesListItem : IPropertiesListItem, IVideoPropertiesRow, IEquatable<IVideoPropertiesListItem> { }
}
