using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended music file property values.
    /// </summary>
    /// <seealso cref="ILocalMusicPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="M.IMusicPropertySet" />
    /// <seealso cref="IEquatable{ILocalMusicPropertySet}" />
    /// <seealso cref="Upstream.Model.IMusicPropertySet" />
    public interface ILocalMusicPropertySet : ILocalMusicPropertiesRow, ILocalPropertySet, M.IMusicPropertySet, IEquatable<ILocalMusicPropertySet> { }
}
