using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Contains extended media file property values.
    /// </summary>
    /// <seealso cref="IUpstreamMediaPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IMediaPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamMediaPropertySet}" />
    /// <seealso cref="Local.ILocalMediaPropertySet" />
    public interface IUpstreamMediaPropertySet : IUpstreamMediaPropertiesRow, IUpstreamPropertySet, IMediaPropertySet, IEquatable<IUpstreamMediaPropertySet> { }
}
