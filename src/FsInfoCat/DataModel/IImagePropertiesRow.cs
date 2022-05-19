using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IImageProperties" />
    /// <seealso cref="IEquatable{IImagePropertiesRow}" />
    /// <seealso cref="Local.ILocalImagePropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamImagePropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Model.IImagePropertiesRow")]
    public interface IImagePropertiesRow : IPropertiesRow, IImageProperties, IEquatable<IImagePropertiesRow> { }
}
