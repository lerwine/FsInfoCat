using FsInfoCat.Collections;

namespace FsInfoCat
{
    /// <summary>
    /// Represents extended file properties for GPS information.
    /// </summary>
    /// <seealso cref="IGPSPropertySet"/>
    /// <seealso cref="Local.ILocalGPSPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamGPSPropertySet"/>
    /// <seealso cref="FilePropertiesComparer.Equals(IGPSProperties, IGPSProperties)"/>
    /// <seealso cref="Local.IFileDetailProvider.GetGPSPropertiesAsync(System.Threading.CancellationToken)"/>
    /// <seealso cref="IDbContext.FindMatchingAsync(IGPSProperties, System.Threading.CancellationToken)"/>
    public interface IGPSProperties
    {
        /// The name of the GPS area
        /// </summary>
        /// <value>
        /// The name of the GPS area.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Area Information</description></item>
        /// <item><term>Format ID</term><description>{972E333E-AC7E-49F1-8ADF-A70D07A9BCAB} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-areainformation">[Reference Link]</a></description></item>
        /// </list></remarks>
        string AreaInformation { get; }

        /// <summary>
        /// Indicates the latitude degrees.
        /// </summary>
        /// <value>
        /// This is the value at index 0 from an array of three values.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Latitude Degrees</description></item>
        /// <item><term>Format ID</term><description>{8727CFFF-4868-4EC6-AD5B-81B98521D1AB} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latitudedegrees">[Reference Link]</a></description></item>
        /// </list></remarks>
        double? LatitudeDegrees { get; }

        /// <summary>
        /// Indicates the latitude minutes.
        /// </summary>
        /// <value>
        /// This is the value at index 1 from an array of three values.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Latitude Minutes</description></item>
        /// <item><term>Format ID</term><description>{8727CFFF-4868-4EC6-AD5B-81B98521D1AB} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latitudeminutes">[Reference Link]</a></description></item>
        /// </list></remarks>
        double? LatitudeMinutes { get; }

        /// <summary>
        /// Indicates the latitude seconds.
        /// </summary>
        /// <value>
        /// This is the value at index 2 from an array of three values.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Latitude Seconds</description></item>
        /// <item><term>Format ID</term><description>{8727CFFF-4868-4EC6-AD5B-81B98521D1AB} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latitude">[Reference Link]</a></description></item>
        /// </list></remarks>
        double? LatitudeSeconds { get; }

        /// <summary>
        /// Indicates whether latitude is north or south latitude
        /// </summary>
        /// <value>
        /// Indicates whether latitude is north or south.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Latitude Ref</description></item>
        /// <item><term>Format ID</term><description>{029C0252-5B86-46C7-ACA0-2769FFC8E3D4} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latituderef">[Reference Link]</a></description></item>
        /// </list></remarks>
        string LatitudeRef { get; }

        /// <summary>
        /// Indicates the longitude degrees.
        /// </summary>
        /// <value>
        /// This is the value at index 0 from an array of three values.
        /// </value>
        /// <remarks>
        /// Indicates whether latitude is north or south.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Longitude Degrees</description></item>
        /// <item><term>Format ID</term><description>{C4C4DBB2-B593-466B-BBDA-D03D27D5E43A} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longitudedegrees">[Reference Link]</a></description></item>
        /// </list></remarks>
        double? LongitudeDegrees { get; }

        /// <summary>
        /// Indicates the longitude minutes.
        /// </summary>
        /// <value>
        /// This is the value at index 1 from an array of three values.
        /// </value>
        /// <remarks>
        /// Indicates whether latitude is north or south.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Longitude Minutes</description></item>
        /// <item><term>Format ID</term><description>{C4C4DBB2-B593-466B-BBDA-D03D27D5E43A} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longitudeminutes">[Reference Link]</a></description></item>
        /// </list></remarks>
        double? LongitudeMinutes { get; }

        /// <summary>
        /// Indicates the longitude seconds.
        /// </summary>
        /// <value>
        /// This is the value at index 2 from an array of three values.
        /// </value>
        /// <remarks>
        /// Indicates whether latitude is north or south.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Longitude Seconds</description></item>
        /// <item><term>Format ID</term><description>{C4C4DBB2-B593-466B-BBDA-D03D27D5E43A} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longitudeseconds">[Reference Link]</a></description></item>
        /// </list></remarks>
        double? LongitudeSeconds { get; }

        /// <summary>
        /// Indicates whether longitude is east or west longitude
        /// </summary>
        /// <value>
        /// Indicates whether longitude is east or west.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Longitude Ref</description></item>
        /// <item><term>Format ID</term><description>{33DCF22B-28D5-464C-8035-1EE9EFD25278} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longituderef">[Reference Link]</a></description></item>
        /// </list></remarks>
        string LongitudeRef { get; }

        /// <summary>
        /// Indicates the GPS measurement mode.
        /// </summary>
        /// <value>
        /// eg: 2-dimensional, 3-dimensional Indicates the GPS measurement mode (for example, two-dimensional, three-dimensional).
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Measure Mode</description></item>
        /// <item><term>Format ID</term><description>{A015ED5D-AAEA-4D58-8A86-3C586920EA0B} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-measuremode">[Reference Link]</a></description></item>
        /// </list></remarks>
        string MeasureMode { get; }

        /// <summary>
        /// Indicates the name of the method used for location finding
        /// </summary>
        /// <value>
        /// Indicates the name of the method used for finding locations.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Processing Method</description></item>
        /// <item><term>Format ID</term><description>{59D49E61-840F-4AA9-A939-E2099B7F6399} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-processingmethod">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ProcessingMethod { get; }

        /// <summary>
        /// Indicates the version of the GPS information
        /// </summary>
        /// <value>
        /// Indicates the version of the GPS information.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Version ID</description></item>
        /// <item><term>Format ID</term><description>{22704DA4-C6B2-4A99-8E56-F16DF8C92599} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-versionid">[Reference Link]</a></description></item>
        /// </list></remarks>
        ByteValues VersionID { get; }
    }
}
