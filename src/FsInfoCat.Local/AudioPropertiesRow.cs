using System;
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

        [NotNull]
        public string Compression { get => _compression; set => _compression = value.AsWsNormalizedOrEmpty(); }

        public uint? EncodingBitrate { get; set; }

        [NotNull]
        public string Format { get => _format; set => _format = value.AsWsNormalizedOrEmpty(); }

        public bool? IsVariableBitrate { get; set; }

        public uint? SampleRate { get; set; }

        public uint? SampleSize { get; set; }

        [NotNull]
        public string StreamName { get => _streamName; set => _streamName = value.AsWsNormalizedOrEmpty(); }

        public ushort? StreamNumber { get; set; }

        #endregion

        protected bool ArePropertiesEqual([DisallowNull] IAudioProperties other) => _compression == other.Compression &&
            _format == other.Format &&
            _streamName == other.StreamName &&
            EncodingBitrate == other.EncodingBitrate &&
            IsVariableBitrate == other.IsVariableBitrate &&
            SampleRate == other.SampleRate &&
            SampleSize == other.SampleSize &&
            StreamNumber == other.StreamNumber;

        public abstract bool Equals(IAudioPropertiesRow other);

        public abstract bool Equals(IAudioProperties other);

        public override int GetHashCode()
        {
            Guid id = Id;
            if (id.Equals(Guid.Empty))
            {
                HashCode hash = new HashCode();
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
            return id.GetHashCode();
        }
    }
}
