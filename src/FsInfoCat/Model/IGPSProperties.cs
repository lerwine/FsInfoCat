using FsInfoCat.Collections;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Represents extended file properties for GPS information.
    /// </summary>
    [EntityInterface]
    public interface IGPSProperties : IEquatable<IGPSProperties>
    {
        /// <summary>
        /// Gets the name of the GPS area.
        /// </summary>
        /// <value>The name of the GPS area.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Area Information</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{972E333E-AC7E-49F1-8ADF-A70D07A9BCAB} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-areainformation">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.AreaInformation), ResourceType = typeof(Properties.Resources))]
        string AreaInformation { get; }

        /// <summary>
        /// Indicates the latitude degrees.
        /// </summary>
        /// <value>This is the value at index 0 from an array of three values.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Latitude Degrees</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{8727CFFF-4868-4EC6-AD5B-81B98521D1AB} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latitudedegrees">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.LatitudeDegrees), ResourceType = typeof(Properties.Resources))]
        double? LatitudeDegrees { get; }

        /// <summary>
        /// Indicates the latitude minutes.
        /// </summary>
        /// <value>This is the value at index 1 from an array of three values.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Latitude Minutes</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{8727CFFF-4868-4EC6-AD5B-81B98521D1AB} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latitudeminutes">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.LatitudeMinutes), ResourceType = typeof(Properties.Resources))]
        double? LatitudeMinutes { get; }

        /// <summary>
        /// Indicates the latitude seconds.
        /// </summary>
        /// <value>This is the value at index 2 from an array of three values.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Latitude Seconds</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{8727CFFF-4868-4EC6-AD5B-81B98521D1AB} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latitude">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.LatitudeSeconds), ResourceType = typeof(Properties.Resources))]
        double? LatitudeSeconds { get; }

        /// <summary>
        /// Indicates whether latitude is north or south latitude.
        /// </summary>
        /// <value>Indicates whether latitude is north or south.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Latitude Reference</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{029C0252-5B86-46C7-ACA0-2769FFC8E3D4} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latituderef">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.LatitudeRef), ResourceType = typeof(Properties.Resources))]
        string LatitudeRef { get; }

        /// <summary>
        /// Indicates the longitude degrees.
        /// </summary>
        /// <value>This is the value at index 0 from an array of three values.</value>
        /// <remarks>
        /// Indicates whether latitude is north or south.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Longitude Degrees</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{C4C4DBB2-B593-466B-BBDA-D03D27D5E43A} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longitudedegrees">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.LongitudeDegrees), ResourceType = typeof(Properties.Resources))]
        double? LongitudeDegrees { get; }

        /// <summary>
        /// Indicates the longitude minutes.
        /// </summary>
        /// <value>This is the value at index 1 from an array of three values.</value>
        /// <remarks>
        /// Indicates whether latitude is north or south.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Longitude Minutes</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{C4C4DBB2-B593-466B-BBDA-D03D27D5E43A} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longitudeminutes">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.LongitudeMinutes), ResourceType = typeof(Properties.Resources))]
        double? LongitudeMinutes { get; }

        /// <summary>
        /// Indicates the longitude seconds.
        /// </summary>
        /// <value>This is the value at index 2 from an array of three values.</value>
        /// <remarks>
        /// Indicates whether latitude is north or south.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Longitude Seconds</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{C4C4DBB2-B593-466B-BBDA-D03D27D5E43A} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longitudeseconds">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.LongitudeSeconds), ResourceType = typeof(Properties.Resources))]
        double? LongitudeSeconds { get; }

        /// <summary>
        /// Indicates whether longitude is east or west longitude.
        /// </summary>
        /// <value>Indicates whether longitude is east or west.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Longitude Reference</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{33DCF22B-28D5-464C-8035-1EE9EFD25278} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longituderef">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.LongitudeRef), ResourceType = typeof(Properties.Resources))]
        string LongitudeRef { get; }

        /// <summary>
        /// Indicates the GPS measurement mode.
        /// </summary>
        /// <value>eg: 2-dimensional, 3-dimensional Indicates the GPS measurement mode (for example, two-dimensional, three-dimensional).</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Measure Mode</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{A015ED5D-AAEA-4D58-8A86-3C586920EA0B} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-measuremode">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.MeasureMode), ResourceType = typeof(Properties.Resources))]
        string MeasureMode { get; }

        /// <summary>
        /// Indicates the name of the method used for location finding.
        /// </summary>
        /// <value>Indicates the name of the method used for finding locations.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Processing Method</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{59D49E61-840F-4AA9-A939-E2099B7F6399} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-processingmethod">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.ProcessingMethod), ResourceType = typeof(Properties.Resources))]
        string ProcessingMethod { get; }

        /// <summary>
        /// Indicates the version of the GPS information.
        /// </summary>
        /// <value>Indicates the version of the GPS information.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Version ID</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{22704DA4-C6B2-4A99-8E56-F16DF8C92599} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-versionid">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.VersionID), ResourceType = typeof(Properties.Resources))]
        ByteValues VersionID { get; }
    }
}
