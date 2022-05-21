using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IAudioPropertiesRow" />
    /// <seealso cref="Local.Model.ILocalAudioPropertiesRow" />
    public interface IUpstreamAudioPropertiesRow : IUpstreamPropertiesRow, IAudioPropertiesRow { }
}
