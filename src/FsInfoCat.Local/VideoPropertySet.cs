using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class VideoPropertySet : LocalDbEntity, ILocalVideoPropertySet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _compression;
        private readonly IPropertyChangeTracker<MultiStringValue> _director;
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
        public MultiStringValue Director { get => _director.GetValue(); set => _director.SetValue(value); }
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

        public VideoPropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _compression = AddChangeTracker<string>(nameof(Compression), null);
            _director = AddChangeTracker<MultiStringValue>(nameof(Director), null);
            _encodingBitrate = AddChangeTracker<uint?>(nameof(EncodingBitrate), null);
            _frameHeight = AddChangeTracker<uint?>(nameof(FrameHeight), null);
            _frameRate = AddChangeTracker<uint?>(nameof(FrameRate), null);
            _frameWidth = AddChangeTracker<uint?>(nameof(FrameWidth), null);
            _horizontalAspectRatio = AddChangeTracker<uint?>(nameof(HorizontalAspectRatio), null);
            _streamName = AddChangeTracker<string>(nameof(StreamName), null);
            _streamNumber = AddChangeTracker<ushort?>(nameof(StreamNumber), null);
            _verticalAspectRatio = AddChangeTracker<uint?>(nameof(VerticalAspectRatio), null);
        }

        internal static void BuildEntity(EntityTypeBuilder<VideoPropertySet> obj) => obj.Property(nameof(Director)).HasConversion(MultiStringValue.Converter);

        internal static async Task RefreshAsync(EntityEntry<DbFile> entry, IFileDetailProvider fileDetailProvider, CancellationToken cancellationToken)
        {
            VideoPropertySet oldVideoPropertySet = entry.Entity.VideoPropertySetId.HasValue ? await entry.GetRelatedReferenceAsync(f => f.VideoProperties, cancellationToken) : null;
            IVideoProperties currentVideoProperties = await fileDetailProvider.GetVideoPropertiesAsync(cancellationToken);
            // TODO: Implement RefreshAsync(EntityEntry<DbFile>, IFileDetailProvider, CancellationToken)
            throw new NotImplementedException();
        }
    }
}
