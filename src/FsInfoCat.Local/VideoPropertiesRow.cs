using FsInfoCat.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public abstract class VideoPropertiesRow : PropertiesRow, ILocalVideoPropertiesRow
    {
        #region Fields

        private string _compression = string.Empty;
        private string _streamName = string.Empty;

        #endregion
        #region Properties

        [NotNull]
        public string Compression { get => _compression; set => _compression = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Director { get; set; }

        public uint? EncodingBitrate { get; set; }

        public uint? FrameHeight { get; set; }

        public uint? FrameRate { get; set; }

        public uint? FrameWidth { get; set; }

        public uint? HorizontalAspectRatio { get; set; }

        [NotNull]
        public string StreamName { get => _streamName; set => _streamName = value.AsWsNormalizedOrEmpty(); }

        public ushort? StreamNumber { get; set; }

        public uint? VerticalAspectRatio { get; set; }

        #endregion

        protected bool ArePropertiesEqual([DisallowNull] IVideoProperties other) => Compression == other.Compression &&
            EqualityComparer<MultiStringValue>.Default.Equals(Director, other.Director) &&
            EncodingBitrate == other.EncodingBitrate &&
            FrameHeight == other.FrameHeight &&
            FrameRate == other.FrameRate &&
            FrameWidth == other.FrameWidth &&
            HorizontalAspectRatio == other.HorizontalAspectRatio &&
            StreamName == other.StreamName &&
            StreamNumber == other.StreamNumber &&
            VerticalAspectRatio == other.VerticalAspectRatio;

        public abstract bool Equals(IVideoPropertiesRow other);

        public abstract bool Equals(IVideoProperties other);

        public override int GetHashCode()
        {
            Guid id = Id;
            if (id.Equals(Guid.Empty))
            {
                HashCode hash = new();
                hash.Add(CreatedOn);
                hash.Add(ModifiedOn);
                hash.Add(UpstreamId);
                hash.Add(LastSynchronizedOn);
                hash.Add(_compression);
                hash.Add(Director);
                hash.Add(EncodingBitrate);
                hash.Add(FrameHeight);
                hash.Add(FrameRate);
                hash.Add(FrameWidth);
                hash.Add(HorizontalAspectRatio);
                hash.Add(_streamName);
                hash.Add(StreamNumber);
                hash.Add(VerticalAspectRatio);
                return hash.ToHashCode();
            }
            return id.GetHashCode();
        }
    }
}
