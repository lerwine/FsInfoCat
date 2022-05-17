using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended photo file property values.
    /// </summary>
    /// <seealso cref="ILocalPhotoPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IPhotoPropertySet" />
    /// <seealso cref="IEquatable{ILocalPhotoPropertySet}" />
    /// <seealso cref="Upstream.IUpstreamPhotoPropertySet" />
    public interface ILocalPhotoPropertySet : ILocalPhotoPropertiesRow, ILocalPropertySet, IPhotoPropertySet, IEquatable<ILocalPhotoPropertySet> { }
}
