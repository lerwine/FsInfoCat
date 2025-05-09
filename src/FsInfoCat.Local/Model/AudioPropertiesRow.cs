using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for entities containing extended file properties for audio files.
    /// <seealso cref="AudioPropertySet" />
    /// <seealso cref="AudioPropertiesListItem" />
    /// <seealso cref="LocalDbContext.AudioPropertySets" />
    /// <seealso cref="LocalDbContext.AudioPropertiesListing" />
    /// </summary>
    public abstract class AudioPropertiesRow : PropertiesRow, ILocalAudioPropertiesRow
    {
        #region Fields

        private string _compression = string.Empty;
        private string _format = string.Empty;
        private string _streamName = string.Empty;

        #endregion

        #region Properties

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
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-compression">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.Compression), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_compression))]
        public string Compression { get => _compression; set => _compression = value.AsWsNormalizedOrEmpty(); }

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
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-encodingbitrate">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.EncodingBitrate), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public uint? EncodingBitrate { get; set; }

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
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-format">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.Format), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_format))]
        public string Format { get => _format; set => _format = value.AsWsNormalizedOrEmpty(); }

        /// <summary>
        /// Indicates whether Bit Rate of the audio is variable.
        /// </summary>
        /// <value>Indicates whether the audio file had a variable or constant bit rate.</value>
        /// <remarks>
        /// <see langword="true" /> if the bit rate of the audio is variable; <see langword="false" /> if the bit rate is constant; otherwise, <see langword="null" />
        /// if this value is not specified.
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
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-isvariablebitrate">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.IsVariableBitrate), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public bool? IsVariableBitrate { get; set; }

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
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-samplerate">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        public uint? SampleRate { get; set; }

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
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-samplesize">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.SampleSize), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public uint? SampleSize { get; set; }

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
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-streamname">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.StreamName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_streamName))]
        public string StreamName { get => _streamName; set => _streamName = value.AsWsNormalizedOrEmpty(); }

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
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-streamnumber">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.StreamNumber), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public ushort? StreamNumber { get; set; }

        #endregion

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalAudioPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalAudioPropertiesRow other) => ArePropertiesEqual((IAudioPropertiesRow)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IAudioPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IAudioPropertiesRow other) => ArePropertiesEqual((IAudioProperties)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IAudioProperties" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IAudioProperties other) => _compression == other.Compression &&
            _format == other.Format &&
            _streamName == other.StreamName &&
            EncodingBitrate == other.EncodingBitrate &&
            IsVariableBitrate == other.IsVariableBitrate &&
            SampleRate == other.SampleRate &&
            SampleSize == other.SampleSize &&
            StreamNumber == other.StreamNumber;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public abstract bool Equals(IAudioPropertiesRow other);

        public abstract bool Equals(IAudioProperties other);

        public override int GetHashCode()
        {
            if (TryGetId(out Guid id)) return id.GetHashCode();
            HashCode hash = new();
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(_compression);
            hash.Add(_format);
            hash.Add(_streamName);
            hash.Add(EncodingBitrate);
            hash.Add(IsVariableBitrate);
            hash.Add(SampleRate);
            hash.Add(SampleSize);
            hash.Add(StreamNumber);
            return hash.ToHashCode();
        }

        protected virtual string PropertiesToString() => $@"EncodingBitrate={EncodingBitrate}, IsVariableBitrate={IsVariableBitrate}, SampleRate={SampleRate}, SampleSize={SampleSize},
    StreamNumber={StreamNumber}, StreamName=""{ExtensionMethods.EscapeCsString(_streamName)}"", Compression=""{ExtensionMethods.EscapeCsString(_compression)}"", Format=""{ExtensionMethods.EscapeCsString(_format)}""";

        public override string ToString() => $@"{{ Id={(TryGetId(out Guid id) ? id : null)}, {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
