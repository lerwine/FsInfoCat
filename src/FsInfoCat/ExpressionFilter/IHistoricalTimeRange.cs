namespace FsInfoCat.ExpressionFilter
{
    // TODO: Document IHistoricalTimeRange interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IHistoricalTimeRange : ITimeRange
    {
        new Historical.HistoricalTimeReference Start { get; }

        new Historical.HistoricalTimeReference End { get; }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
