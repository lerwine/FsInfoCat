namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for a tag list item entity that can be associated with <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/> or <see cref="ILocalVolume"/> entities.
    /// </summary>
    /// <seealso cref="ITagDefinitionListItem" />
    /// <seealso cref="ILocalTagDefinitionRow" />
    /// <seealso cref="Upstream.IUpstreamTagDefinitionListItem" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalTagDefinitionListItem")]
    public interface ILocalTagDefinitionListItem : ITagDefinitionListItem, ILocalTagDefinitionRow { }
}
