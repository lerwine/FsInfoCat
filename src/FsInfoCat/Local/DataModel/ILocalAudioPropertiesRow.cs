namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IAudioPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamAudioPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalAudioPropertiesRow")]
    public interface ILocalAudioPropertiesRow : ILocalPropertiesRow, IAudioPropertiesRow { }
}
