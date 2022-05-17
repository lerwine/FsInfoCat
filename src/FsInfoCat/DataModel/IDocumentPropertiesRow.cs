using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IDocumentProperties" />
    /// <seealso cref="IEquatable{IDocumentPropertiesRow}" />
    /// <seealso cref="Local.ILocalDocumentPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamDocumentPropertiesRow" />
    public interface IDocumentPropertiesRow : IPropertiesRow, IDocumentProperties, IEquatable<IDocumentPropertiesRow> { }

}