using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class RecordedTVPropertySet : LocalDbEntity, ILocalRecordedTVPropertySet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<uint?> _channelNumber;
        private readonly IPropertyChangeTracker<string> _episodeName;
        private readonly IPropertyChangeTracker<bool?> _isDTVContent;
        private readonly IPropertyChangeTracker<bool?> _isHDContent;
        private readonly IPropertyChangeTracker<string> _networkAffiliation;
        private readonly IPropertyChangeTracker<DateTime?> _originalBroadcastDate;
        private readonly IPropertyChangeTracker<string> _programDescription;
        private readonly IPropertyChangeTracker<string> _stationCallSign;
        private readonly IPropertyChangeTracker<string> _stationName;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public uint? ChannelNumber { get => _channelNumber.GetValue(); set => _channelNumber.SetValue(value); }
        public string EpisodeName { get => _episodeName.GetValue(); set => _episodeName.SetValue(value); }
        public bool? IsDTVContent { get => _isDTVContent.GetValue(); set => _isDTVContent.SetValue(value); }
        public bool? IsHDContent { get => _isHDContent.GetValue(); set => _isHDContent.SetValue(value); }
        public string NetworkAffiliation { get => _networkAffiliation.GetValue(); set => _networkAffiliation.SetValue(value); }
        public DateTime? OriginalBroadcastDate { get => _originalBroadcastDate.GetValue(); set => _originalBroadcastDate.SetValue(value); }
        public string ProgramDescription { get => _programDescription.GetValue(); set => _programDescription.SetValue(value); }
        public string StationCallSign { get => _stationCallSign.GetValue(); set => _stationCallSign.SetValue(value); }
        public string StationName { get => _stationName.GetValue(); set => _stationName.SetValue(value); }

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

        public RecordedTVPropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _channelNumber = AddChangeTracker<uint?>(nameof(ChannelNumber), null);
            _episodeName = AddChangeTracker<string>(nameof(EpisodeName), null);
            _isDTVContent = AddChangeTracker<bool?>(nameof(IsDTVContent), null);
            _isHDContent = AddChangeTracker<bool?>(nameof(IsHDContent), null);
            _networkAffiliation = AddChangeTracker<string>(nameof(NetworkAffiliation), null);
            _originalBroadcastDate = AddChangeTracker<System.DateTime?>(nameof(OriginalBroadcastDate), null);
            _programDescription = AddChangeTracker<string>(nameof(ProgramDescription), null);
            _stationCallSign = AddChangeTracker<string>(nameof(StationCallSign), null);
            _stationName = AddChangeTracker<string>(nameof(StationName), null);
        }

        public static async Task ApplyAsync(EntityEntry<DbFile> fileEntry, LocalDbContext dbContext, System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
