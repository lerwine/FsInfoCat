namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="ILocalAudioPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IAudioPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamAudioPropertiesListItem" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalAudioPropertiesListItem")]
    public interface ILocalAudioPropertiesListItem : ILocalAudioPropertiesRow, ILocalPropertiesListItem, IAudioPropertiesListItem { }
}
