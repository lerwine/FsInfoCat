namespace FsInfoCat
{
    [System.Obsolete("Use FsInfoCat.AsyncOps.IBackgroundProgressInfo, instead")]
    public record OperationStatus(MessageCode StatusDescription, string CurrentOperation);
}
