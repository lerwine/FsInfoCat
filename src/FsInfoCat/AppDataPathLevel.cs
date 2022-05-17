namespace FsInfoCat
{
    // TODO: Document AppDataPathLevel enum
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public enum AppDataPathLevel
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// The directory that serves as a common repository for version and culture-specific application data that is used by the current, non-roaming user.
        /// </summary>
        /// <remarks>
        /// This refers to a subdirectory named after a specific <see cref="System.Globalization.CultureInfo.Name">Name</see>, and is located within
        /// the <see cref="CurrentVersion">version-specific local application data subdirectory</see>.
        /// </remarks>
        CultureSpecific,

        /// <summary>
        /// The directory that serves as a common repository for culture-invariant, version-specific application data that is used by the current, non-roaming user.
        /// </summary>
        /// <remarks>
        /// This refers to a subdirectory named after the application's <see cref="System.Reflection.AssemblyName.Version">Assembly Version</see>, and is located within
        /// the <see cref="Assembly">general assembly app data subdirectory</see>.
        /// </remarks>
        CurrentVersion,

        /// <summary>
        /// The parent directory containing assembly data that is used by the current, non-roaming user.
        /// </summary>
        /// <remarks>
        /// This refers to a subdirectory named after the application's <see cref="System.Reflection.AssemblyName.Name">Assembly Name</see>, and is located within
        /// the <see cref="Product">product-specific app data subdirectory</see>.
        /// </remarks>
        Assembly,

        /// <summary>
        /// The parent directory for product-specific data that is used by the current, non-roaming user.
        /// </summary>
        /// <remarks>
        /// This refers to a subdirectory named after the application's <see cref="System.Reflection.AssemblyProductAttribute.Product">Product Name</see>, and is located within
        /// the <see cref="Company">company-specific app data subdirectory</see>.
        /// </remarks>
        Product,

        /// <summary>
        /// TThe directory that serves as a common repository for application-independent, company-specific data that is used by the current, non-roaming user.
        /// </summary>
        /// <remarks>This refers to a subdirectory that has the same name as the company which is specified in
        /// the <see cref="System.Reflection.AssemblyCompanyAttribute.Company">AssemblyCompany</see>, and is located within
        /// the <see cref="System.Environment.SpecialFolder.LocalApplicationData"><c>Local Application Data</c> special folder</see>.</remarks>
        Company
    }
}
