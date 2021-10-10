namespace FsInfoCat.ExpressionFilter
{
    public interface ITimeRange : IFilter
    {
        TimeReference Start { get; }

        TimeReference End { get; }
    }
}
