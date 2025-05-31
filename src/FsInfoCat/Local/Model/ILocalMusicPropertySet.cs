using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Contains extended music file property values.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamMusicPropertySet" />
public interface ILocalMusicPropertySet : ILocalMusicPropertiesRow, ILocalPropertySet, IMusicPropertySet, IEquatable<ILocalMusicPropertySet> { }
