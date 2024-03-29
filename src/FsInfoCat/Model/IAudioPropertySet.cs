using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of audio files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IAudioPropertiesRow" />
    /// <seealso cref="IEquatable{IAudioPropertySet}" />
    /// <seealso cref="Upstream.Model.IUpstreamAudioPropertySet" />
    /// <seealso cref="Local.Model.ILocalAudioPropertySet" />
    /// <seealso cref="IDbContext.AudioPropertySets" />
    /// <seealso cref="IFile.AudioProperties" />
    public interface IAudioPropertySet : IPropertySet, IAudioPropertiesRow, IEquatable<IAudioPropertySet> { }
}
