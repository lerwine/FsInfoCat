using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IImageProperties" />
    /// <seealso cref="IEquatable{IImagePropertiesRow}" />
    /// <seealso cref="Local.Model.ILocalImagePropertiesRow" />
    /// <seealso cref="Upstream.Model.IUpstreamImagePropertiesRow" />
    public interface IImagePropertiesRow : IPropertiesRow, IImageProperties, IEquatable<IImagePropertiesRow> { }
}
