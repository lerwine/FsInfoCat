namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public interface IHistoricalTimeRange : ITimeRange
    {
        new Historical.HistoricalTimeReference Start { get; }

        new Historical.HistoricalTimeReference End { get; }
    }
}
