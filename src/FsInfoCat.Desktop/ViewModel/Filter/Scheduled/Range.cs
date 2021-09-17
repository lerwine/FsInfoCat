using System;

namespace FsInfoCat.Desktop.ViewModel.Filter.Scheduled
{
    public class Range : TimeRange<SchedulableTimeReference>, IScheduleTimeRange
    {
        internal static bool AreSame(Range x, Range y) => (x is null) ? (y is null || SchedulableTimeReference.AreSame(x.Start, null)) :
            (ReferenceEquals(x, y) || SchedulableTimeReference.AreSame(x.Start, y?.Start) && SchedulableTimeReference.AreSame(x.End, y?.End));
    }
}
