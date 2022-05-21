namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="ISharedTagDefinition"/> with an <see cref="IFile"/>, <see cref="ISubdirectory"/> or <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="IItemTag" />
    /// <seealso cref="Local.Model.ILocalSharedTag" />
    /// <seealso cref="Upstream.Model.IUpstreamSharedTag" />
    /// <seealso cref="ISharedTagDefinition.FileTags" />
    /// <seealso cref="ISharedTagDefinition.SubdirectoryTags" />
    /// <seealso cref="ISharedTagDefinition.VolumeTags" />
    public interface ISharedTag : IItemTag
    {
        /// <summary>
        /// Gets the tag definition.
        /// </summary>
        /// <value>The tag definition that is associated with the <see cref="IDbEntity"/>.</value>
        new ISharedTagDefinition Definition { get; }
    }
}
