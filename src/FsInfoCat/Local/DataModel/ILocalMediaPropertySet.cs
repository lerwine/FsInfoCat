using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended media file property values.
    /// </summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IMediaPropertySet" />
    public interface ILocalMediaPropertySet : ILocalMediaPropertiesRow, ILocalPropertySet, IMediaPropertySet, IEquatable<ILocalMediaPropertySet> { }
}
