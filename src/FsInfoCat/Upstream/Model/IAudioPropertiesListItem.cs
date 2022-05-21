using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="IUpstreamAudioPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IAudioPropertiesListItem" />
    /// <seealso cref="Local.Model.IAudioPropertiesListItem" />
    public interface IUpstreamAudioPropertiesListItem : IUpstreamAudioPropertiesRow, IUpstreamPropertiesListItem, IAudioPropertiesListItem { }
}
