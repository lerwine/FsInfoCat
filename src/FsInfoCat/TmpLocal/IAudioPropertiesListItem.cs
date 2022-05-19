using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="ILocalAudioPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="M.IAudioPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IAudioPropertiesListItem" />
    public interface ILocalAudioPropertiesListItem : ILocalAudioPropertiesRow, ILocalPropertiesListItem, M.IAudioPropertiesListItem { }
}
