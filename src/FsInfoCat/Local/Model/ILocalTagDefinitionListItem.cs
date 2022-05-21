using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for a tag list item entity that can be associated with <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/> or <see cref="ILocalVolume"/> entities.
    /// </summary>
    /// <seealso cref="ITagDefinitionListItem" />
    /// <seealso cref="ILocalTagDefinitionRow" />
    /// <seealso cref="Upstream.Model.IUpstreamTagDefinitionListItem" />
    public interface ILocalTagDefinitionListItem : ITagDefinitionListItem, ILocalTagDefinitionRow { }
}
