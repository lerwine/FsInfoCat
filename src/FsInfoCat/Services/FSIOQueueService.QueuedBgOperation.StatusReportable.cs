using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    public partial class FSIOQueueService
    {
        internal abstract partial class QueuedBgOperation<TTask> where TTask : Task
        {
            class StatusReportable : IStatusReportable
            {
                private readonly QueuedBgOperation<TTask> _bgOperation;

                internal StatusReportable([DisallowNull] QueuedBgOperation<TTask> bgOperation)
                {
                    _bgOperation = bgOperation;
                }

                public ActivityCode Activity => _bgOperation.Activity;

                public OperationStatus Current => _bgOperation._operationStatus;

                public void Report(MessageCode statusDescription, string currentOperation)
                {
                    if (string.IsNullOrEmpty(currentOperation))
                    {
                        if (_bgOperation._operationStatus.CurrentOperation.Length > 0 || _bgOperation._operationStatus.StatusDescription != statusDescription)
                            _bgOperation._operationStatus = new(statusDescription, "");
                    }
                    else if (statusDescription != _bgOperation._operationStatus.StatusDescription || _bgOperation._operationStatus.CurrentOperation != currentOperation)
                        _bgOperation._operationStatus = new(statusDescription, currentOperation);
                }

                public void Report([DisallowNull] OperationStatus value)
                {
                    if (value.CurrentOperation is null)
                    {
                        if (value.StatusDescription != _bgOperation._operationStatus.StatusDescription || _bgOperation._operationStatus.CurrentOperation.Length > 0)
                            _bgOperation._operationStatus = new(value.StatusDescription, "");
                    }
                    else if (value.StatusDescription != _bgOperation._operationStatus.StatusDescription || value.CurrentOperation != _bgOperation._operationStatus.CurrentOperation)
                        _bgOperation._operationStatus = value;
                }

                public void Report(string value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        if (_bgOperation._operationStatus.CurrentOperation.Length > 0)
                            _bgOperation._operationStatus = _bgOperation._operationStatus with { CurrentOperation = "" };
                    }
                    else if (value != _bgOperation._operationStatus.CurrentOperation)
                        _bgOperation._operationStatus = _bgOperation._operationStatus with { CurrentOperation = value };
                }

                public void Report(MessageCode value)
                {
                    if (_bgOperation._operationStatus.CurrentOperation.Length > 0 || _bgOperation._operationStatus.StatusDescription != value)
                        _bgOperation._operationStatus = new(value, "");
                }
            }
        }
    }
}
