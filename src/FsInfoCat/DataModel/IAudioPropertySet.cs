using System;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of audio files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IAudioProperties" />
    /// <seealso cref="Local.ILocalAudioPropertySet" />
    /// <seealso cref="Upstream.IUpstreamAudioPropertySet" />
    public interface IAudioPropertySet : IPropertySet, IAudioPropertiesRow, IEquatable<IAudioPropertySet> { }
}
