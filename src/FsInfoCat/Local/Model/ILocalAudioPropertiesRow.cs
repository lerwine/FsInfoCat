using FsInfoCat.Model;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for entities containing extended file properties for audio files.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamAudioPropertiesRow" />
public interface ILocalAudioPropertiesRow : ILocalPropertiesRow, IAudioPropertiesRow { }
