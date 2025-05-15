using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of audio files.
    /// </summary>
    /// <seealso cref="IAudioPropertiesListItem" />
    /// <seealso cref="IDbContext.AudioPropertySets"/>
    public interface IAudioPropertySet : IPropertySet, IAudioPropertiesRow, IEquatable<IAudioPropertySet> { }
}
