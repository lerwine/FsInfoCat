namespace FsInfoCat.ExpressionFilter
{
    public interface IRelativeWeekReference : IRelativeTimeReference
    {
        int Weeks { get; }
    }
}
