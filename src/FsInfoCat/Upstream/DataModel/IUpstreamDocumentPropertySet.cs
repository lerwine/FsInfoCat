using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Contains extended document file property values.
    /// </summary>
    /// <seealso cref="IUpstreamDocumentPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IDocumentPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamDocumentPropertySet}" />
    /// <seealso cref="Local.ILocalDocumentPropertySet" />
    public interface IUpstreamDocumentPropertySet : IUpstreamDocumentPropertiesRow, IUpstreamPropertySet, IDocumentPropertySet, IEquatable<IUpstreamDocumentPropertySet> { }
}