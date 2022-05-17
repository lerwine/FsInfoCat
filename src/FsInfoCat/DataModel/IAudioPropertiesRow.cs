using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IAudioProperties" />
    /// <seealso cref="IEquatable{IAudioPropertiesRow}" />
    /// <seealso cref="Local.ILocalAudioPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamAudioPropertiesRow" />
    public interface IAudioPropertiesRow : IPropertiesRow, IAudioProperties, IEquatable<IAudioPropertiesRow> { }
}