using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public abstract class AudioPropertiesRow : PropertiesRow, ILocalAudioPropertiesRow
    {
        #region Fields

        private string _compression = string.Empty;
        private string _format = string.Empty;
        private string _streamName = string.Empty;

        #endregion

        #region Properties

        public string Compression { get => _compression; set => _compression = value.AsWsNormalizedOrEmpty(); }

        public uint? EncodingBitrate { get; set; }

        public string Format { get => _format; set => _format = value.AsWsNormalizedOrEmpty(); }

        public bool? IsVariableBitrate { get; set; }

        public uint? SampleRate { get; set; }

        public uint? SampleSize { get; set; }

        public string StreamName { get => _streamName; set => _streamName = value.AsWsNormalizedOrEmpty(); }

        public ushort? StreamNumber { get; set; }

        #endregion

        protected bool ArePropertiesEqual([DisallowNull] IAudioProperties other) => EntityExtensions.NullablesEqual(EncodingBitrate, other.EncodingBitrate) && EntityExtensions.NullablesEqual(IsVariableBitrate, other.IsVariableBitrate) &&
            EntityExtensions.NullablesEqual(SampleRate, other.SampleRate) && EntityExtensions.NullablesEqual(SampleSize, other.SampleSize) && EntityExtensions.NullablesEqual(StreamNumber, other.StreamNumber) &&
            _compression.Equals(other.Compression) && _format.Equals(other.Format) && _streamName.Equals(other.StreamName);

        public abstract bool Equals(IAudioPropertiesRow other);

        public abstract bool Equals(IAudioProperties other);
    }
}
