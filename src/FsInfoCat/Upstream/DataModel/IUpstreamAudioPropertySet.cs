using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Contains extended audio file property values.
    /// </summary>
    /// <seealso cref="IUpstreamAudioPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IAudioPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamAudioPropertySet}" />
    /// <seealso cref="Local.ILocalAudioPropertySet" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamAudioPropertySet")]
    public interface IUpstreamAudioPropertySet : IUpstreamAudioPropertiesRow, IUpstreamPropertySet, IAudioPropertySet, IEquatable<IUpstreamAudioPropertySet> { }
}
