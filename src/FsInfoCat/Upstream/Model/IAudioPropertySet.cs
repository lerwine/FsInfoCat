using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended audio file property values.
    /// </summary>
    /// <seealso cref="IUpstreamAudioPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IAudioPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamAudioPropertySet}" />
    /// <seealso cref="Local.Model.IAudioPropertySet" />
    public interface IUpstreamAudioPropertySet : IUpstreamAudioPropertiesRow, IUpstreamPropertySet, IAudioPropertySet, IEquatable<IUpstreamAudioPropertySet> { }
}
