using System;

namespace FsInfoCat.Local
{
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
}
