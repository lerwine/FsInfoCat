using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended photo file property values.
    /// </summary>
    /// <seealso cref="IUpstreamPhotoPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IPhotoPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamPhotoPropertySet}" />
    /// <seealso cref="Local.Model.IPhotoPropertySet" />
    public interface IUpstreamPhotoPropertySet : IUpstreamPhotoPropertiesRow, IUpstreamPropertySet, IPhotoPropertySet, IEquatable<IUpstreamPhotoPropertySet> { }
}
