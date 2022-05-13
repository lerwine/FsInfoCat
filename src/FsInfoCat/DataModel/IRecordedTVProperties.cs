using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Represents extended file properties for recorded TV files.
    /// </summary>
    [EntityInterface]
    public interface IRecordedTVProperties : IEquatable<IRecordedTVProperties>
    {
        /// <summary>
        /// Gets the Channel Number.
        /// </summary>
        /// <value>Example: 42 The recorded TV channels.</value>
        /// <remarks>
        /// For example, 42, 5, 53.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Channel Number</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>7</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-channelnumber">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ChannelNumber), ResourceType = typeof(Properties.Resources))]
        uint? ChannelNumber { get; }

        /// <summary>
        /// Gets the Episode Name.
        /// </summary>
        /// <value>Example: "Nowhere to Hyde" The names of recorded TV episodes.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>For example, "Nowhere to Hyde".</para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Episode Name</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-episodename">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_EpisodeName), ResourceType = typeof(Properties.Resources))]
        string EpisodeName { get; }

        /// <summary>
        /// Indicates whether the video is DTV.
        /// </summary>
        /// <value><see langword="true" /> if the video is DTV; <see langword="false" /> if not DTV; otherwise, <see langword="null" /> if not specified.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Is DTV Content</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>17</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-isdtvcontent">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsDTVContent), ResourceType = typeof(Properties.Resources))]
        bool? IsDTVContent { get; }

        /// <summary>
        /// Indicates whether the video is HDTV.
        /// </summary>
        /// <value><see langword="true" /> if the video is HDTV; <see langword="false" /> if not HDTV; otherwise, <see langword="null" /> if not specified.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Is HDTV Content</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>18</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-ishdcontent">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsHDContent), ResourceType = typeof(Properties.Resources))]
        bool? IsHDContent { get; }

        /// <summary>
        /// Gets the Network Affiliation.
        /// </summary>
        /// <value>The Network Affiliation</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>TV Network Affiliation</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{2C53C813-FB63-4E22-A1AB-0B331CA1E273} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-networkaffiliation">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_NetworkAffiliation), ResourceType = typeof(Properties.Resources))]
        string NetworkAffiliation { get; }

        /// <summary>
        /// Gets the Original Broadcast Date.
        /// </summary>
        /// <value>The Original Broadcast Date</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Original Broadcast Date</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{4684FE97-8765-4842-9C13-F006447B178C} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-originalbroadcastdate">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_OriginalBroadcastDate), ResourceType = typeof(Properties.Resources))]
        DateTime? OriginalBroadcastDate { get; }

        /// <summary>
        /// Gets the Program Description.
        /// </summary>
        /// <value>The Program Description</value>
        /// <remarks>
        /// This value should be trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Program Description</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>3</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-programdescription">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ProgramDescription), ResourceType = typeof(Properties.Resources))]
        string ProgramDescription { get; }

        /// <summary>
        /// Gets the Station Call Sign.
        /// </summary>
        /// <value>Example: "TOONP" Any recorded station call signs.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>For example, "TOONP".</para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Station Call Sign</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>5</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-stationcallsign">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_StationCallSign), ResourceType = typeof(Properties.Resources))]
        string StationCallSign { get; }

        /// <summary>
        /// Gets the Station Name.
        /// </summary>
        /// <value>The  name of the broadcast station or <see langword="null" /> if this value is not specified.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Station Name</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{1B5439E7-EBA1-4AF8-BDD7-7AF1D4549493} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-stationname">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_StationName), ResourceType = typeof(Properties.Resources))]
        string StationName { get; }
    }
}
