using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    public partial class FSIOQueueService
    {
        internal abstract partial class QueuedBgOperation<TState, TTask>
        {
            class StatusReportable : IStatusReportable
            {
                private readonly QueuedBgOperation<TState, TTask> _bgOperation;
                private readonly IProgress<IAsyncOperationInfo> _progressHandler;

                internal StatusReportable([DisallowNull] QueuedBgOperation<TState, TTask> bgOperation, IProgress<IAsyncOperationInfo> progressHandler, CancellationToken cancellationToken)
                {
                    _bgOperation = bgOperation ?? throw new ArgumentNullException(nameof(bgOperation));
                    _progressHandler = progressHandler;
                    Token = cancellationToken;
                }

                public ActivityCode Activity => _bgOperation.Activity;

                public OperationStatus Current => _bgOperation._operationStatus;

                public CancellationToken Token { get; }

                public void Report(MessageCode statusDescription, string currentOperation)
                {
                    OperationStatus operationStatus;
                    Monitor.Enter(_bgOperation.SyncRoot);
                    try
                    {
                        if (string.IsNullOrEmpty(currentOperation))
                        {
                            if (_bgOperation._operationStatus.CurrentOperation.Length > 0 || _bgOperation._operationStatus.StatusDescription != statusDescription)
                                _bgOperation._operationStatus = operationStatus = new(statusDescription, "");
                            else
                                return;
                        }
                        else if (statusDescription != _bgOperation._operationStatus.StatusDescription || _bgOperation._operationStatus.CurrentOperation != currentOperation)
                            _bgOperation._operationStatus = operationStatus = new(statusDescription, currentOperation);
                        else
                            return;
                    }
                    finally { Monitor.Exit(_bgOperation.SyncRoot); }
                    _progressHandler?.Report(new AsyncOperationInfo(_bgOperation.ConcurrencyId, _bgOperation.Status, _bgOperation.Activity, operationStatus.StatusDescription, operationStatus.CurrentOperation,
                        _bgOperation.AsyncState, _bgOperation.ParentOperation));
                }

                public void Report([DisallowNull] OperationStatus value)
                {
                    OperationStatus operationStatus;
                    Monitor.Enter(_bgOperation.SyncRoot);
                    try
                    {
                        if (value.CurrentOperation is null)
                        {
                            if (value.StatusDescription != _bgOperation._operationStatus.StatusDescription || _bgOperation._operationStatus.CurrentOperation.Length > 0)
                                _bgOperation._operationStatus = operationStatus = new(value.StatusDescription, "");
                            else
                                return;
                        }
                        else if (value.StatusDescription != _bgOperation._operationStatus.StatusDescription || value.CurrentOperation != _bgOperation._operationStatus.CurrentOperation)
                            _bgOperation._operationStatus = operationStatus = value;
                        else
                            return;
                    }
                    finally { Monitor.Exit(_bgOperation.SyncRoot); }
                    _progressHandler?.Report(new AsyncOperationInfo(_bgOperation.ConcurrencyId, _bgOperation.Status, _bgOperation.Activity, operationStatus.StatusDescription, operationStatus.CurrentOperation,
                        _bgOperation.AsyncState, _bgOperation.ParentOperation));
                }

                public void Report(string value)
                {
                    OperationStatus operationStatus;
                    Monitor.Enter(_bgOperation.SyncRoot);
                    try
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            if (_bgOperation._operationStatus.CurrentOperation.Length > 0)
                                _bgOperation._operationStatus = operationStatus = _bgOperation._operationStatus with { CurrentOperation = "" };
                            else
                                return;
                        }
                        else if (value != _bgOperation._operationStatus.CurrentOperation)
                            _bgOperation._operationStatus = operationStatus = _bgOperation._operationStatus with { CurrentOperation = value };
                        else
                            return;
                    }
                    finally { Monitor.Exit(_bgOperation.SyncRoot); }
                    _progressHandler?.Report(new AsyncOperationInfo(_bgOperation.ConcurrencyId, _bgOperation.Status, _bgOperation.Activity, operationStatus.StatusDescription, operationStatus.CurrentOperation,
                        _bgOperation.AsyncState, _bgOperation.ParentOperation));
                }

                public void Report(MessageCode value)
                {
                    OperationStatus operationStatus;
                    Monitor.Enter(_bgOperation.SyncRoot);
                    try
                    {
                        if (_bgOperation._operationStatus.CurrentOperation.Length > 0 || _bgOperation._operationStatus.StatusDescription != value)
                            _bgOperation._operationStatus = operationStatus = new(value, "");
                        else
                            return;
                    }
                    finally { Monitor.Exit(_bgOperation.SyncRoot); }
                    _progressHandler?.Report(new AsyncOperationInfo(_bgOperation.ConcurrencyId, _bgOperation.Status, _bgOperation.Activity, operationStatus.StatusDescription, operationStatus.CurrentOperation,
                        _bgOperation.AsyncState, _bgOperation.ParentOperation));
                }
            }
        }
    }
}
