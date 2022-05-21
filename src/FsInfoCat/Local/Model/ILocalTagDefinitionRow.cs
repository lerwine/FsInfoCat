using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for a tag entity that can be associated with <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/> or <see cref="ILocalVolume"/> entities.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ITagDefinitionRow" />
    /// <seealso cref="Upstream.Model.IUpstreamTagDefinitionRow" />
    public interface ILocalTagDefinitionRow : ILocalDbEntity, ITagDefinitionRow { }
}
