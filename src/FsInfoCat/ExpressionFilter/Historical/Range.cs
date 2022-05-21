namespace FsInfoCat.ExpressionFilter.Historical
{
    // TODO: Absolute Range class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Range : TimeRange<HistoricalTimeReference>, IHistoricalTimeRange
    {
        internal static bool AreSame(Range x, Range y) => (x is null) ? (y is null || HistoricalTimeReference.AreSame(y.Start, null)) :
            (ReferenceEquals(x, y) || HistoricalTimeReference.AreSame(x.Start, y?.Start) && HistoricalTimeReference.AreSame(x.End, y?.End));
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
