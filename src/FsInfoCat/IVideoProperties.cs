namespace FsInfoCat
{
    public interface IVideoProperties
    {
        /// <summary>
        /// Indicates the level of compression for the video stream.
        /// </summary>
        /// <remarks>ID: {64440491-4C8B-11D1-8B70-080036B11A03}, 10 (VideoSummaryInformation)</remarks>
        string Compression { get; }

        /// <summary>
        /// Gets the Director
        /// </summary>
        /// <remarks>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 20 (MEDIAFILESUMMARYINFORMATION)</remarks>
        MultiStringValue Director { get; }

        /// <summary>
        /// Indicates the data rate in &quot;bits per second&quot; for the video stream.
        /// </summary>
        /// <remarks>ID: {64440491-4C8B-11D1-8B70-080036B11A03}, 8 (VideoSummaryInformation)</remarks>
        uint? EncodingBitrate { get; }

        /// <summary>
        /// Indicates the frame height for the video stream.
        /// </summary>
        /// <remarks>ID: {64440491-4C8B-11D1-8B70-080036B11A03}, 4 (VideoSummaryInformation)</remarks>
        uint? FrameHeight { get; }

        /// <summary>
        /// Indicates the frame rate in &quot;frames per millisecond&quot; for the video stream.
        /// </summary>
        /// <remarks>ID: {64440491-4C8B-11D1-8B70-080036B11A03}, 6 (VideoSummaryInformation)</remarks>
        uint? FrameRate { get; }

        /// <summary>
        /// Indicates the frame width for the video stream.
        /// </summary>
        /// <remarks>ID: {64440491-4C8B-11D1-8B70-080036B11A03}, 3 (VideoSummaryInformation)</remarks>
        uint? FrameWidth { get; }

        /// <summary>
        /// Indicates the horizontal portion of the aspect ratio.
        /// </summary>
        /// <remarks>The X portion of XX:YY, like 16:9.
        /// <para>ID: {64440491-4C8B-11D1-8B70-080036B11A03}, 42 (VideoSummaryInformation)</para></remarks>
        uint? HorizontalAspectRatio { get; }

        /// <summary>
        /// Indicates the name for the video stream.
        /// </summary>
        /// <remarks>ID: {64440491-4C8B-11D1-8B70-080036B11A03}, 2 (VideoSummaryInformation)</remarks>
        string StreamName { get; }

        /// <summary>
        /// Gets the Stream Number
        /// </summary>
        /// <remarks>ID: {64440491-4C8B-11D1-8B70-080036B11A03}, 11 (VideoSummaryInformation)</remarks>
        ushort? StreamNumber { get; }

        /// <summary>
        /// Indicates the vertical portion of the aspect ratio
        /// </summary>
        /// <remarks>The Y portion of XX:YY, like 16:9.
        /// <para>ID: {64440491-4C8B-11D1-8B70-080036B11A03}, 45 (VideoSummaryInformation)</para></remarks>
        uint? VerticalAspectRatio { get; }
    }
}
