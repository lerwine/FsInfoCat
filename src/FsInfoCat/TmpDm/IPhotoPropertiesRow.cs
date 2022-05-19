using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IPhotoProperties" />
    /// <seealso cref="IEquatable{IPhotoPropertiesRow}" />
    /// <seealso cref="Local.ILocalPhotoPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamPhotoPropertiesRow" />
    public interface IPhotoPropertiesRow : IPropertiesRow, IPhotoProperties, IEquatable<IPhotoPropertiesRow> { }
}