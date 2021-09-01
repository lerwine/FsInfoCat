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
    public class RecordedTVPropertiesListItem : RecordedTVPropertiesRow, ILocalRecordedTVPropertiesListItem
    {
        private readonly IPropertyChangeTracker<long> _fileCount;

        public long FileCount { get => _fileCount.GetValue(); set => _fileCount.SetValue(value); }

        public RecordedTVPropertiesListItem()
        {
            _fileCount = AddChangeTracker(nameof(FileCount), 0L);
        }
    }
    public class RecordedTVPropertiesRow : PropertiesRow, IRecordedTVProperties
    {
        #region Fields

        private readonly IPropertyChangeTracker<uint?> _channelNumber;
        private readonly IPropertyChangeTracker<string> _episodeName;
        private readonly IPropertyChangeTracker<bool?> _isDTVContent;
        private readonly IPropertyChangeTracker<bool?> _isHDContent;
        private readonly IPropertyChangeTracker<string> _networkAffiliation;
        private readonly IPropertyChangeTracker<DateTime?> _originalBroadcastDate;
        private readonly IPropertyChangeTracker<string> _programDescription;
        private readonly IPropertyChangeTracker<string> _stationCallSign;
        private readonly IPropertyChangeTracker<string> _stationName;

        #endregion

        #region Properties

        public uint? ChannelNumber { get => _channelNumber.GetValue(); set => _channelNumber.SetValue(value); }
        public string EpisodeName { get => _episodeName.GetValue(); set => _episodeName.SetValue(value); }
        public bool? IsDTVContent { get => _isDTVContent.GetValue(); set => _isDTVContent.SetValue(value); }
        public bool? IsHDContent { get => _isHDContent.GetValue(); set => _isHDContent.SetValue(value); }
        public string NetworkAffiliation { get => _networkAffiliation.GetValue(); set => _networkAffiliation.SetValue(value); }
        public DateTime? OriginalBroadcastDate { get => _originalBroadcastDate.GetValue(); set => _originalBroadcastDate.SetValue(value); }
        public string ProgramDescription { get => _programDescription.GetValue(); set => _programDescription.SetValue(value); }
        public string StationCallSign { get => _stationCallSign.GetValue(); set => _stationCallSign.SetValue(value); }
        public string StationName { get => _stationName.GetValue(); set => _stationName.SetValue(value); }

        #endregion

        public RecordedTVPropertiesRow()
        {
            _channelNumber = AddChangeTracker<uint?>(nameof(ChannelNumber), null);
            _episodeName = AddChangeTracker(nameof(EpisodeName), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _isDTVContent = AddChangeTracker<bool?>(nameof(IsDTVContent), null);
            _isHDContent = AddChangeTracker<bool?>(nameof(IsHDContent), null);
            _networkAffiliation = AddChangeTracker(nameof(NetworkAffiliation), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _originalBroadcastDate = AddChangeTracker<DateTime?>(nameof(OriginalBroadcastDate), null);
            _programDescription = AddChangeTracker(nameof(ProgramDescription), null, FilePropertiesComparer.StringValueCoersion);
            _stationCallSign = AddChangeTracker(nameof(StationCallSign), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _stationName = AddChangeTracker(nameof(StationName), null, FilePropertiesComparer.NormalizedStringValueCoersion);
        }
    }
    /// <summary>
    /// Class RecordedTVPropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalRecordedTVPropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalRecordedTVPropertySet" />
    public class RecordedTVPropertySet : RecordedTVPropertiesRow, ILocalRecordedTVPropertySet, ISimpleIdentityReference<RecordedTVPropertySet>
    {
        private HashSet<DbFile> _files = new();

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        RecordedTVPropertySet IIdentityReference<RecordedTVPropertySet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

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
            RecordedTVPropertySet oldPropertySet = (entity = entry.Entity).RecordedTVPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.RecordedTVProperties, cancellationToken) : null;
            IRecordedTVProperties currentProperties = await fileDetailProvider.GetRecordedTVPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            if (currentProperties.IsNullOrAllPropertiesEmpty())
                entity.RecordedTVProperties = null;
            else
                entity.RecordedTVProperties = await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        dbContext.RecordedTVPropertySets.Remove(oldPropertySet);
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
