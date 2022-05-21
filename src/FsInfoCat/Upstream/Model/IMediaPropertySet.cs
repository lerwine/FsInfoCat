using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended media file property values.
    /// </summary>
    /// <seealso cref="IUpstreamMediaPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IMediaPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamMediaPropertySet}" />
    /// <seealso cref="Local.Model.IMediaPropertySet" />
    public interface IUpstreamMediaPropertySet : IUpstreamMediaPropertiesRow, IUpstreamPropertySet, IMediaPropertySet, IEquatable<IUpstreamMediaPropertySet> { }
}
