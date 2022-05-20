namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for a tag entity that can be associated with <see cref="IUpstreamFile"/>, <see cref="IUpstreamSubdirectory"/> or <see cref="IUpstreamVolume"/> entities.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="ITagDefinitionRow" />
    /// <seealso cref="Local.ILocalTagDefinitionRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamTagDefinitionRow")]
    public interface IUpstreamTagDefinitionRow : IUpstreamDbEntity, ITagDefinitionRow { }
}
