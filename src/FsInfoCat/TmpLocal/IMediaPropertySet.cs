using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended media file property values.
    /// </summary>
    /// <seealso cref="ILocalMediaPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="M.IMediaPropertySet" />
    /// <seealso cref="IEquatable{ILocalMediaPropertySet}" />
    /// <seealso cref="Upstream.Model.IMediaPropertySet" />
    public interface ILocalMediaPropertySet : ILocalMediaPropertiesRow, ILocalPropertySet, M.IMediaPropertySet, IEquatable<ILocalMediaPropertySet> { }
}
