using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended music file property values.
    /// </summary>
    /// <seealso cref="ILocalMusicPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IMusicPropertySet" />
    /// <seealso cref="IEquatable{ILocalMusicPropertySet}" />
    /// <seealso cref="Upstream.Model.IMusicPropertySet" />
    public interface ILocalMusicPropertySet : ILocalMusicPropertiesRow, ILocalPropertySet, IMusicPropertySet, IEquatable<ILocalMusicPropertySet> { }
}
