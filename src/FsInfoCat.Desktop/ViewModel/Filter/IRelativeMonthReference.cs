namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public interface IRelativeMonthReference : IRelativeTimeReference
    {
        int Years { get; }

        int Months { get; }
    }
}
