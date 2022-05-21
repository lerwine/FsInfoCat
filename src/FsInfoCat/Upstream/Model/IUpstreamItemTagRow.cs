using FsInfoCat.Model;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for an entity that associates a <see cref="IUpstreamTagDefinition"/> with an <see cref="IUpstreamFile"/>, <see cref="IUpstreamSubdirectory"/>
    /// or <see cref="IUpstreamVolume"/>.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IItemTagRow" />
    /// <seealso cref="Local.Model.ILocalItemTagRow" />
    public interface IUpstreamItemTagRow : IUpstreamDbEntity, IItemTagRow { }
}
