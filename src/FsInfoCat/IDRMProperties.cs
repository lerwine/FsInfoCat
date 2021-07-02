namespace FsInfoCat
{
    /// <summary>
    /// Represents extended file properties for DRM information.
    /// </summary>
    /// <seealso cref="IDRMPropertySet"/>
    /// <seealso cref="Local.ILocalDRMPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamDRMPropertySet"/>
    /// <seealso cref="FilePropertiesComparer.Equals(IDRMProperties, IDRMProperties)"/>
    /// <seealso cref="Local.IFileDetailProvider.GetDRMPropertiesAsync(System.Threading.CancellationToken)"/>
    /// <seealso cref="IDbContext.FindMatchingAsync(IDRMProperties, System.Threading.CancellationToken)"/>
    public interface IDRMProperties
    {
        /// <summary>
        /// Indicates when play expires for digital rights management.
        /// </summary>
        /// <value>
        /// Indicates when play rights expire.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Date Play Expires</description></item>
        /// <item><term>Format ID</term><description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description></item>
        /// <item><term>Property ID</term><description>6</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-dateplayexpires">[Reference Link]</a></description></item>
        /// </list></remarks>
        System.DateTime? DatePlayExpires { get; }

        /// <summary>
        /// Indicates when play starts for digital rights management.
        /// </summary>
        /// <value>
        /// Indicates when play rights begin.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Date Play Starts</description></item>
        /// <item><term>Format ID</term><description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description></item>
        /// <item><term>Property ID</term><description>5</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-dateplaystarts">[Reference Link]</a></description></item>
        /// </list></remarks>
        System.DateTime? DatePlayStarts { get; }

        /// <su
        /// <summary>
        /// Displays the description for digital rights management.
        /// </summary>
        /// <value>
        /// Displays the description for Digital Rights Management (DRM).
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>License Description</description></item>
        /// <item><term>Format ID</term><description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description></item>
        /// <item><term>Property ID</term><description>3</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-description">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Description { get; }

        /// <summary>
        /// Indicates whether the content is protected
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the content of the file is protected; <see langword="false"/> if the file content is unprotected; otherwise, <see langword="null"/> if this value is not specified.
        /// </value>
        /// <remarks>Indicates whether the file is protected under Digital Rights Management (DRM).
        /// <list type="bullet">
        /// <item><term>Name</term><description>Is Protected</description></item>
        /// <item><term>Format ID</term><description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description></item>
        /// <item><term>Property ID</term><description>2</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-isprotected">[Reference Link]</a></description></item>
        /// </list></remarks>
        /// <remarks>ID: {AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED}, 2 (DRM)</remarks>
        bool? IsProtected { get; }

        /// <summary>
        /// Indicates the play count for digital rights management.
        /// </summary>
        /// <value>
        /// Indicates the number of times the file has been played.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Plays Remaining</description></item>
        /// <item><term>Format ID</term><description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description></item>
        /// <item><term>Property ID</term><description>4</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-playcount">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? PlayCount { get; }
    }
}