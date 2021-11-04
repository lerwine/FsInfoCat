namespace FsInfoCat
{
    public record AsyncOperationInfo(AsyncJobStatus Status, ActivityCode? Activity, MessageCode? StatusDescription, string CurrentOperation, object AsyncState) : IAsyncOperationInfo;
}
