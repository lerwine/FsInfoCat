namespace FsInfoCat.ExpressionFilter
{
    // TODO: IScheduleTimeRange Absolute interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IScheduleTimeRange : ITimeRange
    {
        new Scheduled.SchedulableTimeReference Start { get; }

        new Scheduled.SchedulableTimeReference End { get; }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
