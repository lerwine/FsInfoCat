using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IImageProperties" />
    /// <seealso cref="IEquatable{IImagePropertiesRow}" />
    /// <seealso cref="Local.ILocalImagePropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamImagePropertiesRow" />
    public interface IImagePropertiesRow : IPropertiesRow, IImageProperties, IEquatable<IImagePropertiesRow> { }
}