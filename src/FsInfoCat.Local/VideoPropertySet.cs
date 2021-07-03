using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            _compression = AddChangeTracker(nameof(Compression), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _director = AddChangeTracker<MultiStringValue>(nameof(Director), null);
            _encodingBitrate = AddChangeTracker<uint?>(nameof(EncodingBitrate), null);
            _frameHeight = AddChangeTracker<uint?>(nameof(FrameHeight), null);
            _frameRate = AddChangeTracker<uint?>(nameof(FrameRate), null);
            _frameWidth = AddChangeTracker<uint?>(nameof(FrameWidth), null);
            _horizontalAspectRatio = AddChangeTracker<uint?>(nameof(HorizontalAspectRatio), null);
            _streamName = AddChangeTracker(nameof(StreamName), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _streamNumber = AddChangeTracker<ushort?>(nameof(StreamNumber), null);
            _verticalAspectRatio = AddChangeTracker<uint?>(nameof(VerticalAspectRatio), null);
        }

        internal static void BuildEntity([DisallowNull] EntityTypeBuilder<VideoPropertySet> builder) =>
            (builder ?? throw new ArgumentOutOfRangeException(nameof(builder))).Property(nameof(Director)).HasConversion(MultiStringValue.Converter);

        internal static async Task RefreshAsync([DisallowNull] EntityEntry<DbFile> entry, [DisallowNull] IFileDetailProvider fileDetailProvider,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (entry is null)
                throw new ArgumentNullException(nameof(entry));
            if (fileDetailProvider is null)
                throw new ArgumentNullException(nameof(fileDetailProvider));
            switch (entry.State)
            {
                case EntityState.Detached:
                    throw new ArgumentOutOfRangeException(nameof(entry), $"{nameof(DbFile)} is detached");
                case EntityState.Deleted:
                    throw new ArgumentOutOfRangeException(nameof(entry), $"{nameof(DbFile)} is flagged for deletion");
            }
            if (entry.Context is not LocalDbContext dbContext)
                throw new ArgumentOutOfRangeException(nameof(entry), "Invalid database context");
            DbFile entity;
            VideoPropertySet oldPropertySet = (entity = entry.Entity).VideoPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.VideoProperties, cancellationToken) : null;
            IVideoProperties currentProperties = await fileDetailProvider.GetVideoPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            if (currentProperties.IsNullOrAllPropertiesEmpty())
                entity.VideoProperties = null;
            else
                entity.VideoProperties = await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        dbContext.VideoPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }
    }
}
