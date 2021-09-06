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

    public interface ISummaryPropertiesListItem : IPropertiesListItem, ISummaryProperties { }

    public interface IDocumentPropertiesListItem : IPropertiesListItem, IDocumentProperties { }

    public interface IAudioPropertiesListItem : IPropertiesListItem, IAudioProperties { }

    public interface IDRMPropertiesListItem : IPropertiesListItem, IDRMProperties { }

    public interface IGPSPropertiesListItem : IPropertiesListItem, IGPSProperties { }

    public interface IImagePropertiesListItem : IPropertiesListItem, IImageProperties { }

    public interface IMediaPropertiesListItem : IPropertiesListItem, IMediaProperties { }

    public interface IMusicPropertiesListItem : IPropertiesListItem, IMusicProperties { }

    public interface IPhotoPropertiesListItem : IPropertiesListItem, IPhotoProperties { }

    public interface IRecordedTVPropertiesListItem : IPropertiesListItem, IRecordedTVProperties { }

    public interface IVideoPropertiesListItem : IPropertiesListItem, IVideoProperties { }
}

