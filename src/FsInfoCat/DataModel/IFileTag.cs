namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for an <see cref="ITagDefinition"/> that is associated with an <see cref="IFile"/>.
    /// </summary>
    /// <seealso cref="IItemTag" />
    /// <seealso cref="IHasMembershipKeyReference{IFile, ITagDefinition}" />
    /// <seealso cref="Local.ILocalFileTag" />
    /// <seealso cref="Upstream.IUpstreamFileTag" />
    /// <seealso cref="IFile.SharedTags" />
    /// <seealso cref="IFile.PersonalTags" />
    /// <seealso cref="ITagDefinition.FileTags" />
    [System.Obsolete("Use FsInfoCat.Model.IFileTag")]
    public interface IFileTag : IItemTag, IHasMembershipKeyReference<IFile, ITagDefinition>
    {
        /// <summary>
        /// Gets the tagged file.
        /// </summary>
        /// <value>The tagged <see cref="IFile"/>.</value>
        new IFile Tagged { get; }
    }
}
