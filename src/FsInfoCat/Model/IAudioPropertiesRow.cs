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
    /// <seealso cref="Upstream.Model.IUpstreamAudioPropertiesRow" />
    /// <seealso cref="Local.Model.ILocalAudioPropertiesRow" />
    public interface IAudioPropertiesRow : IPropertiesRow, IAudioProperties, IEquatable<IAudioPropertiesRow> { }
}
