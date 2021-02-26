using System;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Test.Helpers
{
    public static class ExtensionMethods
    {
        public static IFuncInvocationResult<R, TResult> Evaluate<T1, T2, T3, R, TResult>(IFuncTestData1<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3)
        {
            InvocationMonitor<R, TResult> monitor = new FuncInvocationMonitor<R, TResult>();
        }
    }
}
