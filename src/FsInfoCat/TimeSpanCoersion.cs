using System;

namespace FsInfoCat;

/// <summary>
/// Class for coersion to <see cref="TimeSpan"/> values.
/// </summary>
public class TimeSpanCoersion : ValueCoersion<TimeSpan>
{
    /// <summary>
    /// Gets the default <see cref="TimeSpanCoersion"/> object where <see cref="ICoersion{T}.Normalize(T)"/> normalizes <see cref="TimeSpan.Milliseconds"/>, <see cref="TimeSpan.Microseconds"/>,
    /// and <see cref="TimeSpan.Nanoseconds"/> to zero.
    /// </summary>
    public static readonly TimeSpanCoersion NormalizeToSeconds = new Seconds();

    /// <summary>
    /// Gets the default <see cref="TimeSpanCoersion"/> object where <see cref="ICoersion{T}.Normalize(T)"/> normalizes <see cref="TimeSpan.Seconds"/>, <see cref="TimeSpan.Milliseconds"/>,
    /// <see cref="TimeSpan.Microseconds"/>, and <see cref="TimeSpan.Nanoseconds"/> to zero.
    /// </summary>
    public static readonly TimeSpanCoersion NormalizedToMinutes = new Minutes();

    /// <summary>
    /// Gets the default <see cref="TimeSpanCoersion"/> object where <see cref="ICoersion{T}.Normalize(T)"/> normalizes <see cref="TimeSpan.Minutes"/>, <see cref="TimeSpan.Seconds"/>,
    /// <see cref="TimeSpan.Milliseconds"/>, <see cref="TimeSpan.Microseconds"/>, and <see cref="TimeSpan.Nanoseconds"/> to zero.
    /// </summary>
    public static readonly TimeSpanCoersion NormalizedToHours = new Hours();

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    protected override TimeSpan OnConvert(object obj) => (obj is DateTime dateTime) ? dateTime.TimeOfDay : (obj is DateTimeOffset offset) ? offset.Offset :
        (obj is long ticks) ? new TimeSpan(ticks) : (obj is string s && TimeSpan.TryParse(s, out TimeSpan timeSpan)) ? timeSpan : base.Coerce(obj);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

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
