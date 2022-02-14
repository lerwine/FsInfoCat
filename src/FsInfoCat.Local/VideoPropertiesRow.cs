using FsInfoCat.Collections;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public abstract class VideoPropertiesRow : PropertiesRow, IVideoProperties
    {
        #region Fields

        private string _compression = string.Empty;
        private string _streamName = string.Empty;

        #endregion

        #region Properties

        public string Compression { get => _compression; set => _compression = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Director { get; set; }

        public uint? EncodingBitrate { get; set; }

        public uint? FrameHeight { get; set; }

        public uint? FrameRate { get; set; }

        public uint? FrameWidth { get; set; }

        public uint? HorizontalAspectRatio { get; set; }

        public string StreamName { get => _streamName; set => _streamName = value.AsWsNormalizedOrEmpty(); }

        public ushort? StreamNumber { get; set; }

        public uint? VerticalAspectRatio { get; set; }

        #endregion

        protected bool ArePropertiesEqual([DisallowNull] IVideoProperties other)
        {
            throw new NotImplementedException();
        }

        public abstract bool Equals(IVideoProperties other);
    }
}
