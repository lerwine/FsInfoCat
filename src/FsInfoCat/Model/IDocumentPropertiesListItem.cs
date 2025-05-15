using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="IDocumentPropertySet" />
    /// <seealso cref="IDbContext.DocumentPropertiesListing"/>
    public interface IDocumentPropertiesListItem : IPropertiesListItem, IDocumentPropertiesRow, IEquatable<IDocumentPropertiesListItem> { }
}
