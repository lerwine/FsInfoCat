using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Contains extended audio file property values.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamAudioPropertySet" />
public interface ILocalAudioPropertySet : ILocalAudioPropertiesRow, ILocalPropertySet, IAudioPropertySet, IEquatable<ILocalAudioPropertySet> { }
