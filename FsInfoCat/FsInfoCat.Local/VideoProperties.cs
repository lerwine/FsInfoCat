using System;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Local
{
    public class VideoProperties : LocalDbEntity, ILocalVideoProperties
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _compression;
        private readonly IPropertyChangeTracker<string[]> _director;
        private readonly IPropertyChangeTracker<uint?> _encodingBitrate;
        private readonly IPropertyChangeTracker<uint?> _frameHeight;
        private readonly IPropertyChangeTracker<uint?> _frameRate;
        private readonly IPropertyChangeTracker<uint?> _frameWidth;
        private readonly IPropertyChangeTracker<uint?> _horizontalAspectRatio;
        private readonly IPropertyChangeTracker<string> _streamName;
        private readonly IPropertyChangeTracker<ushort?> _streamNumber;
        private readonly IPropertyChangeTracker<uint?> _verticalAspectRatio;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public string Compression { get => _compression.GetValue(); set => _compression.SetValue(value); }
        public string[] Director { get => _director.GetValue(); set => _director.SetValue(value); }
        public uint? EncodingBitrate { get => _encodingBitrate.GetValue(); set => _encodingBitrate.SetValue(value); }
        public uint? FrameHeight { get => _frameHeight.GetValue(); set => _frameHeight.SetValue(value); }
        public uint? FrameRate { get => _frameRate.GetValue(); set => _frameRate.SetValue(value); }
        public uint? FrameWidth { get => _frameWidth.GetValue(); set => _frameWidth.SetValue(value); }
        public uint? HorizontalAspectRatio { get => _horizontalAspectRatio.GetValue(); set => _horizontalAspectRatio.SetValue(value); }
        public string StreamName { get => _streamName.GetValue(); set => _streamName.SetValue(value); }
        public ushort? StreamNumber { get => _streamNumber.GetValue(); set => _streamNumber.SetValue(value); }
        public uint? VerticalAspectRatio { get => _verticalAspectRatio.GetValue(); set => _verticalAspectRatio.SetValue(value); }

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        #endregion

        public VideoProperties()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _compression = AddChangeTracker<string>(nameof(Compression), null);
            _director = AddChangeTracker<string[]>(nameof(Director), null);
            _encodingBitrate = AddChangeTracker<uint?>(nameof(EncodingBitrate), null);
            _frameHeight = AddChangeTracker<uint?>(nameof(FrameHeight), null);
            _frameRate = AddChangeTracker<uint?>(nameof(FrameRate), null);
            _frameWidth = AddChangeTracker<uint?>(nameof(FrameWidth), null);
            _horizontalAspectRatio = AddChangeTracker<uint?>(nameof(HorizontalAspectRatio), null);
            _streamName = AddChangeTracker<string>(nameof(StreamName), null);
            _streamNumber = AddChangeTracker<ushort?>(nameof(StreamNumber), null);
            _verticalAspectRatio = AddChangeTracker<uint?>(nameof(VerticalAspectRatio), null);
        }
    }
}
