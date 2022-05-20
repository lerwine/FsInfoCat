namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="IUpstreamAudioPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IAudioPropertiesListItem" />
    /// <seealso cref="Local.ILocalAudioPropertiesListItem" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamAudioPropertiesListItem")]
    public interface IUpstreamAudioPropertiesListItem : IUpstreamAudioPropertiesRow, IUpstreamPropertiesListItem, IAudioPropertiesListItem { }
}
