using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Contains extended audio file property values.
    /// </summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IAudioPropertySet" />
    public interface IUpstreamAudioPropertySet : IUpstreamPropertySet, IAudioPropertySet, IEquatable<IUpstreamAudioPropertySet> { }
}
