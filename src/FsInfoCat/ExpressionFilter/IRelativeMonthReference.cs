namespace FsInfoCat.ExpressionFilter
{
    // TODO: Document IRelativeMonthReference interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IRelativeMonthReference : IRelativeTimeReference
    {
        int Years { get; }

        int Months { get; }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
