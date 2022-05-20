namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for a tag list item entity that can be associated with <see cref="IUpstreamFile"/>, <see cref="IUpstreamSubdirectory"/> or
    /// <see cref="IUpstreamVolume"/> entities.
    /// </summary>
    /// <seealso cref="ITagDefinitionListItem" />
    /// <seealso cref="IUpstreamTagDefinitionRow" />
    /// <seealso cref="Local.ILocalTagDefinitionListItem" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamTagDefinitionListItem")]
    public interface IUpstreamTagDefinitionListItem : ITagDefinitionListItem, IUpstreamTagDefinitionRow { }
}
