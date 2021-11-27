using System;

namespace FsInfoCat.AsyncOps
{
    [Obsolete("Use FsInfoCat.AsyncOps.IBackgroundProgressInfo, instead")]
    public record AsyncOperationInfo(Guid ConcurrencyId, AsyncJobStatus Status, ActivityCode? Activity, MessageCode? StatusDescription, string CurrentOperation, object AsyncState, IAsyncOperationInfo ParentOperation = null) : IAsyncOperationInfo;
}
