namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for an <see cref="ITagDefinition"/> that is associated with an <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="IItemTag" />
    /// <seealso cref="IHasMembershipKeyReference{IVolume, ITagDefinition}" />
    /// <seealso cref="Local.ILocalVolumeTag" />
    /// <seealso cref="Upstream.IUpstreamVolumeTag" />
    /// <seealso cref="IVolume.SharedTags" />
    /// <seealso cref="IVolume.PersonalTags" />
    /// <seealso cref="ITagDefinition.VolumeTags" />
    public interface IVolumeTag : IItemTag, IHasMembershipKeyReference<IVolume, ITagDefinition>
    {
        /// <summary>
        /// Gets the tagged volume.
        /// </summary>
        /// <value>The tagged <see cref="IVolume"/>.</value>
        new IVolume Tagged { get; }
    }
}
