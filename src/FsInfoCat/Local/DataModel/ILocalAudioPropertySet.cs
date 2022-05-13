using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended audio file property values.
    /// </summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IAudioPropertySet" />
    public interface ILocalAudioPropertySet : ILocalAudioPropertiesRow, ILocalPropertySet, IAudioPropertySet, IEquatable<ILocalAudioPropertySet> { }
}
