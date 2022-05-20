using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Contains extended photo file property values.
    /// </summary>
    /// <seealso cref="IUpstreamPhotoPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IPhotoPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamPhotoPropertySet}" />
    /// <seealso cref="Local.ILocalPhotoPropertySet" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamPhotoPropertySet")]
    public interface IUpstreamPhotoPropertySet : IUpstreamPhotoPropertiesRow, IUpstreamPropertySet, IPhotoPropertySet, IEquatable<IUpstreamPhotoPropertySet> { }
}
