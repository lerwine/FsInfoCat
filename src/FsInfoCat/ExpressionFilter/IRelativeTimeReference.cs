namespace FsInfoCat.ExpressionFilter
{
    public interface IRelativeTimeReference : ITimeReference
    {
        int Days { get; }
        int Hours { get; }

        bool IsZero();
    }
}
