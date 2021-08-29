using System;

namespace FsInfoCat
{
    public class DateTimeCoersion : ValueCoersion<DateTime>
    {
        public static readonly DateTimeCoersion NormalizedToLocal = new Local();
        public static readonly DateTimeCoersion NormalizedToUtc = new Utc();
        public static readonly DateTimeCoersion NormalizedToSeconds = new Seconds();
        public static readonly DateTimeCoersion NormalizedToSecondsLocal = new SecondsLocal();
        public static readonly DateTimeCoersion NormalizedToSecondsUtc = new SecondsUtc();
        public static readonly DateTimeCoersion NormalizedToMinutes = new Minutes();
        public static readonly DateTimeCoersion NormalizedToMinutesLocal = new MinutesLocal();
        public static readonly DateTimeCoersion NormalizedToMinutesUtc = new MinutesUtc();
        public static readonly DateTimeCoersion NormalizedToHours = new Hours();
        public static readonly DateTimeCoersion NormalizedToHoursLocal = new HoursLocal();
        public static readonly DateTimeCoersion NormalizedToHoursUtc = new HoursUtc();
        public static readonly DateTimeCoersion NormalizedToDays = new Days();
        public static readonly DateTimeCoersion NormalizedToDaysLocal = new DaysLocal();
        public static readonly DateTimeCoersion NormalizedToDaysUtc = new DaysUtc();

        protected override DateTime OnConvert(object obj) => (obj is long ticks) ? new DateTime(ticks) :
            (obj is System.Data.SqlTypes.SqlDateTime sqlDateTime) ? sqlDateTime.Value :
            (obj is string s && DateTime.TryParse(s, out DateTime dateTime)) ? dateTime : base.OnConvert(obj);

        class Local : DateTimeCoersion
        {
            public override DateTime Normalize(DateTime obj) => base.Normalize((obj.Kind == DateTimeKind.Local) ? obj : obj.ToLocalTime());
        }

        class Utc : Seconds
        {
            public override DateTime Normalize(DateTime obj) => base.Normalize((obj.Kind == DateTimeKind.Utc) ? obj : obj.ToUniversalTime());
        }

        class Seconds : DateTimeCoersion
        {
            public override DateTime Normalize(DateTime obj) => (obj.Millisecond == 0) ? obj :
                new DateTime(obj.Year, obj.Month, obj.Day, obj.Hour, obj.Minute, obj.Second, 0, obj.Kind);
        }

        class SecondsLocal : Seconds
        {
            public override DateTime Normalize(DateTime obj) => base.Normalize((obj.Kind == DateTimeKind.Local) ? obj : obj.ToLocalTime());
        }

        class SecondsUtc : Seconds
        {
            public override DateTime Normalize(DateTime obj) => base.Normalize((obj.Kind == DateTimeKind.Utc) ? obj : obj.ToUniversalTime());
        }

        class Minutes : DateTimeCoersion
        {
            public override DateTime Normalize(DateTime obj) => (obj.Millisecond == 0 && obj.Second == 0) ? obj :
                new DateTime(obj.Year, obj.Month, obj.Day, obj.Hour, obj.Minute, 0, 0, obj.Kind);
        }

        class MinutesLocal : Minutes
        {
            public override DateTime Normalize(DateTime obj) => base.Normalize((obj.Kind == DateTimeKind.Local) ? obj : obj.ToLocalTime());
        }

        class MinutesUtc : Minutes
        {
            public override DateTime Normalize(DateTime obj) => base.Normalize((obj.Kind == DateTimeKind.Utc) ? obj : obj.ToUniversalTime());
        }

        class Hours : DateTimeCoersion
        {
            public override DateTime Normalize(DateTime obj) => (obj.Millisecond == 0 && obj.Second == 0 && obj.Minute == 0) ? obj :
                new DateTime(obj.Year, obj.Month, obj.Day, obj.Hour, 0, 0, 0, obj.Kind);
        }

        class HoursLocal : Hours
        {
            public override DateTime Normalize(DateTime obj) => base.Normalize((obj.Kind == DateTimeKind.Local) ? obj : obj.ToLocalTime());
        }

        class HoursUtc : Hours
        {
            public override DateTime Normalize(DateTime obj) => base.Normalize((obj.Kind == DateTimeKind.Utc) ? obj : obj.ToUniversalTime());
        }

        class Days : DateTimeCoersion
        {
            public override DateTime Normalize(DateTime obj) => (obj.Millisecond == 0 && obj.Second == 0 && obj.Minute == 0 && obj.Hour == 0) ? obj : obj.Date;
        }

        class DaysLocal : Days
        {
            public override DateTime Normalize(DateTime obj) => base.Normalize((obj.Kind == DateTimeKind.Local) ? obj : obj.ToLocalTime());
        }

        class DaysUtc : Days
        {
            public override DateTime Normalize(DateTime obj) => base.Normalize((obj.Kind == DateTimeKind.Utc) ? obj : obj.ToUniversalTime());
        }
    }

}
