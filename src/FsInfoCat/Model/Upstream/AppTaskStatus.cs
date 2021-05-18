namespace FsInfoCat.Model.Upstream
{
    public enum AppTaskStatus : byte
    {
        New = 0,
        Accepted = 1,
        Suspended = 2,
        Canceled = 3,
        Completed = 4
    }
}
