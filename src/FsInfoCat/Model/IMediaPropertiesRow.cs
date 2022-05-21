using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IMediaProperties" />
    /// <seealso cref="IEquatable{IMediaPropertiesRow}" />
    /// <seealso cref="Local.Model.ILocalMediaPropertiesRow" />
    /// <seealso cref="Upstream.Model.IUpstreamMediaPropertiesRow" />
    public interface IMediaPropertiesRow : IPropertiesRow, IMediaProperties, IEquatable<IMediaPropertiesRow> { }
}
