using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class AudioPropertySet : LocalDbEntity, ILocalAudioPropertySet, ISimpleIdentityReference<AudioPropertySet>
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _compression;
        private readonly IPropertyChangeTracker<uint?> _encodingBitrate;
        private readonly IPropertyChangeTracker<string> _format;
        private readonly IPropertyChangeTracker<bool?> _isVariableBitrate;
        private readonly IPropertyChangeTracker<uint?> _sampleRate;
        private readonly IPropertyChangeTracker<uint?> _sampleSize;
        private readonly IPropertyChangeTracker<string> _streamName;
        private readonly IPropertyChangeTracker<ushort?> _streamNumber;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public string Compression { get => _compression.GetValue(); set => _compression.SetValue(value); }
        public uint? EncodingBitrate { get => _encodingBitrate.GetValue(); set => _encodingBitrate.SetValue(value); }
        public string Format { get => _format.GetValue(); set => _format.SetValue(value); }
        public bool? IsVariableBitrate { get => _isVariableBitrate.GetValue(); set => _isVariableBitrate.SetValue(value); }
        public uint? SampleRate { get => _sampleRate.GetValue(); set => _sampleRate.SetValue(value); }
        public uint? SampleSize { get => _sampleSize.GetValue(); set => _sampleSize.SetValue(value); }
        public string StreamName { get => _streamName.GetValue(); set => _streamName.SetValue(value); }
        public ushort? StreamNumber { get => _streamNumber.GetValue(); set => _streamNumber.SetValue(value); }

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        AudioPropertySet IIdentityReference<AudioPropertySet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        public AudioPropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _compression = AddChangeTracker(nameof(Compression), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _encodingBitrate = AddChangeTracker<uint?>(nameof(EncodingBitrate), null);
            _format = AddChangeTracker(nameof(Format), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _isVariableBitrate = AddChangeTracker<bool?>(nameof(IsVariableBitrate), null);
            _sampleRate = AddChangeTracker<uint?>(nameof(SampleRate), null);
            _sampleSize = AddChangeTracker<uint?>(nameof(SampleSize), null);
            _streamName = AddChangeTracker(nameof(StreamName), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _streamNumber = AddChangeTracker<ushort?>(nameof(StreamNumber), null);
        }

        internal static async Task RefreshAsync([DisallowNull] EntityEntry<DbFile> entry, [DisallowNull] IFileDetailProvider fileDetailProvider, CancellationToken cancellationToken)
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
            AudioPropertySet oldPropertySet = (entity = entry.Entity).AudioPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.AudioProperties, cancellationToken) : null;
            IAudioProperties currentProperties = await fileDetailProvider.GetAudioPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            if (currentProperties.IsNullOrAllPropertiesEmpty())
                entity.AudioProperties = null;
            else
                entity.AudioProperties = await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        dbContext.AudioPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
