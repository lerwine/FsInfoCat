namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for a list item entity that associates an <see cref="IUpstreamTagDefinition"/> with an <see cref="IUpstreamFile"/>, <see cref="IUpstreamSubdirectory"/>
    /// or <see cref="IUpstreamVolume"/>.
    /// </summary>
    /// <seealso cref="IItemTagListItem" />
    /// <seealso cref="IUpstreamItemTagRow" />
    /// <seealso cref="Local.ILocalItemTagListItem" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamItemTagListItem")]
    public interface IUpstreamItemTagListItem : IItemTagListItem, IUpstreamItemTagRow { }
}
