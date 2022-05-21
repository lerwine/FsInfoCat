using FsInfoCat.Model;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for a list item entity that associates an <see cref="IUpstreamTagDefinition"/> with an <see cref="IUpstreamFile"/>, <see cref="IUpstreamSubdirectory"/>
    /// or <see cref="IUpstreamVolume"/>.
    /// </summary>
    /// <seealso cref="IItemTagListItem" />
    /// <seealso cref="IUpstreamItemTagRow" />
    /// <seealso cref="Local.Model.ILocalItemTagListItem" />
    public interface IUpstreamItemTagListItem : IItemTagListItem, IUpstreamItemTagRow { }
}
