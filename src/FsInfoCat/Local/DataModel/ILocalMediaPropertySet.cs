using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended media file property values.
    /// </summary>
    /// <seealso cref="ILocalMediaPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IMediaPropertySet" />
    /// <seealso cref="IEquatable{ILocalMediaPropertySet}" />
    /// <seealso cref="Upstream.IUpstreamMediaPropertySet" />
    public interface ILocalMediaPropertySet : ILocalMediaPropertiesRow, ILocalPropertySet, IMediaPropertySet, IEquatable<ILocalMediaPropertySet> { }
}
