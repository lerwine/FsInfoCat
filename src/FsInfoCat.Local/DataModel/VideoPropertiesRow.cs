using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    // TODO: Document VideoPropertiesRow class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class VideoPropertiesRow : PropertiesRow, ILocalVideoPropertiesRow
    {
        #region Fields

        private string _compression = string.Empty;
        private string _streamName = string.Empty;

        #endregion
        #region Properties

        [NotNull]
        [BackingField(nameof(_compression))]
        public string Compression { get => _compression; set => _compression = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Director { get; set; }

        public uint? EncodingBitrate { get; set; }

        public uint? FrameHeight { get; set; }

        public uint? FrameRate { get; set; }

        public uint? FrameWidth { get; set; }

        public uint? HorizontalAspectRatio { get; set; }

        [NotNull]
        [BackingField(nameof(_streamName))]
        public string StreamName { get => _streamName; set => _streamName = value.AsWsNormalizedOrEmpty(); }

        public ushort? StreamNumber { get; set; }

        public uint? VerticalAspectRatio { get; set; }

        #endregion

        protected bool ArePropertiesEqual([DisallowNull] ILocalVideoPropertiesRow other) => ArePropertiesEqual((IVideoPropertiesRow)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        protected bool ArePropertiesEqual([DisallowNull] IVideoPropertiesRow other) => ArePropertiesEqual((IVideoProperties)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        protected bool ArePropertiesEqual([DisallowNull] IVideoProperties other) => _compression == other.Compression &&
            _streamName == other.StreamName &&
            EqualityComparer<MultiStringValue>.Default.Equals(Director, other.Director) &&
            EncodingBitrate == other.EncodingBitrate &&
            FrameHeight == other.FrameHeight &&
            FrameRate == other.FrameRate &&
            FrameWidth == other.FrameWidth &&
            HorizontalAspectRatio == other.HorizontalAspectRatio &&
            StreamNumber == other.StreamNumber &&
            VerticalAspectRatio == other.VerticalAspectRatio;

        public abstract bool Equals(IVideoPropertiesRow other);

        public abstract bool Equals(IVideoProperties other);

        public override int GetHashCode()
        {
            if (TryGetId(out Guid id)) return id.GetHashCode();
            HashCode hash = new();
            hash.Add(_compression);
            hash.Add(_streamName);
            hash.Add(Director);
            hash.Add(EncodingBitrate);
            hash.Add(FrameHeight);
            hash.Add(FrameRate);
            hash.Add(FrameWidth);
            hash.Add(HorizontalAspectRatio);
            hash.Add(StreamNumber);
            hash.Add(VerticalAspectRatio);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
