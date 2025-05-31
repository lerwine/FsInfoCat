using FsInfoCat.Model;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for list item entities containing extended file properties for audio files.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamAudioPropertiesListItem" />
public interface ILocalAudioPropertiesListItem : ILocalAudioPropertiesRow, ILocalPropertiesListItem, IAudioPropertiesListItem { }
