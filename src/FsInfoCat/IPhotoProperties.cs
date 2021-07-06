using FsInfoCat.Collections;

namespace FsInfoCat
{
    /// <summary>
    /// Represents extended file properties for photo files.
    /// </summary>
    /// <seealso cref="IPhotoPropertySet"/>
    /// <seealso cref="Local.ILocalPhotoPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamPhotoPropertySet"/>
    /// <seealso cref="FilePropertiesComparer.Equals(IPhotoProperties, IPhotoProperties)"/>
    /// <seealso cref="Local.IFileDetailProvider.GetPhotoPropertiesAsync(System.Threading.CancellationToken)"/>
    /// <seealso cref="IDbContext.FindMatchingAsync(IPhotoProperties, System.Threading.CancellationToken)"/>
    public interface IPhotoProperties
    {
        /// <summary>
        /// Gets the Camera Manufacturer
        /// </summary>
        /// <value>
        /// The manufacturer name of the camera that took the photo, in a string format.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Camera Manufacturer</description></item>
        /// <item><term>Format ID</term><description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description></item>
        /// <item><term>Property ID</term><description>271</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-cameramanufacturer">[Reference Link]</a></description></item>
        /// </list></remarks>
        string CameraManufacturer { get; }

        /// <summary>
        /// Gets the Camera Model
        /// </summary>
        /// <value>
        /// The model name of the camera that shot the photo, in string form.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Camera Model</description></item>
        /// <item><term>Format ID</term><description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description></item>
        /// <item><term>Property ID</term><description>272</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-cameramodel">[Reference Link]</a></description></item>
        /// </list></remarks>
        string CameraModel { get; }

        /// <summary>
        /// Gets the Date Taken
        /// </summary>
        /// <value>
        /// The date when the photo was taken, as read from the camera in the file's Exchangeable Image File (EXIF) tag.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Date Taken</description></item>
        /// <item><term>Format ID</term><description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description></item>
        /// <item><term>Property ID</term><description>36867</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-datetaken">[Reference Link]</a></description></item>
        /// </list></remarks>
        System.DateTime? DateTaken { get; }

        /// <summary>
        /// Return the event at which the photo was taken
        /// </summary>
        /// <value>
        /// The event where the photo was taken.
        /// </value>
        /// <remarks>
        /// The end-user provides this value.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Event Name</description></item>
        /// <item><term>Format ID</term><description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description></item>
        /// <item><term>Property ID</term><description>18248</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-event">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue Event { get; }

        /// <summary>
        /// Returns the EXIF version.
        /// </summary>
        /// <value>
        /// The Exchangeable Image File (EXIF) version.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>EXIF Version</description></item>
        /// <item><term>Format ID</term><description>{D35F743A-EB2E-47F2-A286-844132CB1427} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-exifversion">[Reference Link]</a></description></item>
        /// </list></remarks>
        string EXIFVersion { get; }

        /// <summary>
        /// Gets the Orientation
        /// </summary>
        /// <value>
        /// The orientation of the photo when it was taken, as specified in the Exchangeable Image File (EXIF) information and in terms of rows and columns.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Orientation</description></item>
        /// <item><term>Format ID</term><description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description></item>
        /// <item><term>Property ID</term><description>274</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-orientation">[Reference Link]</a></description></item>
        /// </list></remarks>
        ushort? Orientation { get; }

        /// <summary>
        /// The user-friendly form of System.Photo.Orientation
        /// </summary>
        /// <value>
        /// The user-friendly form of System.
        /// </value>
        /// <remarks>This value should be trimmed, with white-space-only converted to <see langword="null"/>.
        /// <para>Not intended to be parsed programmatically.</para>
        /// <list type="bullet">
        /// <item><term>Name</term><description>Orientation</description></item>
        /// <item><term>Format ID</term><description>{A9EA193C-C511-498A-A06B-58E2776DCC28} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-orientationtext">[Reference Link]</a></description></item>
        /// </list></remarks>
        string OrientationText { get; }

        /// <summary>
        /// The people tags on an image.
        /// </summary>
        /// <value>
        /// The people tags on an image.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>People Tags</description></item>
        /// <item><term>Format ID</term><description>{E8309B6E-084C-49B4-B1FC-90A80331B638} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-peoplenames">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue PeopleNames { get; }
    }
}
