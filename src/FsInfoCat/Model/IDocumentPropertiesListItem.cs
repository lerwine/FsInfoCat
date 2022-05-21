using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IDocumentPropertiesRow" />
    /// <seealso cref="IDocumentPropertySet" />
    /// <seealso cref="IEquatable{IDocumentPropertiesListItem}" />
    /// <seealso cref="Local.Model.ILocalDocumentPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamDocumentPropertiesListItem" />
    /// <seealso cref="IDbContext.DocumentPropertiesListing" />
    public interface IDocumentPropertiesListItem : IPropertiesListItem, IDocumentPropertiesRow, IEquatable<IDocumentPropertiesListItem> { }
}
