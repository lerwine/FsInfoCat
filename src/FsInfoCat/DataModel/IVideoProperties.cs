using FsInfoCat.Collections;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Represents extended file properties for video files.
    /// </summary>
    [EntityInterface]
    public interface IVideoProperties : IEquatable<IVideoProperties>
    {
        /// <summary>
        /// Indicates the level of compression for the video stream.
        /// </summary>
        /// <value>Specifies the video compression format.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Compression</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>10</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-compression">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Compression), ResourceType = typeof(Properties.Resources))]
        string Compression { get; }

        /// <summary>
        /// Gets the Director.
        /// </summary>
        /// <value>Indicates the person who directed the video.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Director</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>20</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-director">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Director), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Director { get; }

        /// <summary>
        /// Indicates the data rate in "bits per second" for the video stream.
        /// </summary>
        /// <value>Indicates the data rate in "bits per second" for the video stream.</value>
        /// <remarks>
        /// "DataRate".
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Encoding Data Rate</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>8</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-encodingbitrate">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_EncodingBitrate), ResourceType = typeof(Properties.Resources))]
        uint? EncodingBitrate { get; }

        /// <summary>
        /// Indicates the frame height for the video stream.
        /// </summary>
        /// <value>Indicates the frame height for the video stream.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Frame Height</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>4</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-frameheight">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_FrameHeight), ResourceType = typeof(Properties.Resources))]
        uint? FrameHeight { get; }

        /// <summary>
        /// Indicates the frame rate in "frames per millisecond" for the video stream.
        /// </summary>
        /// <value>Indicates the frame rate for the video stream, in frames per 1000 seconds.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Frame Rate</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>6</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-framerate">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_FrameRate), ResourceType = typeof(Properties.Resources))]
        uint? FrameRate { get; }

        /// <summary>
        /// Indicates the frame width for the video stream.
        /// </summary>
        /// <value>Indicates the frame width for the video stream.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Frame Width</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>3</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-framewidth">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_FrameWidth), ResourceType = typeof(Properties.Resources))]
        uint? FrameWidth { get; }

        /// <summary>
        /// Indicates the horizontal portion of the aspect ratio.
        /// </summary>
        /// <value>The X portion of XX:YY, like 16:9.</value>
        /// <remarks>
        /// Indicates the horizontal portion of the pixel aspect ratio. The X portion of XX:YY. For example, 10 is the X portion of 10:11.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Horizontal Aspect Ratio</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>42</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-horizontalaspectratio">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_HorizontalAspectRatio), ResourceType = typeof(Properties.Resources))]
        uint? HorizontalAspectRatio { get; }

        /// <summary>
        /// Gets the Stream Number.
        /// </summary>
        /// <value>Indicates the ordinal number of the stream being played.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Stream Number</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>11</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-streamnumber">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_StreamNumber), ResourceType = typeof(Properties.Resources))]
        ushort? StreamNumber { get; }

        /// <summary>
        /// Gets the name for the video stream..
        /// </summary>
        /// <value>The name for the video stream.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Stream Name</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-streamname">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_StreamName), ResourceType = typeof(Properties.Resources))]
        string StreamName { get; }

        /// <summary>
        /// Indicates the vertical portion of the aspect ratio.
        /// </summary>
        /// <value>The Y portion of XX:YY, like 16:9.</value>
        /// <remarks>
        /// Indicates the horizontal portion of the pixel aspect ratio. The Y portion of XX:YY. For example, 11 is the Y portion of 10:11 .
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Vertical Aspect Ratio</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>45</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-verticalaspectratio">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_VerticalAspectRatio), ResourceType = typeof(Properties.Resources))]
        uint? VerticalAspectRatio { get; }
    }
}
