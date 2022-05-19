using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    // TODO: Document ImagePropertiesRow class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Obsolete("Use FsInfoCat.Local.Model.ImagePropertiesRow")]
    public abstract class ImagePropertiesRow : PropertiesRow, ILocalImagePropertiesRow
    {
        #region Fields

        private string _compressionText = string.Empty;
        private string _imageID = string.Empty;

        #endregion

        #region Properties

        public uint? BitDepth { get; set; }

        public ushort? ColorSpace { get; set; }

        public double? CompressedBitsPerPixel { get; set; }

        public ushort? Compression { get; set; }

        [NotNull]
        [BackingField(nameof(_compressionText))]
        public string CompressionText { get => _compressionText; set => _compressionText = value.AsWsNormalizedOrEmpty(); }

        public double? HorizontalResolution { get; set; }

        public uint? HorizontalSize { get; set; }

        [NotNull]
        [BackingField(nameof(_imageID))]
        public string ImageID { get => _imageID; set => _imageID = value.AsWsNormalizedOrEmpty(); }

        public short? ResolutionUnit { get; set; }

        public double? VerticalResolution { get; set; }

        public uint? VerticalSize { get; set; }


        #endregion

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalImagePropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalImagePropertiesRow other) => ArePropertiesEqual((IImagePropertiesRow)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IImagePropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IImagePropertiesRow other) => ArePropertiesEqual((IImageProperties)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IImageProperties" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IImageProperties other) => _compressionText == other.CompressionText &&
            _imageID == other.ImageID &&
            BitDepth == other.BitDepth &&
            ColorSpace == other.ColorSpace &&
            CompressedBitsPerPixel == other.CompressedBitsPerPixel &&
            Compression == other.Compression &&
            HorizontalResolution == other.HorizontalResolution &&
            HorizontalSize == other.HorizontalSize &&
            ResolutionUnit == other.ResolutionUnit &&
            VerticalResolution == other.VerticalResolution &&
            VerticalSize == other.VerticalSize;
        //EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
        //LastSynchronizedOn == other.LastSynchronizedOn &&
        //CreatedOn == other.CreatedOn &&
        //ModifiedOn == other.ModifiedOn;

        public abstract bool Equals(IImagePropertiesRow other);

        public abstract bool Equals(IImageProperties other);

        public override int GetHashCode()
        {
            if (TryGetId(out Guid id)) return id.GetHashCode();
            HashCode hash = new();
            hash.Add(_compressionText);
            hash.Add(_imageID);
            hash.Add(BitDepth);
            hash.Add(ColorSpace);
            hash.Add(CompressedBitsPerPixel);
            hash.Add(Compression);
            hash.Add(HorizontalResolution);
            hash.Add(HorizontalSize);
            hash.Add(ResolutionUnit);
            hash.Add(VerticalResolution);
            hash.Add(VerticalSize);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
