namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IAudioPropertiesRow" />
    public interface IUpstreamAudioPropertiesRow : IUpstreamPropertiesRow, IAudioPropertiesRow { }
}
