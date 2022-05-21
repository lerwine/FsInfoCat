namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="IPersonalTagDefinition"/> with an <see cref="IFile"/>, <see cref="ISubdirectory"/> or <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="IItemTag" />
    /// <seealso cref="Local.Model.ILocalPersonalTag" />
    /// <seealso cref="Upstream.Model.IUpstreamPersonalTag" />
    /// <seealso cref="IPersonalTagDefinition.FileTags" />
    /// <seealso cref="IPersonalTagDefinition.SubdirectoryTags" />
    /// <seealso cref="IPersonalTagDefinition.VolumeTags" />
    public interface IPersonalTag : IItemTag
    {
        /// <summary>
        /// Gets the personal tag definition.
        /// </summary>
        /// <value>The personal tag definition that is associated with the <see cref="IDbEntity"/>.</value>
        new IPersonalTagDefinition Definition { get; }
    }
}
