namespace FsInfoCat
{
    public interface IDRMProperties
    {
        /// <summary>
        /// Indicates when play expires for digital rights management.
        /// </summary>
        /// <remarks>ID: {AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED}, 6 (DRM)</remarks>
        System.DateTime? DatePlayExpires { get; }

        /// <summary>
        /// Indicates when play starts for digital rights management.
        /// </summary>
        /// <remarks>ID: {AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED}, 5 (DRM)</remarks>
        System.DateTime? DatePlayStarts { get; }

        /// <summary>
        /// Displays the description for digital rights management.
        /// </summary>
        /// <remarks>ID: {AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED}, 3 (DRM)</remarks>
        string Description { get; }

        /// <summary>
        /// Indicates whether the content is protected
        /// </summary>
        /// <value><see langword="true"/> if the content of the file is protected; <see langword="false"/> if the file content is unprotected; otherwise, <see langword="null"/> if this value is not specified.</value>
        /// <remarks>ID: {AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED}, 2 (DRM)</remarks>
        bool? IsProtected { get; }

        /// <summary>
        /// Indicates the play count for digital rights management.
        /// </summary>
        /// <remarks>ID: {AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED}, 4 (DRM)</remarks>
        uint? PlayCount { get; }
    }
}
