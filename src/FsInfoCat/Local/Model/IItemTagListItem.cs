using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for a list item entity that associates an <see cref="ILocalTagDefinition"/> with an <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/>
    /// or <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="IItemTagListItem" />
    /// <seealso cref="ILocalItemTagRow" />
    /// <seealso cref="Upstream.Model.IItemTagListItem" />
    public interface ILocalItemTagListItem : IItemTagListItem, ILocalItemTagRow { }
}
