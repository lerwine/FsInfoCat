using System;

namespace FsInfoCat.ExpressionFilter.Historical
{
    public class Range : TimeRange<HistoricalTimeReference>, IHistoricalTimeRange
    {
        internal static bool AreSame(Range x, Range y) => (x is null) ? (y is null || HistoricalTimeReference.AreSame(y.Start, null)) :
            (ReferenceEquals(x, y) || HistoricalTimeReference.AreSame(x.Start, y?.Start) && HistoricalTimeReference.AreSame(x.End, y?.End));
    }
}
