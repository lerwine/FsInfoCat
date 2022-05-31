namespace FsInfoCat.ExpressionFilter
{
    // TODO: Document IRelativeTimeReference interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IRelativeTimeReference : ITimeReference
    {
        int Days { get; }

        int Hours { get; }

        bool IsZero();
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
