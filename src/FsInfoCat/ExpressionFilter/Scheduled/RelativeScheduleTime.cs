using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.ExpressionFilter.Scheduled
{
    public abstract class RelativeScheduleTime : SchedulableTimeReference, IRelativeTimeReference
    {
        private readonly IPropertyChangeTracker<bool> _isHistorical;
        private readonly IPropertyChangeTracker<int> _days;
        private readonly IPropertyChangeTracker<int> _hours;

        public bool IsHistorical { get => _isHistorical.GetValue(); set => _isHistorical.SetValue(value); }

        [Range(0, int.MaxValue, ErrorMessageResourceName = nameof(Properties.Resources.ErrorMessage_InvalidDays), ErrorMessageResourceType = typeof(Properties.Resources))]
        public int Days { get => _days.GetValue(); set => _days.SetValue(value); }

        [Range(0, 23, ErrorMessageResourceName = nameof(Properties.Resources.ErrorMessage_InvalidHours), ErrorMessageResourceType = typeof(Properties.Resources))]
        public int Hours { get => _hours.GetValue(); set => _hours.SetValue(value); }

        public RelativeScheduleTime()
        {
            _isHistorical = AddChangeTracker(nameof(IsHistorical), false);
            _days = AddChangeTracker(nameof(Days), 0);
            _hours = AddChangeTracker(nameof(Hours), 0);
        }

        public abstract bool IsZero();
    }
}
