namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public interface ITimeRange : IFilter
    {
        TimeReference Start { get; }

        TimeReference End { get; }
    }
}
