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
    /// <summary>
    /// Class MusicPropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalMusicPropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalMusicPropertySet" />
    public class MusicPropertySet : LocalDbEntity, ILocalMusicPropertySet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _albumArtist;
        private readonly IPropertyChangeTracker<string> _albumTitle;
        private readonly IPropertyChangeTracker<MultiStringValue> _artist;
        private readonly IPropertyChangeTracker<uint?> _channelCount;
        private readonly IPropertyChangeTracker<MultiStringValue> _composer;
        private readonly IPropertyChangeTracker<MultiStringValue> _conductor;
        private readonly IPropertyChangeTracker<string> _displayArtist;
        private readonly IPropertyChangeTracker<MultiStringValue> _genre;
        private readonly IPropertyChangeTracker<string> _partOfSet;
        private readonly IPropertyChangeTracker<string> _period;
        private readonly IPropertyChangeTracker<uint?> _trackNumber;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public string AlbumArtist { get => _albumArtist.GetValue(); set => _albumArtist.SetValue(value); }
        public string AlbumTitle { get => _albumTitle.GetValue(); set => _albumTitle.SetValue(value); }
        public MultiStringValue Artist { get => _artist.GetValue(); set => _artist.SetValue(value); }
        public uint? ChannelCount { get => _channelCount.GetValue(); set => _channelCount.SetValue(value); }
        public MultiStringValue Composer { get => _composer.GetValue(); set => _composer.SetValue(value); }
        public MultiStringValue Conductor { get => _conductor.GetValue(); set => _conductor.SetValue(value); }
        public string DisplayArtist { get => _displayArtist.GetValue(); set => _displayArtist.SetValue(value); }
        public MultiStringValue Genre { get => _genre.GetValue(); set => _genre.SetValue(value); }
        public string PartOfSet { get => _partOfSet.GetValue(); set => _partOfSet.SetValue(value); }
        public string Period { get => _period.GetValue(); set => _period.SetValue(value); }
        public uint? TrackNumber { get => _trackNumber.GetValue(); set => _trackNumber.SetValue(value); }

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

        public MusicPropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _albumArtist = AddChangeTracker(nameof(AlbumArtist), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _albumTitle = AddChangeTracker(nameof(AlbumTitle), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _artist = AddChangeTracker<MultiStringValue>(nameof(Artist), null);
            _channelCount = AddChangeTracker<uint?>(nameof(ChannelCount), null);
            _composer = AddChangeTracker<MultiStringValue>(nameof(Composer), null);
            _conductor = AddChangeTracker<MultiStringValue>(nameof(Conductor), null);
            _displayArtist = AddChangeTracker(nameof(DisplayArtist), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _genre = AddChangeTracker<MultiStringValue>(nameof(Genre), null);
            _partOfSet = AddChangeTracker(nameof(PartOfSet), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _period = AddChangeTracker(nameof(Period), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _trackNumber = AddChangeTracker<uint?>(nameof(TrackNumber), null);
        }

        internal static void BuildEntity([DisallowNull] EntityTypeBuilder<MusicPropertySet> builder)
        {
            if (builder is null)
                throw new ArgumentOutOfRangeException(nameof(builder));
            builder.Property(nameof(Artist)).HasConversion(MultiStringValue.Converter);
            builder.Property(nameof(Composer)).HasConversion(MultiStringValue.Converter);
            builder.Property(nameof(Conductor)).HasConversion(MultiStringValue.Converter);
            builder.Property(nameof(Genre)).HasConversion(MultiStringValue.Converter);
        }

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
            MusicPropertySet oldPropertySet = (entity = entry.Entity).MusicPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.MusicProperties, cancellationToken) : null;
            IMusicProperties currentProperties = await fileDetailProvider.GetMusicPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            if (currentProperties.IsNullOrAllPropertiesEmpty())
                entity.MusicProperties = null;
            else
                entity.MusicProperties = await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        dbContext.MusicPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }
    }
}
