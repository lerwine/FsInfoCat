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
    public interface ILocalAudioPropertySet : ILocalAudioPropertiesRow, ILocalPropertySet, IAudioPropertySet, IEquatable<ILocalAudioPropertySet> { }
}
