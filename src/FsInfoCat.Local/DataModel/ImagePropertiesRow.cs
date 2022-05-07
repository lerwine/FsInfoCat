using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
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

        protected bool ArePropertiesEqual([DisallowNull] ILocalImagePropertiesRow other) => ArePropertiesEqual((IImagePropertiesRow)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        protected bool ArePropertiesEqual([DisallowNull] IImagePropertiesRow other) => ArePropertiesEqual((IImageProperties)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

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
}
