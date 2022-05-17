namespace FsInfoCat.ExpressionFilter.Scheduled
{
    // TODO: Document Range class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Range : TimeRange<SchedulableTimeReference>, IScheduleTimeRange
    {
        internal static bool AreSame(Range x, Range y) => (x is null) ? (y is null || SchedulableTimeReference.AreSame(y.Start, null)) :
            (ReferenceEquals(x, y) || SchedulableTimeReference.AreSame(x.Start, y?.Start) && SchedulableTimeReference.AreSame(x.End, y?.End));
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
