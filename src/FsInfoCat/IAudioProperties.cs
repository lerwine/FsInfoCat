namespace FsInfoCat
{
    public interface IAudioProperties
    {
        /// <summary>
        /// Gets the Compression Method.
        /// </summary>
        /// <remarks>ID: {64440490-4C8B-11D1-8B70-080036B11A03}, 10 (AudioSummaryInformation)</remarks>
        string Compression { get; }

        /// <summary>
        /// Indicates the average data rate in Hz for the audio file in &quot;bits per second&quot;.
        /// </summary>
        /// <remarks>ID: {64440490-4C8B-11D1-8B70-080036B11A03}, 4 (AudioSummaryInformation)</remarks>
        uint? EncodingBitrate { get; }

        /// <summary>
        /// Indicates the format of the audio file.
        /// </summary>
        /// <remarks>ID: {64440490-4C8B-11D1-8B70-080036B11A03}, 2 (AudioSummaryInformation)</remarks>
        string Format { get; }

        /// <summary>
        /// Indicates whether Bit Rate of the audio is variable
        /// </summary>
        /// <value><see langword="true"/> if the bit rate of the audio is variable; <see langword="false"/> if the bit rate is constant; otherwise, <see langword="null"/> if this value is not specified.</value>
        /// <remarks>ID: {E6822FEE-8C17-4D62-823C-8E9CFCBD1D5C}, 100</remarks>
        bool? IsVariableBitrate { get; }

        /// <summary>
        /// Indicates the audio sample rate for the audio file in &quot;samples per second&quot;.
        /// </summary>
        /// <remarks>ID: {64440490-4C8B-11D1-8B70-080036B11A03}, 5 (AudioSummaryInformation)</remarks>
        uint? SampleRate { get; }

        /// <summary>
        /// Indicates the audio sample size for the audio file in &quot;bits per sample&quot;.
        /// </summary>
        /// <remarks>ID: {64440490-4C8B-11D1-8B70-080036B11A03}, 6 (AudioSummaryInformation)</remarks>
        uint? SampleSize { get; }

        /// <summary>
        /// Gets the Stream Name
        /// </summary>
        /// <remarks>ID: {64440490-4C8B-11D1-8B70-080036B11A03}, 9 (AudioSummaryInformation)</remarks>
        string StreamName { get; }

        /// <summary>
        /// Gets the Stream Number
        /// </summary>
        /// <remarks>ID: {64440490-4C8B-11D1-8B70-080036B11A03}, 8 (AudioSummaryInformation)</remarks>
        ushort? StreamNumber { get; }
    }
}
