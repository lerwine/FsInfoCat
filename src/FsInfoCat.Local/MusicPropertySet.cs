using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class MusicPropertySet : LocalDbEntity, ILocalMusicPropertySet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _albumArtist;
        private readonly IPropertyChangeTracker<string> _albumTitle;
        private readonly IPropertyChangeTracker<string[]> _artist;
        private readonly IPropertyChangeTracker<uint?> _channelCount;
        private readonly IPropertyChangeTracker<string[]> _composer;
        private readonly IPropertyChangeTracker<string[]> _conductor;
        private readonly IPropertyChangeTracker<string> _displayArtist;
        private readonly IPropertyChangeTracker<string[]> _genre;
        private readonly IPropertyChangeTracker<string> _partOfSet;
        private readonly IPropertyChangeTracker<string> _period;
        private readonly IPropertyChangeTracker<uint?> _trackNumber;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public string AlbumArtist { get => _albumArtist.GetValue(); set => _albumArtist.SetValue(value); }
        public string AlbumTitle { get => _albumTitle.GetValue(); set => _albumTitle.SetValue(value); }
        public string[] Artist { get => _artist.GetValue(); set => _artist.SetValue(value); }
        public uint? ChannelCount { get => _channelCount.GetValue(); set => _channelCount.SetValue(value); }
        public string[] Composer { get => _composer.GetValue(); set => _composer.SetValue(value); }
        public string[] Conductor { get => _conductor.GetValue(); set => _conductor.SetValue(value); }
        public string DisplayArtist { get => _displayArtist.GetValue(); set => _displayArtist.SetValue(value); }
        public string[] Genre { get => _genre.GetValue(); set => _genre.SetValue(value); }
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
            _albumArtist = AddChangeTracker<string>(nameof(AlbumArtist), null);
            _albumTitle = AddChangeTracker<string>(nameof(AlbumTitle), null);
            _artist = AddChangeTracker<string[]>(nameof(Artist), null);
            _channelCount = AddChangeTracker<uint?>(nameof(ChannelCount), null);
            _composer = AddChangeTracker<string[]>(nameof(Composer), null);
            _conductor = AddChangeTracker<string[]>(nameof(Conductor), null);
            _displayArtist = AddChangeTracker<string>(nameof(DisplayArtist), null);
            _genre = AddChangeTracker<string[]>(nameof(Genre), null);
            _partOfSet = AddChangeTracker<string>(nameof(PartOfSet), null);
            _period = AddChangeTracker<string>(nameof(Period), null);
            _trackNumber = AddChangeTracker<uint?>(nameof(TrackNumber), null);
        }

        public static async Task ApplyAsync(EntityEntry<DbFile> fileEntry, LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
