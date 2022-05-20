namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IAudioPropertiesRow" />
    /// <seealso cref="Local.ILocalAudioPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamAudioPropertiesRow")]
    public interface IUpstreamAudioPropertiesRow : IUpstreamPropertiesRow, IAudioPropertiesRow { }
}
