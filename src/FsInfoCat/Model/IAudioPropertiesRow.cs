using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IAudioProperties" />
    /// <seealso cref="IEquatable{IAudioPropertiesRow}" />
    /// <seealso cref="IAudioPropertiesListItem" />
    /// <seealso cref="IAudioPropertySet" />
    /// <seealso cref="Upstream.IUpstreamAudioPropertiesRow" />
    /// <seealso cref="Local.ILocalAudioPropertiesRow" />
    public interface IAudioPropertiesRow : IPropertiesRow, IAudioProperties, IEquatable<IAudioPropertiesRow> { }
}
