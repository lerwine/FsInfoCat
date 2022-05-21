using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IAudioPropertiesRow" />
    /// <seealso cref="IEquatable{IAudioPropertiesListItem}" />
    /// <seealso cref="Upstream.Model.IUpstreamAudioPropertiesListItem" />
    /// <seealso cref="Local.Model.ILocalAudioPropertiesListItem" />
    /// <seealso cref="IDbContext.AudioPropertiesListing" />
    public interface IAudioPropertiesListItem : IPropertiesListItem, IAudioPropertiesRow, IEquatable<IAudioPropertiesListItem> { }
}
