using FsInfoCat.Model;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="Local.Model.ILocalPropertiesListItem" />
    /// <seealso cref="IUpstreamAudioPropertiesListItem" />
    /// <seealso cref="IUpstreamDocumentPropertiesListItem" />
    /// <seealso cref="IUpstreamDRMPropertiesListItem" />
    /// <seealso cref="IUpstreamGPSPropertiesListItem" />
    /// <seealso cref="IUpstreamImagePropertiesListItem" />
    /// <seealso cref="IUpstreamMediaPropertiesListItem" />
    /// <seealso cref="IUpstreamMusicPropertiesListItem" />
    /// <seealso cref="IUpstreamPhotoPropertiesListItem" />
    /// <seealso cref="IUpstreamRecordedTVPropertiesListItem" />
    /// <seealso cref="IUpstreamSummaryPropertiesListItem" />
    /// <seealso cref="IUpstreamVideoPropertiesListItem" />
    public interface IUpstreamPropertiesListItem : IUpstreamPropertiesRow, IPropertiesListItem { }
}
