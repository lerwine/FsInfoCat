using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    /// <summary>Base interface for entities that represent a grouping of extended file properties.</summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IPropertySet" />
    public interface ILocalPropertySet : ILocalDbEntity, IPropertySet
    {
        /// <summary>Gets the files that share the same property values as this property set.</summary>
        /// <value>The <see cref="ILocalFile">files</see> that share the same property values as this property set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalFile> Files { get; }
    }

    public interface ILocalPropertiesListItem : IPropertiesListItem, ILocalDbEntity { }

    public interface ILocalSummaryPropertiesListItem : ILocalPropertiesListItem, ISummaryPropertiesListItem { }

    public interface ILocalDocumentPropertiesListItem : ILocalPropertiesListItem, IDocumentPropertiesListItem { }

    public interface ILocalAudioPropertiesListItem : ILocalPropertiesListItem, IAudioPropertiesListItem { }

    public interface ILocalDRMPropertiesListItem : ILocalPropertiesListItem, IDRMPropertiesListItem { }

    public interface ILocalGPSPropertiesListItem : ILocalPropertiesListItem, IGPSPropertiesListItem { }

    public interface ILocalImagePropertiesListItem : ILocalPropertiesListItem, IImagePropertiesListItem { }

    public interface ILocalMediaPropertiesListItem : ILocalPropertiesListItem, IMediaPropertiesListItem { }

    public interface ILocalMusicPropertiesListItem : ILocalPropertiesListItem, IMusicPropertiesListItem { }

    public interface ILocalPhotoPropertiesListItem : ILocalPropertiesListItem, IPhotoPropertiesListItem { }

    public interface ILocalRecordedTVPropertiesListItem : ILocalPropertiesListItem, IRecordedTVPropertiesListItem { }

    public interface ILocalVideoPropertiesListItem : ILocalPropertiesListItem, IVideoPropertiesListItem { }
}
