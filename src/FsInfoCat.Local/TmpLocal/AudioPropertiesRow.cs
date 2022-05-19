using M = FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    // TODO: Document AudioPropertiesRow class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class AudioPropertiesRow : PropertiesRow, ILocalAudioPropertiesRow
    {
        #region Fields

        private string _compression = string.Empty;
        private string _format = string.Empty;
        private string _streamName = string.Empty;

        #endregion

        #region Properties

        [NotNull]
        [BackingField(nameof(_compression))]
        public string Compression { get => _compression; set => _compression = value.AsWsNormalizedOrEmpty(); }

        public uint? EncodingBitrate { get; set; }

        [NotNull]
        [BackingField(nameof(_format))]
        public string Format { get => _format; set => _format = value.AsWsNormalizedOrEmpty(); }

        public bool? IsVariableBitrate { get; set; }

        public uint? SampleRate { get; set; }

        public uint? SampleSize { get; set; }

        [NotNull]
        [BackingField(nameof(_streamName))]
        public string StreamName { get => _streamName; set => _streamName = value.AsWsNormalizedOrEmpty(); }

        public ushort? StreamNumber { get; set; }

        #endregion

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalAudioPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalAudioPropertiesRow other) => ArePropertiesEqual((M.IAudioPropertiesRow)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="M.IAudioPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] M.IAudioPropertiesRow other) => ArePropertiesEqual((M.IAudioProperties)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="M.IAudioProperties" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] M.IAudioProperties other) => _compression == other.Compression &&
            _format == other.Format &&
            _streamName == other.StreamName &&
            EncodingBitrate == other.EncodingBitrate &&
            IsVariableBitrate == other.IsVariableBitrate &&
            SampleRate == other.SampleRate &&
            SampleSize == other.SampleSize &&
            StreamNumber == other.StreamNumber;

        public abstract bool Equals(M.IAudioPropertiesRow other);

        public abstract bool Equals(M.IAudioProperties other);

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
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
