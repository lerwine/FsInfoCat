namespace FsInfoCat.ExpressionFilter
{
    // TODO: Document ITimeRange interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface ITimeRange : IFilter
    {
        TimeReference Start { get; }

        TimeReference End { get; }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
