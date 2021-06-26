namespace FsInfoCat
{
    public interface IVideoProperties
    {
        /// <summary>
        /// Indicates the level of compression for the video stream.
        /// </summary>
        /// <value>
        /// Specifies the video compression format.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Compression</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>10</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-compression">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Compression { get; }

        /// <summary>
        /// Gets the Director
        /// </summary>
        /// <value>
        /// Indicates the person who directed the video.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Director</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>20</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-director">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue Director { get; }

        /// <summary>
        /// Indicates the data rate in "bits per second" for the video stream.
        /// </summary>
        /// <value>
        /// Indicates the data rate in "bits per second" for the video stream.
        /// </value>
        /// <remarks>
        /// "DataRate".
        /// <list type="bullet">
        /// <item><term>Name</term><description>Encoding Data Rate</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>8</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-encodingbitrate">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? EncodingBitrate { get; }

        /// <summary>
        /// Indicates the frame height for the video stream.
        /// </summary>
        /// <value>
        /// Indicates the frame height for the video stream.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Frame Height</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>4</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-frameheight">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? FrameHeight { get; }

        /// <summary>
        /// Indicates the frame rate in "frames per millisecond" for the video stream.
        /// </summary>
        /// <value>
        /// Indicates the frame rate for the video stream, in frames per 1000 seconds.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Frame Rate</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>6</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-framerate">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? FrameRate { get; }

        /// <summary>
        /// Indicates the frame width for the video stream.
        /// </summary>
        /// <value>
        /// Indicates the frame width for the video stream.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Frame Width</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>3</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-framewidth">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? FrameWidth { get; }

        /// <summary>
        /// Indicates the horizontal portion of the aspect ratio.
        /// </summary>
        /// <value>
        /// The X portion of XX:YY, like 16:9.
        /// </value>
        /// <remarks>
        /// Indicates the horizontal portion of the pixel aspect ratio. The X portion of XX:YY. For example, 10 is the X portion of 10:11.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Horizontal Aspect Ratio</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>42</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-horizontalaspectratio">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? HorizontalAspectRatio { get; }

        /// <summary>
        /// Indicates the name for the video stream.
        /// </summary>
        /// <value>
        /// Indicates the name for the video stream.
        /// </value>
        /// <remarks>
        /// "StreamName".
        /// <list type="bullet">
        /// <item><term>Name</term><description>Stream Name</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>2</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-streamname">[Reference Link]</a></description></item>
        /// </list></remarks>
        string StreamName { get; }

        /// <summary>
        /// Gets the Stream Number
        /// </summary>
        /// <value>
        /// Indicates the ordinal number of the stream being played.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Stream Number</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>11</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-streamnumber">[Reference Link]</a></description></item>
        /// </list></remarks>
        ushort? StreamNumber { get; }
        
        /// <summary>
        /// Indicates the vertical portion of the aspect ratio
        /// </summary>
        /// <value>
        /// The Y portion of XX:YY, like 16:9.
        /// </value>
        /// <remarks>
        /// Indicates the horizontal portion of the pixel aspect ratio. The Y portion of XX:YY. For example, 11 is the Y portion of 10:11 .
        /// <list type="bullet">
        /// <item><term>Name</term><description>Vertical Aspect Ratio</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>45</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-verticalaspectratio">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? VerticalAspectRatio { get; }
    }
}
