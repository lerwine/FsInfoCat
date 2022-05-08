namespace FsInfoCat.Upstream
{
    /// <summary>
    /// List item entity for a device that hosts one or more <see cref="IUpstreamVolume">volumes</see>.
    /// </summary>
    /// <seealso cref="IHostDeviceRow" />
    public interface IHostDeviceListItem : IHostDeviceRow
    {
        /// <summary>
        /// Gets the display name of the associated platform.
        /// </summary>
        /// <value>The <see cref="IHostPlatformRow.DisplayName"/> of the associated <see cref="IHostPlatformRow"/>.</value>
        string PlatformDisplayName { get; }

        /// <summary>
        /// Gets the type of the associated platform.
        /// </summary>
        /// <value>The <see cref="IHostPlatformRow.Type"/> of the associated <see cref="IHostPlatformRow"/>.</value>
        PlatformType PlatformType { get; }

        /// <summary>
        /// Gets the volume count.
        /// </summary>
        /// <value>The number of <see cref="IUpstreamVolume"/> entities associated with the current host.</value>
        long VolumeCount { get; }
    }
}
