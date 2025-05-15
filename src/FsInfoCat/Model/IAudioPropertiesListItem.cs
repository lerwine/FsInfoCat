using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for audio files.
    /// </summary>
    /// <seealso cref="IAudioPropertySet" />
    /// <seealso cref="IDbContext.AudioPropertiesListing"/>
    public interface IAudioPropertiesListItem : IPropertiesListItem, IAudioPropertiesRow, IEquatable<IAudioPropertiesListItem> { }
}
