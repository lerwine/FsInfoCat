using System;

namespace FsInfoCat.Local
{
    public class DRMPropertiesRow : PropertiesRow, IDRMProperties
    {
        #region Fields

        private readonly IPropertyChangeTracker<DateTime?> _datePlayExpires;
        private readonly IPropertyChangeTracker<DateTime?> _datePlayStarts;
        private readonly IPropertyChangeTracker<string> _description;
        private readonly IPropertyChangeTracker<bool?> _isProtected;
        private readonly IPropertyChangeTracker<uint?> _playCount;

        #endregion

        #region Properties

        public DateTime? DatePlayExpires { get => _datePlayExpires.GetValue(); set => _datePlayExpires.SetValue(value); }
        public DateTime? DatePlayStarts { get => _datePlayStarts.GetValue(); set => _datePlayStarts.SetValue(value); }
        public string Description { get => _description.GetValue(); set => _description.SetValue(value); }
        public bool? IsProtected { get => _isProtected.GetValue(); set => _isProtected.SetValue(value); }
        public uint? PlayCount { get => _playCount.GetValue(); set => _playCount.SetValue(value); }

        #endregion

        public DRMPropertiesRow()
        {
            _datePlayExpires = AddChangeTracker<DateTime?>(nameof(DatePlayExpires), null);
            _datePlayStarts = AddChangeTracker<DateTime?>(nameof(DatePlayStarts), null);
            _description = AddChangeTracker(nameof(Description), null, FilePropertiesComparer.StringValueCoersion);
            _isProtected = AddChangeTracker<bool?>(nameof(IsProtected), null);
            _playCount = AddChangeTracker<uint?>(nameof(PlayCount), null);
        }
    }
}
