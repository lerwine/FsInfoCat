namespace FsInfoCat
{
    public enum AsyncJobStatus
    {
        WaitingToRun,
        Running,
        Cancelling,
        Canceled,
        Faulted,
        Succeeded
    }
}
