using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IImagePropertiesRow" />
    /// <seealso cref="IImagePropertySet" />
    /// <seealso cref="IEquatable{IImagePropertiesListItem}" />
    /// <seealso cref="Local.Model.ILocalImagePropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamImagePropertiesListItem" />
    /// <seealso cref="IDbContext.ImagePropertiesListing" />
    public interface IImagePropertiesListItem : IPropertiesListItem, IImagePropertiesRow, IEquatable<IImagePropertiesListItem> { }
}