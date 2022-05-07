using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Base interface for entities that represent a grouping of extended file properties.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IPropertySet" />
    public interface IUpstreamPropertySet : IUpstreamDbEntity, IPropertySet
    {
        /// <summary>
        /// Gets the files that share the same property values as this property set.
        /// </summary>
        /// <value>The <see cref="IUpstreamFile">files</see> that share the same property values as this property set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamFile> Files { get; }
    }

    public interface IUpstreamPropertiesListItem : IPropertiesListItem, IUpstreamDbEntity { }

    public interface IUpstreamSummaryPropertiesListItem : IUpstreamPropertiesListItem, ISummaryPropertiesListItem { }

    public interface IUpstreamDocumentPropertiesListItem : IUpstreamPropertiesListItem, IDocumentPropertiesListItem { }

    public interface IUpstreamAudioPropertiesListItem : IUpstreamPropertiesListItem, IAudioPropertiesListItem { }

    public interface IUpstreamDRMPropertiesListItem : IUpstreamPropertiesListItem, IDRMPropertiesListItem { }

    public interface IUpstreamGPSPropertiesListItem : IUpstreamPropertiesListItem, IGPSPropertiesListItem { }

    public interface IUpstreamImagePropertiesListItem : IUpstreamPropertiesListItem, IImagePropertiesListItem { }

    public interface IUpstreamMediaPropertiesListItem : IUpstreamPropertiesListItem, IMediaPropertiesListItem { }

    public interface IUpstreamMusicPropertiesListItem : IUpstreamPropertiesListItem, IMusicPropertiesListItem { }

    public interface IUpstreamPhotoPropertiesListItem : IUpstreamPropertiesListItem, IPhotoPropertiesListItem { }

    public interface IUpstreamRecordedTVPropertiesListItem : IUpstreamPropertiesListItem, IRecordedTVPropertiesListItem { }

    public interface IUpstreamVideoPropertiesListItem : IUpstreamPropertiesListItem, IVideoPropertiesListItem { }
}
