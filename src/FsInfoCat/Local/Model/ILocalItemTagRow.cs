using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an entity that associates a <see cref="ILocalTagDefinition"/> with an <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/>
    /// or <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IItemTagRow" />
    /// <seealso cref="Upstream.Model.IUpstreamItemTagRow" />
    public interface ILocalItemTagRow : ILocalDbEntity, IItemTagRow { }
}
