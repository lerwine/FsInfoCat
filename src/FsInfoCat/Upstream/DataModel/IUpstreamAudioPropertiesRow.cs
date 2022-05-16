namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IAudioPropertiesRow" />
    /// <seealso cref="Local.ILocalAudioPropertiesRow" />
    public interface IUpstreamAudioPropertiesRow : IUpstreamPropertiesRow, IAudioPropertiesRow { }
}
