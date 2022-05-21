using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IDocumentProperties" />
    /// <seealso cref="IEquatable{IDocumentPropertiesRow}" />
    /// <seealso cref="Local.Model.ILocalDocumentPropertiesRow" />
    /// <seealso cref="Upstream.Model.IUpstreamDocumentPropertiesRow" />
    public interface IDocumentPropertiesRow : IPropertiesRow, IDocumentProperties, IEquatable<IDocumentPropertiesRow> { }
}
