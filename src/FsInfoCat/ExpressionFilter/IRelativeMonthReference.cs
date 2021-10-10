namespace FsInfoCat.ExpressionFilter
{
    public interface IRelativeMonthReference : IRelativeTimeReference
    {
        int Years { get; }
        int Months { get; }
    }
}
