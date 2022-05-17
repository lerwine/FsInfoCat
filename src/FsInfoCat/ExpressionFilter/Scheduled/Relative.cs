using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.ExpressionFilter.Scheduled
{
    // TODO: Document Relative class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Relative : RelativeScheduleTime, IRelativeMonthReference
    {
        private readonly IPropertyChangeTracker<int> _years;
        private readonly IPropertyChangeTracker<int> _months;

        [Range(0, int.MaxValue, ErrorMessageResourceName = nameof(Properties.Resources.ErrorMessage_InvalidYears), ErrorMessageResourceType = typeof(Properties.Resources))]
        public int Years { get => _years.GetValue(); set => _years.SetValue(value); }

        [Range(0, 11, ErrorMessageResourceName = nameof(Properties.Resources.ErrorMessage_InvalidMonths), ErrorMessageResourceType = typeof(Properties.Resources))]
        public int Months { get => _months.GetValue(); set => _months.SetValue(value); }

        public Relative()
        {
            _years = AddChangeTracker(nameof(Years), 0);
            _months = AddChangeTracker(nameof(Months), 0);
        }

        public override DateTime ToDateTime() => (Years == 0) ?
            ((Months == 0) ?
                ((Days == 0) ?
                    ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(IsHistorical ? 0 - Hours : Hours)) :
                    (IsHistorical ?
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)).AddDays(0 - Days) :
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(Hours)).AddDays(Days)
                    )
                ) :
                (IsHistorical ?
                    ((Days == 0) ?
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)) :
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)).AddDays(0 - Days)
                    ).AddMonths(0 - Months) :
                    ((Days == 0) ?
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(Hours)) :
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(Hours)).AddDays(Days)
                    ).AddMonths(Months)
                )
            ) :
            (IsHistorical ?
                ((Months == 0) ?
                    ((Days == 0) ?
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)) :
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)).AddDays(0 - Days)
                    ) :
                    ((Days == 0) ?
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)) :
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)).AddDays(0 - Days)
                    ).AddMonths(0 - Months)
                ).AddYears(0 - Years) :
                ((Months == 0) ?
                    ((Days == 0) ?
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(Hours)) :
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(Hours)).AddDays(Days)
                    ) :
                    ((Days == 0) ?
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(Hours)) :
                        ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(Hours)).AddDays(Days)
                    ).AddMonths(Months)
                ).AddYears(Years)
            );

        public override bool IsZero() => Years == 0 && Months == 0 && Days == 0 && Hours == 9;

        protected override int CompareTo(DateTime other) => ToDateTime().CompareTo(other);
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
