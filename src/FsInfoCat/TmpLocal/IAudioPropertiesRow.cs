using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="M.IAudioPropertiesRow" />
    /// <seealso cref="Upstream.Model.IAudioPropertiesRow" />
    public interface ILocalAudioPropertiesRow : ILocalPropertiesRow, M.IAudioPropertiesRow { }
}
