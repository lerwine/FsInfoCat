using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamPropertiesListItem" />
    /// <seealso cref="ILocalAudioPropertiesListItem" />
    /// <seealso cref="ILocalDocumentPropertiesListItem" />
    /// <seealso cref="ILocalDRMPropertiesListItem" />
    /// <seealso cref="ILocalGPSPropertiesListItem" />
    /// <seealso cref="ILocalImagePropertiesListItem" />
    /// <seealso cref="ILocalMediaPropertiesListItem" />
    /// <seealso cref="ILocalMusicPropertiesListItem" />
    /// <seealso cref="ILocalPhotoPropertiesListItem" />
    /// <seealso cref="ILocalRecordedTVPropertiesListItem" />
    /// <seealso cref="ILocalSummaryPropertiesListItem" />
    /// <seealso cref="ILocalVideoPropertiesListItem" />
    public interface ILocalPropertiesListItem : ILocalPropertiesRow, IPropertiesListItem { }
}
