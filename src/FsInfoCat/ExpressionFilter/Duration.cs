using System;

namespace FsInfoCat.ExpressionFilter
{
    // TODO: Document Duration class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Duration : NotifyDataErrorInfo, IFilter, IComparable<TimeSpan?>
    {
        private readonly IPropertyChangeTracker<bool> _isExclusive;
        private readonly IPropertyChangeTracker<bool> _includeNull;
        private readonly IPropertyChangeTracker<int?> _days;
        private readonly IPropertyChangeTracker<int?> _hours;
        private readonly IPropertyChangeTracker<int?> _minutes;

        public bool IsExclusive { get => _isExclusive.GetValue(); set => _isExclusive.SetValue(value); }

        public bool IncludeNull { get => _includeNull.GetValue(); set => _includeNull.SetValue(value); }

        public int? Days { get => _days.GetValue(); set => _days.SetValue(value); }

        public int? Hours { get => _hours.GetValue(); set => _hours.SetValue(value); }

        public int? Minutes { get => _minutes.GetValue(); set => _minutes.SetValue(value); }

        public Duration()
        {
            _isExclusive = AddChangeTracker(nameof(IsExclusive), false);
            _includeNull = AddChangeTracker(nameof(IncludeNull), false);
            _days = AddChangeTracker<int?>(nameof(Days), null);
            _hours = AddChangeTracker<int?>(nameof(Hours), null);
            _minutes = AddChangeTracker<int?>(nameof(Minutes), null);
        }

        public TimeSpan ToTimeSpan() => new(Days ?? 0, Hours ?? 0, Minutes ?? 0, 0, 0);

        public int CompareTo(TimeSpan? other) => (other.HasValue)? ToTimeSpan().CompareTo(other.Value) : IncludeNull ? 0 : 1;

        internal static bool AreSame(Duration x, Duration y)
        {
            if (x is null)
                return y is null || (y.Days == 0 && y.Hours == 0 && y.Minutes == 0 && !y.IncludeNull);
            if (y is null)
                return x.Days == 0 && x.Hours == 0 && x.Minutes == 0 && !x.IncludeNull;
            return ReferenceEquals(x, y) || (x.Days == y.Days && x.Hours == y.Hours && x.Minutes == y.Minutes && x.IncludeNull == y.IncludeNull);
        }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
