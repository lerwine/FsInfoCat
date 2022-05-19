using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IDocumentPropertiesRow" />
    /// <seealso cref="IDocumentPropertySet" />
    /// <seealso cref="IEquatable{IDocumentPropertiesListItem}" />
    /// <seealso cref="Local.ILocalDocumentPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamDocumentPropertiesListItem" />
    /// <seealso cref="IDbContext.DocumentPropertiesListing" />
    [System.Obsolete("Use FsInfoCat.Model.IDocumentPropertiesListItem")]
    public interface IDocumentPropertiesListItem : IPropertiesListItem, IDocumentPropertiesRow, IEquatable<IDocumentPropertiesListItem> { }
}
