using System;

namespace FsInfoCat
{
    public record AsyncOperationInfo(Guid ConcurrencyId, AsyncJobStatus Status, ActivityCode? Activity, MessageCode? StatusDescription, string CurrentOperation, object AsyncState) : IAsyncOperationInfo;
}
