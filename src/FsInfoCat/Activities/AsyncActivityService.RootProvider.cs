using System;
using System.Collections.Generic;
using System.Threading;

namespace FsInfoCat.Activities
{
    public partial class AsyncActivityService
    {
        class RootProvider : AsyncActivityProvider
        {
            internal bool IsActive { get; private set; }

            internal Observable<bool>.Source ActiveStatusSource { get; private set; }

            internal RootProvider() : base(null) { }

            protected override LinkedListNode<IAsyncActivity> OnStarting(IAsyncActivity asyncActivity)
            {
                bool isFirst;
                LinkedListNode<IAsyncActivity> node;
                Monitor.Enter(SyncRoot);
                try
                {
                    isFirst = IsEmpty();
                    node = base.OnStarting(asyncActivity);
                    if (isFirst && !IsActive)
                        try
                        {
                            IsActive = true;
                            ActiveStatusSource.RaiseNext(true);
                        }
                        catch
                        {
                            base.OnCompleted(node);
                            IsActive = false;
                            throw;
                        }
                }
                finally { Monitor.Exit(SyncRoot); }
                return node;
            }

            protected override void OnCompleted(LinkedListNode<IAsyncActivity> node)
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    base.OnCompleted(node);
                    if (IsEmpty() && IsActive)
                    {
                        IsActive = false;
                        ActiveStatusSource.RaiseNext(false);
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }
    }
}
