namespace FsInfoCat.ExpressionFilter
{
    public interface IScheduleTimeRange : ITimeRange
    {
        new Scheduled.SchedulableTimeReference Start { get; }

        new Scheduled.SchedulableTimeReference End { get; }
    }
}
