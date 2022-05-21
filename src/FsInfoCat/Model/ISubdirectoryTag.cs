namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ITagDefinition"/> that is associated with an <see cref="ISubdirectory"/>.
    /// </summary>
    /// <seealso cref="IItemTag" />
    /// <seealso cref="IHasMembershipKeyReference{ISubdirectory, ITagDefinition}" />
    /// <seealso cref="Local.Model.ILocalSubdirectoryTag" />
    /// <seealso cref="Upstream.Model.IUpstreamSubdirectoryTag" />
    /// <seealso cref="ISubdirectory.SharedTags" />
    /// <seealso cref="ISubdirectory.PersonalTags" />
    /// <seealso cref="ITagDefinition.SubdirectoryTags" />
    public interface ISubdirectoryTag : IItemTag, IHasMembershipKeyReference<ISubdirectory, ITagDefinition>
    {
        /// <summary>
        /// Gets the tagged subdirectory.
        /// </summary>
        /// <value>The tagged <see cref="ISubdirectory"/>.</value>
        new ISubdirectory Tagged { get; }
    }
}
