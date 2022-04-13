using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public abstract class RecordedTVPropertiesRow : PropertiesRow, ILocalRecordedTVPropertiesRow
    {
        #region Fields

        private string _episodeName;
        private string _networkAffiliation;
        private string _programDescription;
        private string _stationCallSign;
        private string _stationName;

        #endregion

        #region Properties

        public uint? ChannelNumber { get; set; }

        [NotNull]
        public string EpisodeName { get => _episodeName; set => _episodeName = value.AsWsNormalizedOrEmpty(); }

        public bool? IsDTVContent { get; set; }

        public bool? IsHDContent { get; set; }

        [NotNull]
        public string NetworkAffiliation { get => _networkAffiliation; set => _networkAffiliation = value.AsWsNormalizedOrEmpty(); }

        public DateTime? OriginalBroadcastDate { get; set; }

        [NotNull]
        public string ProgramDescription { get => _programDescription; set => _programDescription = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string StationCallSign { get => _stationCallSign; set => _stationCallSign = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string StationName { get => _stationName; set => _stationName = value.AsWsNormalizedOrEmpty(); }

        #endregion

        protected bool ArePropertiesEqual([DisallowNull] IRecordedTVProperties other)
        {
            throw new NotImplementedException();
        }

        public abstract bool Equals(IRecordedTVPropertiesRow other);

        public abstract bool Equals(IRecordedTVProperties other);
    }
}
