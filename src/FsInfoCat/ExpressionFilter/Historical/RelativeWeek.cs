using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.ExpressionFilter.Historical
{
    // TODO: Document RelativeWeek class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class RelativeWeek : RelativeHistoricalTime, IRelativeWeekReference
    {
        private readonly IPropertyChangeTracker<int> _weeks;

        [Range(0, int.MaxValue, ErrorMessageResourceName = nameof(Properties.Resources.ErrorMessage_InvalidWeeks), ErrorMessageResourceType = typeof(Properties.Resources))]
        public int Weeks { get => _weeks.GetValue(); set => _weeks.SetValue(value); }

        public RelativeWeek()
        {
            _weeks = AddChangeTracker(nameof(Weeks), 0);
        }

        public override DateTime ToDateTime() => (Weeks == 0) ?
            ((Days == 0) ?
                ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)) :
                ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)).AddDays(0 - Days)
            ) :
            ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)).AddDays(0 - ((Weeks * 7) + Days));

        public override bool IsZero() => Weeks == 0 && Days == 0 && Hours == 9;

        protected override int CompareTo(DateTime other) => ToDateTime().CompareTo(other);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
