using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IMediaProperties" />
    /// <seealso cref="IEquatable{IMediaPropertiesRow}" />
    /// <seealso cref="Local.ILocalMediaPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamMediaPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Model.IMediaPropertiesRow")]
    public interface IMediaPropertiesRow : IPropertiesRow, IMediaProperties, IEquatable<IMediaPropertiesRow> { }
}
