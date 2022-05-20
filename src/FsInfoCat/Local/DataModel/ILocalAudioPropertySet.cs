using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended audio file property values.
    /// </summary>
    /// <seealso cref="ILocalAudioPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IAudioPropertySet" />
    /// <seealso cref="IEquatable{ILocalAudioPropertySet}" />
    /// <seealso cref="Upstream.IUpstreamAudioPropertySet" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalAudioPropertySet")]
    public interface ILocalAudioPropertySet : ILocalAudioPropertiesRow, ILocalPropertySet, IAudioPropertySet, IEquatable<ILocalAudioPropertySet> { }
}
