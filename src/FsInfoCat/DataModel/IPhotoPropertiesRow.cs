using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IPhotoProperties" />
    /// <seealso cref="IEquatable{IPhotoPropertiesRow}" />
    /// <seealso cref="Local.ILocalPhotoPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamPhotoPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Model.IPhotoPropertiesRow")]
    public interface IPhotoPropertiesRow : IPropertiesRow, IPhotoProperties, IEquatable<IPhotoPropertiesRow> { }
}
