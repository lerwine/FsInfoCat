using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.ExpressionFilter.Historical
{
    public abstract class RelativeHistoricalTime : HistoricalTimeReference, IRelativeTimeReference
    {
        private readonly IPropertyChangeTracker<int> _days;
        private readonly IPropertyChangeTracker<int> _hours;

        [Range(0, int.MaxValue, ErrorMessageResourceName = nameof(Properties.Resources.ErrorMessage_InvalidDays), ErrorMessageResourceType = typeof(Properties.Resources))]
        public int Days { get => _days.GetValue(); set => _days.SetValue(value); }

        [Range(0, 23, ErrorMessageResourceName = nameof(Properties.Resources.ErrorMessage_InvalidHours), ErrorMessageResourceType = typeof(Properties.Resources))]
        public int Hours { get => _hours.GetValue(); set => _hours.SetValue(value); }

        public RelativeHistoricalTime()
        {
            _days = AddChangeTracker(nameof(Days), 0);
            _hours = AddChangeTracker(nameof(Hours), 0);
        }

        public abstract bool IsZero();
    }
}
