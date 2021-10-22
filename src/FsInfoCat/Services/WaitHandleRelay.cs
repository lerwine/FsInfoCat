using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Services
{
    public class WaitHandleRelay : WaitHandle
    {
        public WaitHandleRelay([DisallowNull] WaitHandle backingWaitHandle)
        {
            if ((SafeWaitHandle = (backingWaitHandle ?? throw new ArgumentNullException(nameof(backingWaitHandle))).SafeWaitHandle) is null)
                throw new InvalidOperationException("SaveWaitHandle cannot be null");
        }

        public static WaitHandleRelay CreateManualSetOnly(out Action setDelegate)
        {
            ManualResetEvent manualResetEvent = new(false);
            setDelegate = () => manualResetEvent.Set();
            return new(manualResetEvent);
        }
    }
}
