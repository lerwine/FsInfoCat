using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Represents extended file properties for audio files.
    /// </summary>
    [EntityInterface]
    public interface IAudioProperties : IEquatable<IAudioProperties>
    {
        /// <summary>
        /// Gets the Compression Method.
        /// </summary>
        /// <value>Indicates the audio compression used on the audio file.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Compression Method</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>10</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-compression">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Compression), ResourceType = typeof(Properties.Resources))]
        string Compression { get; }

        /// <summary>
        /// Indicates the average data rate in Hz for the audio file in "bits per second".
        /// </summary>
        /// <value>Indicates the average data rate in Hertz (Hz) for the audio file in bits per second.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Encoding Bitrate</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>4</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-encodingbitrate">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_EncodingBitrate), ResourceType = typeof(Properties.Resources))]
        uint? EncodingBitrate { get; }

        /// <summary>
        /// Indicates the format of the audio file.
        /// </summary>
        /// <value>Indicates the format of the audio file.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Format</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-format">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Format), ResourceType = typeof(Properties.Resources))]
        string Format { get; }

        /// <summary>
        /// Indicates whether Bit Rate of the audio is variable.
        /// </summary>
        /// <value>Indicates whether the audio file had a variable or constant bit rate.</value>
        /// <remarks>
        /// <see langword="true" /> if the bit rate of the audio is variable; <see langword="false" /> if the bit rate is constant; otherwise, <see langword="null" /> if this value is not specified.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Is Variable Bitrate</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{E6822FEE-8C17-4D62-823C-8E9CFCBD1D5C} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-isvariablebitrate">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsVariableBitrate), ResourceType = typeof(Properties.Resources))]
        bool? IsVariableBitrate { get; }

        /// <summary>
        /// Indicates the audio sample rate for the audio file in "samples per second".
        /// </summary>
        /// <value>Indicates the sample rate for the audio file in samples per second.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Sample Rate</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>5</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-samplerate">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_SampleRate), ResourceType = typeof(Properties.Resources))]
        uint? SampleRate { get; }

        /// <summary>
        /// Indicates the audio sample size for the audio file in "bits per sample".
        /// </summary>
        /// <value>Indicates the sample size for the audio file in bits per sample.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Sample Size</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>6</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-samplesize">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_SampleSize), ResourceType = typeof(Properties.Resources))]
        uint? SampleSize { get; }

        /// <summary>
        /// Gets the Stream Name.
        /// </summary>
        /// <value>Identifies the name of the stream for the audio file.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Stream Name</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>9</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-streamname">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_StreamName), ResourceType = typeof(Properties.Resources))]
        string StreamName { get; }

        /// <summary>
        /// Gets the Stream Number.
        /// </summary>
        /// <value>Identifies the stream number of the audio file.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Stream Number</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>8</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-streamnumber">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_StreamNumber), ResourceType = typeof(Properties.Resources))]
        ushort? StreamNumber { get; }
    }
}
