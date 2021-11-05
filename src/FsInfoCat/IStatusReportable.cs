using System;

namespace FsInfoCat
{
    public interface IStatusReportable : IProgress<OperationStatus>, IProgress<string>, IProgress<MessageCode>
    {
        ActivityCode Activity { get; }

        OperationStatus Current { get; }

        void Report(MessageCode statusDescription, string currentOperation);
    }
}
