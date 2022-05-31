namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public interface IRelativeTimeReference : ITimeReference
    {
        int Days { get; }

        int Hours { get; }

        bool IsZero();
    }
}
