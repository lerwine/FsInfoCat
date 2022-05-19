using System;

namespace FsInfoCat
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
    [Obsolete("Use FsInfoCat.Model.IAudioPropertiesRow")]
    public interface IAudioPropertiesRow : IPropertiesRow, IAudioProperties, IEquatable<IAudioPropertiesRow> { }
}
