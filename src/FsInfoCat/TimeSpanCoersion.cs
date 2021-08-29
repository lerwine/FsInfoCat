using System;

namespace FsInfoCat
{
    public class TimeSpanCoersion : ValueCoersion<TimeSpan>
    {
        public static readonly TimeSpanCoersion NormalizedToSeconds = new Seconds();
        public static readonly TimeSpanCoersion NormalizedToMinutes = new Minutes();
        public static readonly TimeSpanCoersion NormalizedToHours = new Hours();

        protected override TimeSpan OnConvert(object obj) => (obj is DateTime dateTime) ? dateTime.TimeOfDay : (obj is DateTimeOffset offset) ? offset.Offset :
            (obj is long ticks) ? new TimeSpan(ticks) : (obj is string s && TimeSpan.TryParse(s, out TimeSpan timeSpan)) ? timeSpan : base.Coerce(obj);

        class Seconds : TimeSpanCoersion
        {
            public override TimeSpan Normalize(TimeSpan obj) => (obj.Milliseconds == 0) ? obj : new(obj.Days, obj.Hours, obj.Minutes, obj.Seconds, 0);
        }

        class Minutes : TimeSpanCoersion
        {
            public override TimeSpan Normalize(TimeSpan obj) => (obj.Milliseconds == 0 && obj.Seconds == 0) ? obj :
                new(obj.Days, obj.Hours, obj.Minutes, 0, 0);
        }

        class Hours : TimeSpanCoersion
        {
            public override TimeSpan Normalize(TimeSpan obj) => (obj.Milliseconds == 0 && obj.Seconds == 0 && obj.Minutes == 0) ? obj :
                new(obj.Days, obj.Hours, 0, 0, 0);
        }
    }

}
