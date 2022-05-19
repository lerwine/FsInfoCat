using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended audio file property values.
    /// </summary>
    /// <seealso cref="ILocalAudioPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="M.IAudioPropertySet" />
    /// <seealso cref="IEquatable{ILocalAudioPropertySet}" />
    /// <seealso cref="Upstream.Model.IAudioPropertySet" />
    public interface ILocalAudioPropertySet : ILocalAudioPropertiesRow, ILocalPropertySet, M.IAudioPropertySet, IEquatable<ILocalAudioPropertySet> { }
}
