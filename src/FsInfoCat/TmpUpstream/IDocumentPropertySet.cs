using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended document file property values.
    /// </summary>
    /// <seealso cref="IUpstreamDocumentPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="M.IDocumentPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamDocumentPropertySet}" />
    /// <seealso cref="Local.Model.IDocumentPropertySet" />
    public interface IUpstreamDocumentPropertySet : IUpstreamDocumentPropertiesRow, IUpstreamPropertySet, M.IDocumentPropertySet, IEquatable<IUpstreamDocumentPropertySet> { }
}
