using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended audio file property values.
    /// </summary>
    /// <seealso cref="ILocalAudioPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IAudioPropertySet" />
    /// <seealso cref="IEquatable{ILocalAudioPropertySet}" />
    /// <seealso cref="Upstream.Model.IAudioPropertySet" />
    public interface ILocalAudioPropertySet : ILocalAudioPropertiesRow, ILocalPropertySet, IAudioPropertySet, IEquatable<ILocalAudioPropertySet> { }
}
